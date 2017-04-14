using System.Windows;

namespace Microsoft.Expression.Media
{
	internal interface ICalloutGeometrySourceParameters : IGeometrySourceParameters
	{
		Point AnchorPoint
		{
			get;
		}

		Microsoft.Expression.Media.CalloutStyle CalloutStyle
		{
			get;
		}
	}
}