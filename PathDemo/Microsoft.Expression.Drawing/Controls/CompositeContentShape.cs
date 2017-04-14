using Microsoft.Expression.Drawing.Core;
using Microsoft.Expression.Media;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Expression.Controls
{
	public abstract class CompositeContentShape : ContentControl, IGeometrySourceParameters, IShape
	{
		public readonly static DependencyProperty FillProperty;

		public readonly static DependencyProperty StrokeProperty;

		public readonly static DependencyProperty StrokeThicknessProperty;

		public readonly static DependencyProperty StretchProperty;

		public readonly static DependencyProperty StrokeStartLineCapProperty;

		public readonly static DependencyProperty StrokeEndLineCapProperty;

		public readonly static DependencyProperty StrokeLineJoinProperty;

		public readonly static DependencyProperty StrokeMiterLimitProperty;

		public readonly static DependencyProperty StrokeDashArrayProperty;

		public readonly static DependencyProperty StrokeDashCapProperty;

		public readonly static DependencyProperty StrokeDashOffsetProperty;

		public readonly static DependencyProperty InternalContentProperty;

		private IGeometrySource geometrySource;

		private bool realizeGeometryScheduled;

		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(CompositeContentShape.FillProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.FillProperty, (object)value);
			}
		}

		public Thickness GeometryMargin
		{
			get
			{
				if (this.PartPath == null || this.PartPath.Data == null)
				{
					return new Thickness();
				}
				return this.GeometrySource.LogicalBounds.Subtract(this.PartPath.Data.Bounds);
			}
		}

		private IGeometrySource GeometrySource
		{
			get
			{
				IGeometrySource geometrySource = this.geometrySource;
				if (geometrySource == null)
				{
					IGeometrySource geometrySource1 = this.CreateGeometrySource();
					IGeometrySource geometrySource2 = geometrySource1;
					this.geometrySource = geometrySource1;
					geometrySource = geometrySource2;
				}
				return geometrySource;
			}
		}

		public object InternalContent
		{
			get
			{
				return base.GetValue(CompositeContentShape.InternalContentProperty);
			}
			private set
			{
				base.SetValue(CompositeContentShape.InternalContentProperty, value);
			}
		}

		private Path PartPath
		{
			get;
			set;
		}

		public Geometry RenderedGeometry
		{
			get
			{
				return this.GeometrySource.Geometry;
			}
		}

		public System.Windows.Media.Stretch Stretch
		{
			get
			{
				return (System.Windows.Media.Stretch)base.GetValue(CompositeContentShape.StretchProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StretchProperty, value);
			}
		}

		public Brush Stroke
		{
			get
			{
				return (Brush)base.GetValue(CompositeContentShape.StrokeProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeProperty, (object)value);
			}
		}

		public DoubleCollection StrokeDashArray
		{
			get
			{
				return (DoubleCollection)base.GetValue(CompositeContentShape.StrokeDashArrayProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeDashArrayProperty, (object)value);
			}
		}

		public PenLineCap StrokeDashCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeContentShape.StrokeDashCapProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeDashCapProperty, value);
			}
		}

		public double StrokeDashOffset
		{
			get
			{
				return (double)base.GetValue(CompositeContentShape.StrokeDashOffsetProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeDashOffsetProperty, value);
			}
		}

		public PenLineCap StrokeEndLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeContentShape.StrokeEndLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeEndLineCapProperty, value);
			}
		}

		public PenLineJoin StrokeLineJoin
		{
			get
			{
				return (PenLineJoin)base.GetValue(CompositeContentShape.StrokeLineJoinProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeLineJoinProperty, value);
			}
		}

		public double StrokeMiterLimit
		{
			get
			{
				return (double)base.GetValue(CompositeContentShape.StrokeMiterLimitProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeMiterLimitProperty, value);
			}
		}

		public PenLineCap StrokeStartLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeContentShape.StrokeStartLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeStartLineCapProperty, value);
			}
		}

		public double StrokeThickness
		{
			get
			{
				return (double)base.GetValue(CompositeContentShape.StrokeThicknessProperty);
			}
			set
			{
				base.SetValue(CompositeContentShape.StrokeThicknessProperty, value);
			}
		}

		static CompositeContentShape()
		{
			CompositeContentShape.FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(CompositeContentShape), new PropertyMetadata(null));
			CompositeContentShape.StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(CompositeContentShape), new PropertyMetadata(null));
			CompositeContentShape.StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(CompositeContentShape), new DrawingPropertyMetadata((object)1, DrawingPropertyMetadataOptions.AffectsRender));
			CompositeContentShape.StretchProperty = DependencyProperty.Register("Stretch", typeof(System.Windows.Media.Stretch), typeof(CompositeContentShape), new DrawingPropertyMetadata((object)System.Windows.Media.Stretch.Fill, DrawingPropertyMetadataOptions.AffectsRender));
			CompositeContentShape.StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(CompositeContentShape), new PropertyMetadata((object)PenLineCap.Flat));
			CompositeContentShape.StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(CompositeContentShape), new PropertyMetadata((object)PenLineCap.Flat));
			CompositeContentShape.StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(CompositeContentShape), new PropertyMetadata((object)PenLineJoin.Miter));
			CompositeContentShape.StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(CompositeContentShape), new PropertyMetadata((object)10));
			CompositeContentShape.StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(CompositeContentShape), new PropertyMetadata(null));
			CompositeContentShape.StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(CompositeContentShape), new PropertyMetadata((object)PenLineCap.Flat));
			CompositeContentShape.StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(CompositeContentShape), new PropertyMetadata((object)0));
			CompositeContentShape.InternalContentProperty = DependencyProperty.Register("InternalContent", typeof(object), typeof(CompositeContentShape), new PropertyMetadata(null));
		}

		protected CompositeContentShape()
		{
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this.GeometrySource.UpdateGeometry(this, finalSize.Bounds()) && !this.realizeGeometryScheduled)
			{
				this.realizeGeometryScheduled = true;
				base.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
			}
			return base.ArrangeOverride(finalSize);
		}

		protected abstract IGeometrySource CreateGeometrySource();

		public void InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if (this.GeometrySource.InvalidateGeometry(reasons))
			{
				base.InvalidateArrange();
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.PartPath = this.FindVisualDesendent<Path>((Path child) => child.Name == "PART_Path").FirstOrDefault<Path>();
			this.GeometrySource.InvalidateGeometry(InvalidateGeometryReasons.TemplateChanged);
		}

		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			IFormattable content = base.Content as IFormattable;
			string str = base.Content as string;
			if (content == null && str == null)
			{
				this.InternalContent = base.Content;
				return;
			}
			TextBlock internalContent = this.InternalContent as TextBlock;
			if (internalContent == null)
			{
				TextBlock textBlock = new TextBlock();
				internalContent = textBlock;
				this.InternalContent = textBlock;
			}
			internalContent.TextAlignment = TextAlignment.Center;
			internalContent.TextWrapping = TextWrapping.Wrap;
			internalContent.TextTrimming = TextTrimming.WordEllipsis;
			internalContent.Text = str ?? content.ToString(null, null);
		}

		private new void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.realizeGeometryScheduled = false;
			base.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
			this.RealizeGeometry();
		}

		private void RealizeGeometry()
		{
			if (this.PartPath != null)
			{
				this.PartPath.Data = this.RenderedGeometry.CloneCurrentValue();
				this.PartPath.Margin = this.GeometryMargin;
			}
			if (this.RenderedGeometryChanged != null)
			{
				this.RenderedGeometryChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler RenderedGeometryChanged;
	}
}