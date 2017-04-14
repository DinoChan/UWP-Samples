using System;

namespace Microsoft.Expression.Media
{
	internal interface IBlockArrowGeometrySourceParameters : IGeometrySourceParameters
	{
		double ArrowBodySize
		{
			get;
		}

		double ArrowheadAngle
		{
			get;
		}

		ArrowOrientation Orientation
		{
			get;
		}
	}
}