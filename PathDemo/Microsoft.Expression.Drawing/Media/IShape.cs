using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	public interface IShape
	{
		Brush Fill
		{
			get;
			set;
		}

		Thickness GeometryMargin
		{
			get;
		}

		Geometry RenderedGeometry
		{
			get;
		}

		System.Windows.Media.Stretch Stretch
		{
			get;
			set;
		}

		Brush Stroke
		{
			get;
			set;
		}

		double StrokeThickness
		{
			get;
			set;
		}

		void InvalidateGeometry(InvalidateGeometryReasons reasons);

		event EventHandler RenderedGeometryChanged;
	}
}