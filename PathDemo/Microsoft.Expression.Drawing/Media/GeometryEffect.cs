using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	[TypeConverter(typeof(GeometryEffectConverter))]
	public abstract class GeometryEffect : DependencyObject
	{
		public readonly static DependencyProperty GeometryEffectProperty;

		private static GeometryEffect defaultGeometryEffect;

		protected Geometry cachedGeometry;

		private bool effectInvalidated;

		public static GeometryEffect DefaultGeometryEffect
		{
			get
			{
				GeometryEffect noGeometryEffect = GeometryEffect.defaultGeometryEffect;
				if (noGeometryEffect == null)
				{
					noGeometryEffect = new GeometryEffect.NoGeometryEffect();
					GeometryEffect.defaultGeometryEffect = noGeometryEffect;
				}
				return noGeometryEffect;
			}
		}

		public Geometry OutputGeometry
		{
			get
			{
				return this.cachedGeometry;
			}
		}

		protected internal DependencyObject Parent
		{
			get;
			private set;
		}

		static GeometryEffect()
		{
			GeometryEffect.GeometryEffectProperty = DependencyProperty.RegisterAttached("GeometryEffect", typeof(GeometryEffect), typeof(GeometryEffect), new DrawingPropertyMetadata(GeometryEffect.DefaultGeometryEffect, DrawingPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(GeometryEffect.OnGeometryEffectChanged)));
			DrawingPropertyMetadata.DrawingPropertyChanged += new EventHandler<DrawingPropertyChangedEventArgs>((object sender, DrawingPropertyChangedEventArgs args) => {
				GeometryEffect geometryEffect = sender as GeometryEffect;
				if (geometryEffect != null && args.Metadata.AffectsRender)
				{
					geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.PropertyChanged);
				}
			});
		}

		protected GeometryEffect()
		{
		}

		protected internal virtual void Attach(DependencyObject obj)
		{
			if (this.Parent != null)
			{
				this.Detach();
			}
			this.effectInvalidated = true;
			this.cachedGeometry = null;
			if (GeometryEffect.InvalidateParent(obj))
			{
				this.Parent = obj;
			}
		}

		public GeometryEffect CloneCurrentValue()
		{
			return this.DeepCopy();
		}

		protected abstract GeometryEffect DeepCopy();

		protected internal virtual void Detach()
		{
			this.effectInvalidated = true;
			this.cachedGeometry = null;
			if (this.Parent != null)
			{
				GeometryEffect.InvalidateParent(this.Parent);
				this.Parent = null;
			}
		}

		public abstract bool Equals(GeometryEffect geometryEffect);

		public static GeometryEffect GetGeometryEffect(DependencyObject obj)
		{
			return (GeometryEffect)obj.GetValue(GeometryEffect.GeometryEffectProperty);
		}

		public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if (this.effectInvalidated)
			{
				return false;
			}
			this.effectInvalidated = true;
			if (reasons != InvalidateGeometryReasons.ParentInvalidated)
			{
				GeometryEffect.InvalidateParent(this.Parent);
			}
			return true;
		}

		private static bool InvalidateParent(DependencyObject parent)
		{
			IShape shape = parent as IShape;
			if (shape != null)
			{
				shape.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
				return true;
			}
			GeometryEffect geometryEffect = parent as GeometryEffect;
			if (geometryEffect == null)
			{
				return false;
			}
			geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
			return true;
		}

		private static void OnGeometryEffectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			GeometryEffect oldValue = e.OldValue as GeometryEffect;
			GeometryEffect newValue = e.NewValue as GeometryEffect;
			if (oldValue != newValue)
			{
				if (oldValue != null && obj.Equals(oldValue.Parent))
				{
					oldValue.Detach();
				}
				if (newValue != null)
				{
					if (newValue.Parent != null)
					{
						GeometryEffect geometryEffect = newValue.CloneCurrentValue();
						obj.SetValue(GeometryEffect.GeometryEffectProperty, (object)geometryEffect);
						return;
					}
					newValue.Attach(obj);
				}
			}
		}

		public bool ProcessGeometry(Geometry input)
		{
			bool flag = false;
			if (this.effectInvalidated)
			{
				flag = flag | this.UpdateCachedGeometry(input);
				this.effectInvalidated = false;
			}
			return flag;
		}

		public static void SetGeometryEffect(DependencyObject obj, GeometryEffect value)
		{
			obj.SetValue(GeometryEffect.GeometryEffectProperty, (object)value);
		}

		protected abstract bool UpdateCachedGeometry(Geometry input);

		private class NoGeometryEffect : GeometryEffect
		{
			public NoGeometryEffect()
			{
			}

			protected override GeometryEffect DeepCopy()
			{
				return new GeometryEffect.NoGeometryEffect();
			}

			public override bool Equals(GeometryEffect geometryEffect)
			{
				if (geometryEffect == null)
				{
					return true;
				}
				return geometryEffect is GeometryEffect.NoGeometryEffect;
			}

			protected override bool UpdateCachedGeometry(Geometry input)
			{
				this.cachedGeometry = input;
				return false;
			}
		}
	}
}