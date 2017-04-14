using System;
using System.Windows;

namespace Microsoft.Expression.Media
{
	internal struct PointPair
	{
		public Point Item1;

		public Point Item2;

		public PointPair(Point p1, Point p2)
		{
			this.Item1 = p1;
			this.Item2 = p2;
		}
	}
}