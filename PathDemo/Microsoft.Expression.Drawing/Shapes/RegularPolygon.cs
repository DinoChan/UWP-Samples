using Microsoft.Expression.Media;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Expression.Shapes
{
	public sealed class RegularPolygon : PrimitiveShape, IPolygonGeometrySourceParameters, IGeometrySourceParameters
	{
		public readonly static DependencyProperty PointCountProperty;

		public readonly static DependencyProperty InnerRadiusProperty;

		public double InnerRadius
		{
			get
			{
				return JustDecompileGenerated_get_InnerRadius();
			}
			set
			{
				JustDecompileGenerated_set_InnerRadius(value);
			}
		}

		public double JustDecompileGenerated_get_InnerRadius()
		{
			return (double)base.GetValue(RegularPolygon.InnerRadiusProperty);
		}

		public void JustDecompileGenerated_set_InnerRadius(double value)
		{
			base.SetValue(RegularPolygon.InnerRadiusProperty, value);
		}

		public double PointCount
		{
			get
			{
				return JustDecompileGenerated_get_PointCount();
			}
			set
			{
				JustDecompileGenerated_set_PointCount(value);
			}
		}

		public double JustDecompileGenerated_get_PointCount()
		{
			return (double)base.GetValue(RegularPolygon.PointCountProperty);
		}

		public void JustDecompileGenerated_set_PointCount(double value)
		{
			base.SetValue(RegularPolygon.PointCountProperty, value);
		}

		static RegularPolygon()
		{
			RegularPolygon.PointCountProperty = DependencyProperty.Register("PointCount", typeof(double), typeof(RegularPolygon), new DrawingPropertyMetadata(6d, DrawingPropertyMetadataOptions.AffectsRender));
			RegularPolygon.InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(RegularPolygon), new DrawingPropertyMetadata(1d, DrawingPropertyMetadataOptions.AffectsRender));
		}

		public RegularPolygon()
		{
		}

		protected override IGeometrySource CreateGeometrySource()
		{
			return new PolygonGeometrySource();
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