using Microsoft.Expression.Drawing.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	internal class LineArrowGeometrySource : GeometrySource<ILineArrowGeometrySourceParameters>
	{
		public LineArrowGeometrySource()
		{
		}

		private Point GetEndPoint(ILineArrowGeometrySourceParameters parameters)
		{
			Rect logicalBounds = base.LogicalBounds;
			switch (parameters.StartCorner)
			{
				case CornerType.TopLeft:
				{
					return logicalBounds.BottomRight();
				}
				case CornerType.TopRight:
				{
					return logicalBounds.BottomLeft();
				}
				case CornerType.BottomRight:
				{
					return logicalBounds.TopLeft();
				}
				case CornerType.BottomLeft:
				{
					return logicalBounds.TopRight();
				}
			}
			return logicalBounds.BottomRight();
		}

		private Point GetMiddlePoint(ILineArrowGeometrySourceParameters parameters)
		{
			Rect logicalBounds = base.LogicalBounds;
			double bendAmount = (parameters.BendAmount + 1) / 2;
			switch (parameters.StartCorner)
			{
				case CornerType.TopLeft:
				{
					return GeometryHelper.Lerp(logicalBounds.BottomLeft(), logicalBounds.TopRight(), bendAmount);
				}
				case CornerType.TopRight:
				{
					return GeometryHelper.Lerp(logicalBounds.TopLeft(), logicalBounds.BottomRight(), bendAmount);
				}
				case CornerType.BottomRight:
				{
					return GeometryHelper.Lerp(logicalBounds.TopRight(), logicalBounds.BottomLeft(), bendAmount);
				}
				case CornerType.BottomLeft:
				{
					return GeometryHelper.Lerp(logicalBounds.BottomRight(), logicalBounds.TopLeft(), bendAmount);
				}
			}
			return logicalBounds.Center();
		}

		private static Point[] GetPointTrio(Point startPoint, Vector tangent, double size)
		{
			Vector vector = tangent.Perpendicular().Normalized() * 0.57735;
			Point[] pointArray = new Point[] { (startPoint - (tangent * size)) + (vector * size), startPoint, (startPoint - (tangent * size)) - (vector * size) };
			return pointArray;
		}

		private Point GetStartPoint(ILineArrowGeometrySourceParameters parameters)
		{
			Rect logicalBounds = base.LogicalBounds;
			switch (parameters.StartCorner)
			{
				case CornerType.TopLeft:
				{
					return logicalBounds.TopLeft();
				}
				case CornerType.TopRight:
				{
					return logicalBounds.TopRight();
				}
				case CornerType.BottomRight:
				{
					return logicalBounds.BottomRight();
				}
				case CornerType.BottomLeft:
				{
					return logicalBounds.BottomLeft();
				}
			}
			return logicalBounds.BottomRight();
		}

		private static bool UpdateArrow(ArrowType arrowType, double size, PathFigure figure, Point startPoint, Vector tangent)
		{
			bool flag = false;
			switch (arrowType)
			{
				case ArrowType.NoArrow:
				{
					flag = flag | figure.SetIfDifferent(PathFigure.StartPointProperty, startPoint);
					flag = flag | figure.Segments.EnsureListCount<PathSegment>(0, null);
					break;
				}
				case ArrowType.OvalArrow:
				{
					Rect rect = new Rect(startPoint.X - size / 2, startPoint.Y - size / 2, size, size);
					flag = flag | PathFigureHelper.SyncEllipseFigure(figure, rect, SweepDirection.Clockwise, true);
					break;
				}
				default:
				{
					Point[] pointTrio = LineArrowGeometrySource.GetPointTrio(startPoint, tangent, size);
					if (arrowType != ArrowType.StealthArrow)
					{
						bool flag1 = arrowType == ArrowType.OpenArrow;
						flag = flag | PathFigureHelper.SyncPolylineFigure(figure, pointTrio, !flag1, !flag1);
						break;
					}
					else
					{
						List<Point> points = new List<Point>(pointTrio)
						{
							startPoint - (((tangent * size) * 2) / 3)
						};
						flag = flag | PathFigureHelper.SyncPolylineFigure(figure, points, true, true);
						break;
					}
				}
			}
			return flag;
		}

		protected override bool UpdateCachedGeometry(ILineArrowGeometrySourceParameters parameters)
		{
			PathGeometry pathGeometry;
			QuadraticBezierSegment quadraticBezierSegment;
			bool flag = false | GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag = flag | pathGeometry.Figures.EnsureListCount<PathFigure>(3, () => new PathFigure());
			Point startPoint = this.GetStartPoint(parameters);
			Point endPoint = this.GetEndPoint(parameters);
			Point middlePoint = this.GetMiddlePoint(parameters);
			PathFigure item = pathGeometry.Figures[0];
			flag = flag | item.SetIfDifferent(PathFigure.StartPointProperty, startPoint);
			flag = flag | item.SetIfDifferent(PathFigure.IsClosedProperty, false);
			flag = flag | item.SetIfDifferent(PathFigure.IsFilledProperty, false);
			flag = flag | item.Segments.EnsureListCount<PathSegment>(1, () => new QuadraticBezierSegment());
			flag = flag | GeometryHelper.EnsureSegmentType<QuadraticBezierSegment>(out quadraticBezierSegment, item.Segments, 0, () => new QuadraticBezierSegment());
			flag = flag | quadraticBezierSegment.SetIfDifferent(QuadraticBezierSegment.Point1Property, middlePoint);
			flag = flag | quadraticBezierSegment.SetIfDifferent(QuadraticBezierSegment.Point2Property, endPoint);
			flag = flag | LineArrowGeometrySource.UpdateArrow(parameters.StartArrow, parameters.ArrowSize, pathGeometry.Figures[1], startPoint, startPoint.Subtract(middlePoint).Normalized());
			flag = flag | LineArrowGeometrySource.UpdateArrow(parameters.EndArrow, parameters.ArrowSize, pathGeometry.Figures[2], endPoint, endPoint.Subtract(middlePoint).Normalized());
			return true;
		}
	}
}