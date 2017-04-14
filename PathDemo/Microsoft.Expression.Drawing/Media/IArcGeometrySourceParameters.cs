using System;

namespace Microsoft.Expression.Media
{
	internal interface IArcGeometrySourceParameters : IGeometrySourceParameters
	{
		double ArcThickness
		{
			get;
		}

		UnitType ArcThicknessUnit
		{
			get;
		}

		double EndAngle
		{
			get;
		}

		double StartAngle
		{
			get;
		}
	}
}