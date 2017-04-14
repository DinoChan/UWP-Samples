using System;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	public interface IGeometrySourceParameters
	{
		System.Windows.Media.Stretch Stretch
		{
			get;
		}

		Brush Stroke
		{
			get;
		}

		double StrokeThickness
		{
			get;
		}
	}
}