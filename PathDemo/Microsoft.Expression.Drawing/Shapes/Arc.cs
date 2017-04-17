using Microsoft.Expression.Media;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Expression.Shapes
{
	public sealed class Arc : PrimitiveShape, IArcGeometrySourceParameters, IGeometrySourceParameters
	{
		public readonly static DependencyProperty StartAngleProperty;

		public readonly static DependencyProperty EndAngleProperty;

		public readonly static DependencyProperty ArcThicknessProperty;

		public readonly static DependencyProperty ArcThicknessUnitProperty;

		public double ArcThickness
		{
			get
			{
				return JustDecompileGenerated_get_ArcThickness();
			}
			set
			{
				JustDecompileGenerated_set_ArcThickness(value);
			}
		}

		public double JustDecompileGenerated_get_ArcThickness()
		{
			return (double)base.GetValue(Arc.ArcThicknessProperty);
		}

		public void JustDecompileGenerated_set_ArcThickness(double value)
		{
			base.SetValue(Arc.ArcThicknessProperty, value);
		}

		public UnitType ArcThicknessUnit
		{
			get
			{
				return JustDecompileGenerated_get_ArcThicknessUnit();
			}
			set
			{
				JustDecompileGenerated_set_ArcThicknessUnit(value);
			}
		}

		public UnitType JustDecompileGenerated_get_ArcThicknessUnit()
		{
			return (UnitType)base.GetValue(Arc.ArcThicknessUnitProperty);
		}

		public void JustDecompileGenerated_set_ArcThicknessUnit(UnitType value)
		{
			base.SetValue(Arc.ArcThicknessUnitProperty, value);
		}

		public double EndAngle
		{
			get
			{
				return JustDecompileGenerated_get_EndAngle();
			}
			set
			{
				JustDecompileGenerated_set_EndAngle(value);
			}
		}

		public double JustDecompileGenerated_get_EndAngle()
		{
			return (double)base.GetValue(Arc.EndAngleProperty);
		}

		public void JustDecompileGenerated_set_EndAngle(double value)
		{
			base.SetValue(Arc.EndAngleProperty, value);
		}

		public double StartAngle
		{
			get
			{
				return JustDecompileGenerated_get_StartAngle();
			}
			set
			{
				JustDecompileGenerated_set_StartAngle(value);
			}
		}

		public double JustDecompileGenerated_get_StartAngle()
		{
			return (double)base.GetValue(Arc.StartAngleProperty);
		}

		public void JustDecompileGenerated_set_StartAngle(double value)
		{
			base.SetValue(Arc.StartAngleProperty, value);
		}

		static Arc()
		{
		    try
		    {
		        Arc.StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new DrawingPropertyMetadata(0d, DrawingPropertyMetadataOptions.AffectsRender));
		        Arc.EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new DrawingPropertyMetadata(90d, DrawingPropertyMetadataOptions.AffectsRender));
		        Arc.ArcThicknessProperty = DependencyProperty.Register("ArcThickness", typeof(double), typeof(Arc), new DrawingPropertyMetadata(0d, DrawingPropertyMetadataOptions.AffectsRender));
		        Arc.ArcThicknessUnitProperty = DependencyProperty.Register("ArcThicknessUnit", typeof(UnitType), typeof(Arc), new DrawingPropertyMetadata(UnitType.Pixel, DrawingPropertyMetadataOptions.AffectsRender));
		    }
		    catch (Exception ex)
		    {

		    }
		}

		public Arc()
		{
		}

		protected override IGeometrySource CreateGeometrySource()
		{
			return new ArcGeometrySource();
		}

		System.Windows.Media.Stretch Microsoft.Expression.Media.IGeometrySourceParameters.Stretch
		{
		    get { return base.Stretch; }
		}

		Brush Microsoft.Expression.Media.IGeometrySourceParameters.Stroke
		{
		    get { return base.Stroke; }
		}

		double Microsoft.Expression.Media.IGeometrySourceParameters.StrokeThickness
		{
		    get { return base.StrokeThickness; }
		}

        protected override Size MeasureOverride(Size availableSize)
        {
            Debug.WriteLine("MeasureOverride");
            return base.MeasureOverride(availableSize);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            Debug.WriteLine("ArrangeOverride");
            return base.ArrangeOverride(finalSize);
        }

    }
}