using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	internal sealed class PathSegmentData
	{
		public System.Windows.Media.PathSegment PathSegment
		{
			get;
			private set;
		}

		public Point StartPoint
		{
			get;
			private set;
		}

		public PathSegmentData(Point startPoint, System.Windows.Media.PathSegment pathSegment)
		{
			this.PathSegment = pathSegment;
			this.StartPoint = startPoint;
		}
	}
}