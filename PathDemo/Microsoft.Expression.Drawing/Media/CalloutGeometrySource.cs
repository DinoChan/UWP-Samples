using Microsoft.Expression.Drawing.Core;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	internal class CalloutGeometrySource : GeometrySource<ICalloutGeometrySourceParameters>
	{
		private const double centerRatioA = 0;

		private const double centerRatioB = 0.5;

		private const double radiusRatioA = 0.05;

		private const double radiusRatioB = 0.2;

		private readonly static Point[] connectionPoints;

		private readonly static Point cloudStartPoint;

		private readonly static Point[] cloudPoints;

		private readonly static Rect cloudBounds;

		static CalloutGeometrySource()
		{
			Point[] point = new Point[] { new Point(0.25, 0), new Point(0.75, 0), new Point(0.25, 1), new Point(0.75, 1), new Point(0, 0.25), new Point(0, 0.75), new Point(1, 0.25), new Point(1, 0.75) };
			CalloutGeometrySource.connectionPoints = point;
			CalloutGeometrySource.cloudStartPoint = new Point(86.42, 23.3);
			Point[] pointArray = new Point[] { new Point(86.42, 23.18), new Point(86.44, 23.07), new Point(86.44, 22.95), new Point(86.44, 16.53), new Point(81.99, 11.32), new Point(76.51, 11.32), new Point(75.12, 11.32), new Point(73.79, 11.66), new Point(72.58, 12.27), new Point(70.81, 5.74), new Point(65.59, 1), new Point(59.43, 1), new Point(54.48, 1), new Point(50.15, 4.06), new Point(47.71, 8.65), new Point(46.62, 7.08), new Point(44.97, 6.08), new Point(43.11, 6.08), new Point(41.21, 6.08), new Point(39.53, 7.13), new Point(38.45, 8.76), new Point(35.72, 5.49), new Point(31.93, 3.46), new Point(27.73, 3.46), new Point(21.26, 3.46), new Point(15.77, 8.27), new Point(13.67, 14.99), new Point(13.36, 14.96), new Point(13.05, 14.94), new Point(12.73, 14.94), new Point(6.25, 14.94), new Point(1, 21.1), new Point(1, 28.69), new Point(1, 35.68), new Point(5.45, 41.44), new Point(11.21, 42.32), new Point(11.65, 47.61), new Point(15.45, 51.74), new Point(20.08, 51.74), new Point(22.49, 51.74), new Point(24.66, 50.63), new Point(26.27, 48.82), new Point(27.38, 53.36), new Point(30.95, 56.69), new Point(35.18, 56.69), new Point(39, 56.69), new Point(42.27, 53.98), new Point(43.7, 50.13), new Point(45.33, 52.69), new Point(47.92, 54.35), new Point(50.86, 54.35), new Point(55, 54.35), new Point(58.48, 51.03), new Point(59.49, 46.53), new Point(61.53, 51.17), new Point(65.65, 54.35), new Point(70.41, 54.35), new Point(77.09, 54.35), new Point(82.51, 48.1), new Point(82.69, 40.32), new Point(83.3, 40.51), new Point(83.95, 40.63), new Point(84.62, 40.63), new Point(88.77, 40.63), new Point(92.13, 36.69), new Point(92.13, 31.83), new Point(92.13, 27.7), new Point(89.69, 24.25), new Point(86.42, 23.3) };
			CalloutGeometrySource.cloudPoints = pointArray;
			CalloutGeometrySource.cloudBounds = new Rect(1, 1, 91.129997253418, 55.689998626709);
		}

		public CalloutGeometrySource()
		{
		}

		private static Point ClosestConnectionPoint(Point relativePoint)
		{
			double num = double.MaxValue;
			Point point = CalloutGeometrySource.connectionPoints[0];
			Point[] pointArray = CalloutGeometrySource.connectionPoints;
			for (int i = 0; i < (int)pointArray.Length; i++)
			{
				Point point1 = pointArray[i];
				double num1 = GeometryHelper.Distance(relativePoint, point1);
				if (num > num1)
				{
					num = num1;
					point = point1;
				}
			}
			return point;
		}

		private Point[] ComputeCorners(double radius)
		{
			double left = base.LogicalBounds.Left;
			double top = base.LogicalBounds.Top;
			double right = base.LogicalBounds.Right;
			double bottom = base.LogicalBounds.Bottom;
			Point[] point = new Point[] { new Point(left, top + radius), new Point(left + radius, top), new Point(right - radius, top), new Point(right, top + radius), new Point(right, bottom - radius), new Point(right - radius, bottom), new Point(left + radius, bottom), new Point(left, bottom - radius) };
			return point;
		}

		private Point GetAbsoluteAnchorPoint(Point relativePoint)
		{
			return GeometryHelper.RelativeToAbsolutePoint(base.LayoutBounds, relativePoint);
		}

		private static bool IsInside(CalloutStyle style, Point point)
		{
			switch (style)
			{
				case CalloutStyle.Oval:
				case CalloutStyle.Cloud:
				{
					return GeometryHelper.Distance(point, new Point(0.5, 0.5)) <= 0.5;
				}
				default:
				{
					if (Math.Abs(point.X - 0.5) > 0.5)
					{
						break;
					}
					else
					{
						return Math.Abs(point.Y - 0.5) <= 0.5;
					}
				}
			}
			return false;
		}

		protected override bool UpdateCachedGeometry(ICalloutGeometrySourceParameters parameters)
		{
			bool flag = false;
			switch (parameters.CalloutStyle)
			{
				case CalloutStyle.Rectangle:
				{
					flag = flag | this.UpdateRectangleCallout(parameters);
					break;
				}
				case CalloutStyle.RoundedRectangle:
				{
					flag = flag | this.UpdateRoundedRectangleCallout(parameters);
					break;
				}
				case CalloutStyle.Cloud:
				{
					flag = flag | this.UpdateCloudCallout(parameters);
					break;
				}
				default:
				{
					flag = flag | this.UpdateOvalCallout(parameters);
					break;
				}
			}
			if (flag)
			{
				this.cachedGeometry = PathGeometryHelper.FixPathGeometryBoundary(this.cachedGeometry);
			}
			return flag;
		}

		private bool UpdateCloudCallout(ICalloutGeometrySourceParameters parameters)
		{
			GeometryGroup geometryGroup;
			PathGeometry pathGeometry;
			EllipseGeometry ellipseGeometry;
			bool flag = false;
			int num = 3;
			if (CalloutGeometrySource.IsInside(parameters.CalloutStyle, parameters.AnchorPoint))
			{
				num = 0;
			}
			flag = flag | GeometryHelper.EnsureGeometryType<GeometryGroup>(out geometryGroup, ref this.cachedGeometry, () => new GeometryGroup());
			flag = flag | geometryGroup.Children.EnsureListCount<System.Windows.Media.Geometry>(num + 1, () => new PathGeometry());
			flag = flag | GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, geometryGroup.Children, 0, () => new PathGeometry());
			flag = flag | pathGeometry.SetIfDifferent(PathGeometry.FillRuleProperty, FillRule.Nonzero);
			flag = flag | pathGeometry.Figures.EnsureListCount<PathFigure>(1 + num, () => new PathFigure());
			Transform transform = GeometryHelper.RelativeTransform(CalloutGeometrySource.cloudBounds, base.LogicalBounds);
			flag = flag | pathGeometry.SetIfDifferent(System.Windows.Media.Geometry.TransformProperty, transform);
			PathFigure item = pathGeometry.Figures[0];
			flag = flag | item.SetIfDifferent(PathFigure.IsFilledProperty, true);
			flag = flag | item.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag = flag | item.SetIfDifferent(PathFigure.StartPointProperty, CalloutGeometrySource.cloudStartPoint);
			flag = flag | item.Segments.EnsureListCount<PathSegment>(1, () => new PolyBezierSegment());
			flag = flag | PathSegmentHelper.SyncPolyBezierSegment(item.Segments, 0, CalloutGeometrySource.cloudPoints, 0, (int)CalloutGeometrySource.cloudPoints.Length);
			for (int i = 0; i < num; i++)
			{
				double num1 = (double)i / (double)num;
				Point point = GeometryHelper.Lerp(this.GetAbsoluteAnchorPoint(parameters.AnchorPoint), base.LogicalBounds.Center(), MathHelper.Lerp(0, 0.5, num1));
				double num2 = MathHelper.Lerp(0.05, 0.2, num1);
				Rect logicalBounds = base.LogicalBounds;
				double num3 = MathHelper.Lerp(0, logicalBounds.Width / 2, num2);
				Rect rect = base.LogicalBounds;
				double num4 = MathHelper.Lerp(0, rect.Height / 2, num2);
				flag = flag | GeometryHelper.EnsureGeometryType<EllipseGeometry>(out ellipseGeometry, geometryGroup.Children, i + 1, () => new EllipseGeometry());
				flag = flag | ellipseGeometry.SetIfDifferent(EllipseGeometry.CenterProperty, point);
				flag = flag | ellipseGeometry.SetIfDifferent(EllipseGeometry.RadiusXProperty, num3);
				flag = flag | ellipseGeometry.SetIfDifferent(EllipseGeometry.RadiusYProperty, num4);
			}
			return flag;
		}

		private static bool UpdateCornerArc(PathSegmentCollection segments, int index, Point start, Point end)
		{
			bool flag = false;
			ArcSegment item = segments[index] as ArcSegment;
			if (item == null)
			{
				ArcSegment arcSegment = new ArcSegment();
				item = arcSegment;
				segments[index] = arcSegment;
				flag = true;
			}
			double num = Math.Abs(end.X - start.X);
			double num1 = Math.Abs(end.Y - start.Y);
			flag = flag | item.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			flag = flag | item.SetIfDifferent(ArcSegment.PointProperty, end);
			flag = flag | item.SetIfDifferent(ArcSegment.SizeProperty, new Size(num, num1));
			flag = flag | item.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise);
			return flag;
		}

		private static bool UpdateEdge(PathSegmentCollection segments, int index, Point start, Point end, Point anchorPoint, double connection, bool connectToAnchor)
		{
			bool flag = false;
			flag = (!connectToAnchor ? flag | CalloutGeometrySource.UpdateLineSegment(segments, index, end) : flag | CalloutGeometrySource.UpdatePolylineSegment(segments, index, start, end, anchorPoint, connection));
			return flag;
		}

		private static bool UpdateLineSegment(PathSegmentCollection segments, int index, Point point)
		{
			bool flag = false;
			LineSegment item = segments[index] as LineSegment;
			if (item == null)
			{
				LineSegment lineSegment = new LineSegment();
				item = lineSegment;
				segments[index] = lineSegment;
				flag = true;
			}
			flag = flag | item.SetIfDifferent(LineSegment.PointProperty, point);
			return flag;
		}

		private bool UpdateOvalCallout(ICalloutGeometrySourceParameters parameters)
		{
			PathFigure pathFigure;
			ArcSegment arcSegment;
			LineSegment lineSegment;
			bool flag = false;
			if (!CalloutGeometrySource.IsInside(parameters.CalloutStyle, parameters.AnchorPoint))
			{
				PathGeometry pathGeometry = this.cachedGeometry as PathGeometry;
				if (pathGeometry != null && pathGeometry.Figures.Count == 1)
				{
					PathFigure item = pathGeometry.Figures[0];
					pathFigure = item;
					if (item.Segments.Count == 2)
					{
						ArcSegment item1 = pathFigure.Segments[0] as ArcSegment;
						arcSegment = item1;
						if (item1 != null)
						{
							LineSegment lineSegment1 = pathFigure.Segments[1] as LineSegment;
							lineSegment = lineSegment1;
							if (lineSegment1 != null)
							{
								goto Label0;
							}
						}
					}
				}
				PathGeometry pathGeometry1 = new PathGeometry();
				pathGeometry = pathGeometry1;
				this.cachedGeometry = pathGeometry1;
				PathFigureCollection figures = pathGeometry.Figures;
				PathFigure pathFigure1 = new PathFigure();
				pathFigure = pathFigure1;
				figures.Add(pathFigure1);
				PathSegmentCollection segments = pathFigure.Segments;
				ArcSegment arcSegment1 = new ArcSegment();
				arcSegment = arcSegment1;
				segments.Add(arcSegment1);
				PathSegmentCollection pathSegmentCollection = pathFigure.Segments;
				LineSegment lineSegment2 = new LineSegment();
				lineSegment = lineSegment2;
				pathSegmentCollection.Add(lineSegment2);
				pathFigure.IsClosed = true;
				arcSegment.IsLargeArc = true;
				arcSegment.SweepDirection = SweepDirection.Clockwise;
				flag = true;
			Label0:
				double arcAngle = GeometryHelper.GetArcAngle(parameters.AnchorPoint);
				double num = arcAngle + 10;
				double num1 = arcAngle - 10;
				flag = flag | pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(num, base.LogicalBounds));
				flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(num1, base.LogicalBounds));
				flag = flag | arcSegment.SetIfDifferent(ArcSegment.SizeProperty, base.LogicalBounds.Resize(0.5).Size());
				flag = flag | lineSegment.SetIfDifferent(LineSegment.PointProperty, this.GetAbsoluteAnchorPoint(parameters.AnchorPoint));
				this.cachedGeometry = pathGeometry;
			}
			else
			{
				EllipseGeometry ellipseGeometry = this.cachedGeometry as EllipseGeometry;
				if (ellipseGeometry == null)
				{
					EllipseGeometry ellipseGeometry1 = new EllipseGeometry();
					ellipseGeometry = ellipseGeometry1;
					this.cachedGeometry = ellipseGeometry1;
					flag = true;
				}
				flag = flag | ellipseGeometry.SetIfDifferent(EllipseGeometry.CenterProperty, base.LogicalBounds.Center());
				DependencyProperty radiusXProperty = EllipseGeometry.RadiusXProperty;
				Rect logicalBounds = base.LogicalBounds;
				flag = flag | ellipseGeometry.SetIfDifferent(radiusXProperty, logicalBounds.Width / 2);
				DependencyProperty radiusYProperty = EllipseGeometry.RadiusYProperty;
				Rect rect = base.LogicalBounds;
				flag = flag | ellipseGeometry.SetIfDifferent(radiusYProperty, rect.Height / 2);
			}
			return flag;
		}

		private static bool UpdatePolylineSegment(PathSegmentCollection segments, int index, Point start, Point end, Point anchor, double connection)
		{
			bool flag = false;
			Point[] pointArray = new Point[] { GeometryHelper.Lerp(start, end, connection - 0.1), anchor, GeometryHelper.Lerp(start, end, connection + 0.1), end };
			Point[] pointArray1 = pointArray;
			flag = flag | PathSegmentHelper.SyncPolylineSegment(segments, index, pointArray1, 0, (int)pointArray1.Length);
			return flag;
		}

		private bool UpdateRectangleCallout(ICalloutGeometrySourceParameters parameters)
		{
			PathFigure pathFigure;
			PathSegmentCollection pathSegmentCollection;
			bool flag = false;
			PathGeometry pathGeometry = this.cachedGeometry as PathGeometry;
			if (pathGeometry != null && pathGeometry.Figures.Count == 1)
			{
				PathFigure item = pathGeometry.Figures[0];
				pathFigure = item;
				if (item != null)
				{
					PathSegmentCollection segments = pathFigure.Segments;
					pathSegmentCollection = segments;
					if (segments.Count == 4)
					{
						goto Label0;
					}
				}
			}
			PathGeometry pathGeometry1 = new PathGeometry();
			pathGeometry = pathGeometry1;
			this.cachedGeometry = pathGeometry1;
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection()
			{
				new LineSegment(),
				new LineSegment(),
				new LineSegment(),
				new LineSegment()
			};
			pathSegmentCollection = pathSegmentCollection1;
			pathFigure = new PathFigure()
			{
				Segments = pathSegmentCollection
			};
			pathGeometry.Figures.Add(pathFigure);
			flag = true;
		Label0:
			Point anchorPoint = parameters.AnchorPoint;
			Point point = CalloutGeometrySource.ClosestConnectionPoint(anchorPoint);
			bool flag1 = CalloutGeometrySource.IsInside(parameters.CalloutStyle, anchorPoint);
			Point absoluteAnchorPoint = this.GetAbsoluteAnchorPoint(anchorPoint);
			flag = flag | pathFigure.SetIfDifferent(PathFigure.StartPointProperty, base.LogicalBounds.TopLeft());
			flag = flag | pathFigure.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag = flag | CalloutGeometrySource.UpdateEdge(pathSegmentCollection, 0, base.LogicalBounds.TopLeft(), base.LogicalBounds.TopRight(), absoluteAnchorPoint, point.X, (flag1 ? false : point.Y == 0));
			flag = flag | CalloutGeometrySource.UpdateEdge(pathSegmentCollection, 1, base.LogicalBounds.TopRight(), base.LogicalBounds.BottomRight(), absoluteAnchorPoint, point.Y, (flag1 ? false : point.X == 1));
			flag = flag | CalloutGeometrySource.UpdateEdge(pathSegmentCollection, 2, base.LogicalBounds.BottomRight(), base.LogicalBounds.BottomLeft(), absoluteAnchorPoint, 1 - point.X, (flag1 ? false : point.Y == 1));
			flag = flag | CalloutGeometrySource.UpdateEdge(pathSegmentCollection, 3, base.LogicalBounds.BottomLeft(), base.LogicalBounds.TopLeft(), absoluteAnchorPoint, 1 - point.Y, (flag1 ? false : point.X == 0));
			return flag;
		}

		private bool UpdateRoundedRectangleCallout(ICalloutGeometrySourceParameters parameters)
		{
			PathFigure pathFigure;
			PathSegmentCollection pathSegmentCollection;
			bool flag = false;
			double width = base.LogicalBounds.Width;
			Rect logicalBounds = base.LogicalBounds;
			double num = Math.Min(width, logicalBounds.Height) / 10;
			Point[] pointArray = this.ComputeCorners(num);
			PathGeometry pathGeometry = this.cachedGeometry as PathGeometry;
			if (pathGeometry != null && pathGeometry.Figures.Count == 1)
			{
				PathFigure item = pathGeometry.Figures[0];
				pathFigure = item;
				if (item != null)
				{
					PathSegmentCollection segments = pathFigure.Segments;
					pathSegmentCollection = segments;
					if (segments.Count == 8)
					{
						goto Label0;
					}
				}
			}
			PathGeometry pathGeometry1 = new PathGeometry();
			pathGeometry = pathGeometry1;
			this.cachedGeometry = pathGeometry1;
			PathSegmentCollection pathSegmentCollection1 = new PathSegmentCollection()
			{
				new ArcSegment(),
				new LineSegment(),
				new ArcSegment(),
				new LineSegment(),
				new ArcSegment(),
				new LineSegment(),
				new ArcSegment(),
				new LineSegment()
			};
			pathSegmentCollection = pathSegmentCollection1;
			pathFigure = new PathFigure()
			{
				Segments = pathSegmentCollection
			};
			pathGeometry.Figures.Add(pathFigure);
			flag = true;
		Label0:
			Point anchorPoint = parameters.AnchorPoint;
			Point point = CalloutGeometrySource.ClosestConnectionPoint(anchorPoint);
			bool flag1 = CalloutGeometrySource.IsInside(parameters.CalloutStyle, anchorPoint);
			Point absoluteAnchorPoint = this.GetAbsoluteAnchorPoint(anchorPoint);
			flag = flag | pathFigure.SetIfDifferent(PathFigure.StartPointProperty, pointArray[0]);
			flag = flag | pathFigure.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag = flag | CalloutGeometrySource.UpdateCornerArc(pathSegmentCollection, 0, pointArray[0], pointArray[1]);
			bool flag2 = flag;
			PathSegmentCollection pathSegmentCollection2 = pathSegmentCollection;
			Point point1 = pointArray[2];
			Point point2 = absoluteAnchorPoint;
			double x = point.X;
			flag = flag2 | CalloutGeometrySource.UpdateEdge(pathSegmentCollection2, 1, pointArray[1], point1, point2, x, (flag1 ? false : point.Y == 0));
			flag = flag | CalloutGeometrySource.UpdateCornerArc(pathSegmentCollection, 2, pointArray[2], pointArray[3]);
			bool flag3 = flag;
			PathSegmentCollection pathSegmentCollection3 = pathSegmentCollection;
			Point point3 = pointArray[4];
			Point point4 = absoluteAnchorPoint;
			double y = point.Y;
			flag = flag3 | CalloutGeometrySource.UpdateEdge(pathSegmentCollection3, 3, pointArray[3], point3, point4, y, (flag1 ? false : point.X == 1));
			flag = flag | CalloutGeometrySource.UpdateCornerArc(pathSegmentCollection, 4, pointArray[4], pointArray[5]);
			bool flag4 = flag;
			PathSegmentCollection pathSegmentCollection4 = pathSegmentCollection;
			Point point5 = pointArray[6];
			Point point6 = absoluteAnchorPoint;
			double x1 = 1 - point.X;
			flag = flag4 | CalloutGeometrySource.UpdateEdge(pathSegmentCollection4, 5, pointArray[5], point5, point6, x1, (flag1 ? false : point.Y == 1));
			flag = flag | CalloutGeometrySource.UpdateCornerArc(pathSegmentCollection, 6, pointArray[6], pointArray[7]);
			bool flag5 = flag;
			PathSegmentCollection pathSegmentCollection5 = pathSegmentCollection;
			Point point7 = pointArray[0];
			Point point8 = absoluteAnchorPoint;
			double y1 = 1 - point.Y;
			flag = flag5 | CalloutGeometrySource.UpdateEdge(pathSegmentCollection5, 7, pointArray[7], point7, point8, y1, (flag1 ? false : point.X == 0));
			return flag;
		}
	}
}