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
	public abstract class CompositeShape : Control, IGeometrySourceParameters, IShape
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

		private IGeometrySource geometrySource;

		private bool realizeGeometryScheduled;

		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(CompositeShape.FillProperty);
			}
			set
			{
				base.SetValue(CompositeShape.FillProperty, (object)value);
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
				return (System.Windows.Media.Stretch)base.GetValue(CompositeShape.StretchProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StretchProperty, value);
			}
		}

		public Brush Stroke
		{
			get
			{
				return (Brush)base.GetValue(CompositeShape.StrokeProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeProperty, (object)value);
			}
		}

		public DoubleCollection StrokeDashArray
		{
			get
			{
				return (DoubleCollection)base.GetValue(CompositeShape.StrokeDashArrayProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashArrayProperty, (object)value);
			}
		}

		public PenLineCap StrokeDashCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeDashCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashCapProperty, value);
			}
		}

		public double StrokeDashOffset
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeDashOffsetProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashOffsetProperty, value);
			}
		}

		public PenLineCap StrokeEndLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeEndLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeEndLineCapProperty, value);
			}
		}

		public PenLineJoin StrokeLineJoin
		{
			get
			{
				return (PenLineJoin)base.GetValue(CompositeShape.StrokeLineJoinProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeLineJoinProperty, value);
			}
		}

		public double StrokeMiterLimit
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeMiterLimitProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeMiterLimitProperty, value);
			}
		}

		public PenLineCap StrokeStartLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeStartLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeStartLineCapProperty, value);
			}
		}

		public double StrokeThickness
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeThicknessProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeThicknessProperty, value);
			}
		}

		static CompositeShape()
		{
			CompositeShape.FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(CompositeShape), new PropertyMetadata(null));
			CompositeShape.StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(CompositeShape), new PropertyMetadata(null));
			CompositeShape.StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(CompositeShape), new DrawingPropertyMetadata((object)1, DrawingPropertyMetadataOptions.AffectsRender));
			CompositeShape.StretchProperty = DependencyProperty.Register("Stretch", typeof(System.Windows.Media.Stretch), typeof(CompositeShape), new DrawingPropertyMetadata((object)System.Windows.Media.Stretch.Fill, DrawingPropertyMetadataOptions.AffectsRender));
			CompositeShape.StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata((object)PenLineCap.Flat));
			CompositeShape.StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata((object)PenLineCap.Flat));
			CompositeShape.StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(CompositeShape), new PropertyMetadata((object)PenLineJoin.Miter));
			CompositeShape.StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(CompositeShape), new PropertyMetadata((object)10));
			CompositeShape.StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(CompositeShape), new PropertyMetadata(null));
			CompositeShape.StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata((object)PenLineCap.Flat));
			CompositeShape.StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(CompositeShape), new PropertyMetadata((object)0));
		}

		protected CompositeShape()
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