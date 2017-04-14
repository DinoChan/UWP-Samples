using System;

namespace Microsoft.Expression.Media
{
	internal interface IPolygonGeometrySourceParameters : IGeometrySourceParameters
	{
		double InnerRadius
		{
			get;
		}

		double PointCount
		{
			get;
		}
	}
}