using Microsoft.Expression.Media;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Expression.Shapes
{
	public sealed class BlockArrow : PrimitiveShape, IBlockArrowGeometrySourceParameters, IGeometrySourceParameters
	{
		public readonly static DependencyProperty OrientationProperty;

		public readonly static DependencyProperty ArrowheadAngleProperty;

		public readonly static DependencyProperty ArrowBodySizeProperty;

		public double ArrowBodySize
		{
			get
			{
				return JustDecompileGenerated_get_ArrowBodySize();
			}
			set
			{
				JustDecompileGenerated_set_ArrowBodySize(value);
			}
		}

		public double JustDecompileGenerated_get_ArrowBodySize()
		{
			return (double)base.GetValue(BlockArrow.ArrowBodySizeProperty);
		}

		public void JustDecompileGenerated_set_ArrowBodySize(double value)
		{
			base.SetValue(BlockArrow.ArrowBodySizeProperty, value);
		}

		public double ArrowheadAngle
		{
			get
			{
				return JustDecompileGenerated_get_ArrowheadAngle();
			}
			set
			{
				JustDecompileGenerated_set_ArrowheadAngle(value);
			}
		}

		public double JustDecompileGenerated_get_ArrowheadAngle()
		{
			return (double)base.GetValue(BlockArrow.ArrowheadAngleProperty);
		}

		public void JustDecompileGenerated_set_ArrowheadAngle(double value)
		{
			base.SetValue(BlockArrow.ArrowheadAngleProperty, value);
		}

		public ArrowOrientation Orientation
		{
			get
			{
				return JustDecompileGenerated_get_Orientation();
			}
			set
			{
				JustDecompileGenerated_set_Orientation(value);
			}
		}

		public ArrowOrientation JustDecompileGenerated_get_Orientation()
		{
			return (ArrowOrientation)base.GetValue(BlockArrow.OrientationProperty);
		}

		public void JustDecompileGenerated_set_Orientation(ArrowOrientation value)
		{
			base.SetValue(BlockArrow.OrientationProperty, value);
		}

		static BlockArrow()
		{
			BlockArrow.OrientationProperty = DependencyProperty.Register("Orientation", typeof(ArrowOrientation), typeof(BlockArrow), new DrawingPropertyMetadata((object)ArrowOrientation.Right, DrawingPropertyMetadataOptions.AffectsRender));
			BlockArrow.ArrowheadAngleProperty = DependencyProperty.Register("ArrowheadAngle", typeof(double), typeof(BlockArrow), new DrawingPropertyMetadata((object)90, DrawingPropertyMetadataOptions.AffectsRender));
			BlockArrow.ArrowBodySizeProperty = DependencyProperty.Register("ArrowBodySize", typeof(double), typeof(BlockArrow), new DrawingPropertyMetadata((object)0.5, DrawingPropertyMetadataOptions.AffectsRender));
		}

		public BlockArrow()
		{
		}

		protected override IGeometrySource CreateGeometrySource()
		{
			return new BlockArrowGeometrySource();
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
	}
}