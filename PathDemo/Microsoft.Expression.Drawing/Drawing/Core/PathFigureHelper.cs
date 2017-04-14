using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	internal static class PathFigureHelper
	{
		public static IEnumerable<PathSegmentData> AllSegments(this PathFigure figure)
		{
			if (figure != null && figure.Segments.Count > 0)
			{
				Point point = figure.StartPoint;
				foreach (PathSegment pathSegment in figure.Segments)
				{
					Point point1 = pathSegment.GetLastPoint();
					yield return new PathSegmentData(point, pathSegment);
					point = point1;
				}
			}
		}

		internal static void ApplyTransform(this PathFigure figure, GeneralTransform transform)
		{
			figure.StartPoint = transform.Transform(figure.StartPoint);
			for (int i = 0; i < figure.Segments.Count; i++)
			{
				PathSegment pathSegment = PathSegmentHelper.ApplyTransform(figure.Segments[i], figure.StartPoint, transform);
				if (figure.Segments[i] != pathSegment)
				{
					figure.Segments[i] = pathSegment;
				}
			}
		}

		internal static void FlattenFigure(PathFigure figure, IList<Point> points, double tolerance, bool removeRepeat)
		{
			IList<Point> points1;
			if (figure == null)
			{
				throw new ArgumentNullException("figure");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (tolerance < 0)
			{
				throw new ArgumentOutOfRangeException("tolerance");
			}
			if (removeRepeat)
			{
				points1 = new List<Point>();
			}
			else
			{
				points1 = points;
			}
			IList<Point> points2 = points1;
			points2.Add(figure.StartPoint);
			foreach (PathSegmentData pathSegmentDatum in figure.AllSegments())
			{
				pathSegmentDatum.PathSegment.FlattenSegment(points2, pathSegmentDatum.StartPoint, tolerance);
			}
			if (figure.IsClosed)
			{
				points2.Add(figure.StartPoint);
			}
			if (removeRepeat && points2.Count > 0)
			{
				points.Add(points2[0]);
				for (int i = 1; i < points2.Count; i++)
				{
					if (!MathHelper.IsVerySmall(GeometryHelper.SquaredDistance(points.Last<Point>(), points2[i])))
					{
						points.Add(points2[i]);
					}
				}
			}
		}

		internal static bool SyncEllipseFigure(PathFigure figure, Rect bounds, SweepDirection sweepDirection, bool isFilled = true)
		{
			ArcSegment arcSegment;
			bool flag = false;
			Point[] point = new Point[2];
			Size size = new Size(bounds.Width / 2, bounds.Height / 2);
			Point point1 = bounds.Center();
			if (size.Width <= size.Height)
			{
				point[0] = new Point(point1.X, bounds.Top);
				point[1] = new Point(point1.X, bounds.Bottom);
			}
			else
			{
				point[0] = new Point(bounds.Left, point1.Y);
				point[1] = new Point(bounds.Right, point1.Y);
			}
			flag = flag | figure.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag = flag | figure.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
			flag = flag | figure.SetIfDifferent(PathFigure.StartPointProperty, point[0]);
			flag = flag | figure.Segments.EnsureListCount<PathSegment>(2, () => new ArcSegment());
			flag = flag | GeometryHelper.EnsureSegmentType<ArcSegment>(out arcSegment, figure.Segments, 0, () => new ArcSegment());
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, point[1]);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SizeProperty, size);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SweepDirectionProperty, sweepDirection);
			flag = flag | GeometryHelper.EnsureSegmentType<ArcSegment>(out arcSegment, figure.Segments, 1, () => new ArcSegment());
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, point[0]);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SizeProperty, size);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SweepDirectionProperty, sweepDirection);
			return flag;
		}

		internal static bool SyncPolylineFigure(PathFigure figure, IList<Point> points, bool isClosed, bool isFilled = true)
		{
			if (figure == null)
			{
				throw new ArgumentNullException("figure");
			}
			bool flag = false;
			if (points == null || points.Count == 0)
			{
				flag = flag | figure.ClearIfSet(PathFigure.StartPointProperty);
				flag = flag | figure.Segments.EnsureListCount<PathSegment>(0, null);
			}
			else
			{
				flag = flag | figure.SetIfDifferent(PathFigure.StartPointProperty, points[0]);
				flag = flag | figure.Segments.EnsureListCount<PathSegment>(1, () => new PolyLineSegment());
				flag = flag | PathSegmentHelper.SyncPolylineSegment(figure.Segments, 0, points, 1, points.Count - 1);
			}
			flag = flag | figure.SetIfDifferent(PathFigure.IsClosedProperty, isClosed);
			flag = flag | figure.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
			return flag;
		}
	}
}