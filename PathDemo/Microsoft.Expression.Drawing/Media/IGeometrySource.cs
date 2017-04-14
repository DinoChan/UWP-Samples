using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	public interface IGeometrySource
	{
		System.Windows.Media.Geometry Geometry
		{
			get;
		}

		Rect LayoutBounds
		{
			get;
		}

		Rect LogicalBounds
		{
			get;
		}

		bool InvalidateGeometry(InvalidateGeometryReasons reasons);

		bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds);
	}
}