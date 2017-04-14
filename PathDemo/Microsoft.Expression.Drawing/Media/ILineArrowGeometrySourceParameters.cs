using System;

namespace Microsoft.Expression.Media
{
	internal interface ILineArrowGeometrySourceParameters : IGeometrySourceParameters
	{
		double ArrowSize
		{
			get;
		}

		double BendAmount
		{
			get;
		}

		ArrowType EndArrow
		{
			get;
		}

		ArrowType StartArrow
		{
			get;
		}

		CornerType StartCorner
		{
			get;
		}
	}
}