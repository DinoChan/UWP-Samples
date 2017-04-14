using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Microsoft.Expression.Drawing.Core
{
	internal class SimpleSegment
	{
		public Point[] Points
		{
			get;
			private set;
		}

		public SimpleSegment.SegmentType Type
		{
			get;
			private set;
		}

		private SimpleSegment()
		{
		}

		public static SimpleSegment Create(Point point0, Point point1)
		{
			SimpleSegment simpleSegment = new SimpleSegment()
			{
				Type = SimpleSegment.SegmentType.Line,
				Points = new Point[] { point0, point1 }
			};
			return simpleSegment;
		}

		public static SimpleSegment Create(Point point0, Point point1, Point point2)
		{
			Point point = GeometryHelper.Lerp(point0, point1, 0.666666666666667);
			Point point3 = GeometryHelper.Lerp(point1, point2, 0.333333333333333);
			SimpleSegment simpleSegment = new SimpleSegment()
			{
				Type = SimpleSegment.SegmentType.CubicBeizer
			};
			Point[] pointArray = new Point[] { point0, point, point3, point2 };
			simpleSegment.Points = pointArray;
			return simpleSegment;
		}

		public static SimpleSegment Create(Point point0, Point point1, Point point2, Point point3)
		{
			SimpleSegment simpleSegment = new SimpleSegment()
			{
				Type = SimpleSegment.SegmentType.CubicBeizer
			};
			Point[] pointArray = new Point[] { point0, point1, point2, point3 };
			simpleSegment.Points = pointArray;
			return simpleSegment;
		}

		public void Flatten(IList<Point> resultPolyline, double tolerance, IList<double> resultParameters)
		{
			switch (this.Type)
			{
				case SimpleSegment.SegmentType.Line:
				{
					resultPolyline.Add(this.Points[1]);
					if (resultParameters == null)
					{
						break;
					}
					resultParameters.Add(1);
					return;
				}
				case SimpleSegment.SegmentType.CubicBeizer:
				{
					BezierCurveFlattener.FlattenCubic(this.Points, tolerance, resultPolyline, true, resultParameters);
					break;
				}
				default:
				{
					return;
				}
			}
		}

		public enum SegmentType
		{
			Line,
			CubicBeizer
		}
	}
}