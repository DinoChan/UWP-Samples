using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Microsoft.Expression.Media
{
	internal static class IGeometrySourceExtensions
	{
		public static GeometryEffect GetGeometryEffect(this IGeometrySourceParameters parameters)
		{
			DependencyObject dependencyObject = parameters as DependencyObject;
			if (dependencyObject == null)
			{
				return null;
			}
			GeometryEffect geometryEffect = GeometryEffect.GetGeometryEffect(dependencyObject);
			if (geometryEffect != null && dependencyObject.Equals(geometryEffect.Parent))
			{
				return geometryEffect;
			}
			return null;
		}

		public static double GetHalfStrokeThickness(this IGeometrySourceParameters parameter)
		{
			if (parameter.Stroke != null)
			{
				double strokeThickness = parameter.StrokeThickness;
				if (!double.IsNaN(strokeThickness) && !double.IsInfinity(strokeThickness))
				{
					return Math.Abs(strokeThickness) / 2;
				}
			}
			return 0;
		}
	}
}