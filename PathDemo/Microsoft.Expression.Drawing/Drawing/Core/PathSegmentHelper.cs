using Microsoft.Expression.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Controls;

namespace Microsoft.Expression.Drawing.Core
{
	internal static class PathSegmentHelper
	{
		public static PathSegment ApplyTransform(PathSegment segment, Point start, GeneralTransform transform)
		{
			return PathSegmentHelper.PathSegmentImplementation.Create(segment, start).ApplyTransform(transform);
		}

		public static PathSegment ArcToBezierSegments(ArcSegment arcSegment, Point startPoint)
		{
			Point[] pointArray;
			int num;
			bool flag = arcSegment.IsStroked();
			double x = startPoint.X;
			double y = startPoint.Y;
			double width = arcSegment.Size.Width;
			double height = arcSegment.Size.Height;
			double rotationAngle = arcSegment.RotationAngle;
			bool isLargeArc = arcSegment.IsLargeArc;
			double x1 = arcSegment.Point.X;
			Point point = arcSegment.Point;
			PathSegmentHelper.ArcToBezierHelper.ArcToBezier(x, y, width, height, rotationAngle, isLargeArc, arcSegment.SweepDirection == SweepDirection.Clockwise, x1, point.Y, out pointArray, out num);
			if (num == -1)
			{
				return null;
			}
			if (num == 0)
			{
				return PathSegmentHelper.CreateLineSegment(arcSegment.Point, flag);
			}
			if (num != 1)
			{
				return PathSegmentHelper.CreatePolyBezierSegment(pointArray, 0, num * 3, flag);
			}
			return PathSegmentHelper.CreateBezierSegment(pointArray[0], pointArray[1], pointArray[2], flag);
		}

		public static ArcSegment CreateArcSegment(Point point, Size size, bool isLargeArc, bool clockwise, double rotationAngle = 0, bool isStroked = true)
		{
			ArcSegment arcSegment = new ArcSegment();
			arcSegment.SetIfDifferent(ArcSegment.PointProperty, point);
			arcSegment.SetIfDifferent(ArcSegment.SizeProperty, size);
			arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, isLargeArc);
			arcSegment.SetIfDifferent(ArcSegment.SweepDirectionProperty, (clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise));
			arcSegment.SetIfDifferent(ArcSegment.RotationAngleProperty, rotationAngle);
			arcSegment.SetIsStroked(isStroked);
			return arcSegment;
		}

		public static BezierSegment CreateBezierSegment(Point point1, Point point2, Point point3, bool isStroked = true)
		{
			BezierSegment bezierSegment = new BezierSegment()
			{
				Point1 = point1,
				Point2 = point2,
				Point3 = point3
			};
			bezierSegment.SetIsStroked(isStroked);
			return bezierSegment;
		}

		public static LineSegment CreateLineSegment(Point point, bool isStroked = true)
		{
			LineSegment lineSegment = new LineSegment()
			{
				Point = point
			};
			lineSegment.SetIsStroked(isStroked);
			return lineSegment;
		}

		public static PolyBezierSegment CreatePolyBezierSegment(IList<Point> points, int start, int count, bool isStroked = true)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			count = count / 3 * 3;
			if (count < 0 || points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			PolyBezierSegment polyBezierSegment = new PolyBezierSegment()
			{
				Points = new PointCollection()
			};
			for (int i = 0; i < count; i++)
			{
				polyBezierSegment.Points.Add(points[start + i]);
			}
			polyBezierSegment.SetIsStroked(isStroked);
			return polyBezierSegment;
		}

		public static PolyLineSegment CreatePolylineSegment(IList<Point> points, int start, int count, bool isStroked = true)
		{
			if (count < 0 || points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			PolyLineSegment polyLineSegment = new PolyLineSegment()
			{
				Points = new PointCollection()
			};
			for (int i = 0; i < count; i++)
			{
				polyLineSegment.Points.Add(points[start + i]);
			}
			polyLineSegment.SetIsStroked(isStroked);
			return polyLineSegment;
		}

		public static PolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(IList<Point> points, int start, int count, bool isStroked = true)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			count = count / 2 * 2;
			if (count < 0 || points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			PolyQuadraticBezierSegment polyQuadraticBezierSegment = new PolyQuadraticBezierSegment()
			{
				Points = new PointCollection()
			};
			for (int i = 0; i < count; i++)
			{
				polyQuadraticBezierSegment.Points.Add(points[start + i]);
			}
			polyQuadraticBezierSegment.SetIsStroked(isStroked);
			return polyQuadraticBezierSegment;
		}

		public static QuadraticBezierSegment CreateQuadraticBezierSegment(Point point1, Point point2, bool isStroked = true)
		{
			QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment()
			{
				Point1 = point1,
				Point2 = point2
			};
			quadraticBezierSegment.SetIsStroked(isStroked);
			return quadraticBezierSegment;
		}

		public static void FlattenSegment(this PathSegment segment, IList<Point> points, Point start, double tolerance)
		{
			PathSegmentHelper.PathSegmentImplementation.Create(segment, start).Flatten(points, tolerance);
		}

		public static Point GetLastPoint(this PathSegment segment)
		{
			return segment.GetPoint(-1);
		}

		public static Point GetPoint(this PathSegment segment, int index)
		{
			return PathSegmentHelper.PathSegmentImplementation.Create(segment).GetPoint(index);
		}

		public static int GetPointCount(this PathSegment segment)
		{
			if (segment is ArcSegment)
			{
				return 1;
			}
			if (segment is LineSegment)
			{
				return 1;
			}
			if (segment is QuadraticBezierSegment)
			{
				return 2;
			}
			if (segment is BezierSegment)
			{
				return 3;
			}
			PolyLineSegment polyLineSegment = segment as PolyLineSegment;
			PolyLineSegment polyLineSegment1 = polyLineSegment;
			if (polyLineSegment != null)
			{
				return polyLineSegment1.Points.Count;
			}
			PolyQuadraticBezierSegment polyQuadraticBezierSegment = segment as PolyQuadraticBezierSegment;
			PolyQuadraticBezierSegment polyQuadraticBezierSegment1 = polyQuadraticBezierSegment;
			if (polyQuadraticBezierSegment != null)
			{
				return polyQuadraticBezierSegment1.Points.Count / 2 * 2;
			}
			PolyBezierSegment polyBezierSegment = segment as PolyBezierSegment;
			PolyBezierSegment polyBezierSegment1 = polyBezierSegment;
			if (polyBezierSegment == null)
			{
				return 0;
			}
			return polyBezierSegment1.Points.Count / 3 * 3;
		}

		public static IEnumerable<SimpleSegment> GetSimpleSegments(this PathSegment segment, Point start)
		{
			return PathSegmentHelper.PathSegmentImplementation.Create(segment, start).GetSimpleSegments();
		}

		public static bool IsEmpty(this PathSegment segment)
		{
			return segment.GetPointCount() == 0;
		}

		private static void SetIsStroked(this PathSegment segment, bool isStroked)
		{
		}

		public static bool SyncPolyBezierSegment(PathSegmentCollection collection, int index, IList<Point> points, int start, int count)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (index < 0 || index >= collection.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			bool flag = false;
			count = count / 3 * 3;
			PolyBezierSegment item = collection[index] as PolyBezierSegment;
			PolyBezierSegment polyBezierSegment = item;
			if (item == null)
			{
				PolyBezierSegment polyBezierSegment1 = new PolyBezierSegment();
				polyBezierSegment = polyBezierSegment1;
				collection[index] = polyBezierSegment1;
				flag = true;
			}
			polyBezierSegment.Points.EnsureListCount<Point>(count, null);
			for (int i = 0; i < count; i++)
			{
				if (polyBezierSegment.Points[i] != points[i + start])
				{
					polyBezierSegment.Points[i] = points[i + start];
					flag = true;
				}
			}
			return flag;
		}

		public static bool SyncPolylineSegment(PathSegmentCollection collection, int index, IList<Point> points, int start, int count)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (index < 0 || index >= collection.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (points.Count < start + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			bool flag = false;
			PolyLineSegment item = collection[index] as PolyLineSegment;
			PolyLineSegment polyLineSegment = item;
			if (item == null)
			{
				PolyLineSegment polyLineSegment1 = new PolyLineSegment();
				polyLineSegment = polyLineSegment1;
				collection[index] = polyLineSegment1;
				flag = true;
			}
			flag = flag | polyLineSegment.Points.EnsureListCount<Point>(count, null);
			for (int i = 0; i < count; i++)
			{
				if (polyLineSegment.Points[i] != points[i + start])
				{
					polyLineSegment.Points[i] = points[i + start];
					flag = true;
				}
			}
			return flag;
		}

		private class ArcSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			private ArcSegment segment;

			public ArcSegmentImplementation()
			{
			}

			public override PathSegment ApplyTransform(GeneralTransform transform)
			{
				PathSegment bezierSegments = PathSegmentHelper.ArcToBezierSegments(this.segment, base.Start);
				if (bezierSegments != null)
				{
					return PathSegmentHelper.ApplyTransform(bezierSegments, base.Start, transform);
				}
				this.segment.Point = transform.Transform(this.segment.Point);
				return this.segment;
			}

			public static PathSegmentHelper.PathSegmentImplementation Create(ArcSegment source)
			{
				if (source == null)
				{
					return null;
				}
				return new PathSegmentHelper.ArcSegmentImplementation()
				{
					segment = source
				};
			}

			public override void Flatten(IList<Point> points, double tolerance)
			{
				PathSegment bezierSegments = PathSegmentHelper.ArcToBezierSegments(this.segment, base.Start);
				if (bezierSegments != null)
				{
					bezierSegments.FlattenSegment(points, base.Start, tolerance);
				}
			}

			public override Point GetPoint(int index)
			{
				if (index < -1 || index > 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.segment.Point;
			}

			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				PathSegment bezierSegments = PathSegmentHelper.ArcToBezierSegments(this.segment, base.Start);
				if (bezierSegments == null)
				{
					return Enumerable.Empty<SimpleSegment>();
				}
				return bezierSegments.GetSimpleSegments(base.Start);
			}
		}

		private static class ArcToBezierHelper
		{
			private static bool AcceptRadius(double rHalfChord2, double rFuzz2, ref double rRadius)
			{
				bool flag = rRadius * rRadius > rHalfChord2 * rFuzz2;
				if (flag && rRadius < 0)
				{
					rRadius = -rRadius;
				}
				return flag;
			}

			public static void ArcToBezier(double xStart, double yStart, double xRadius, double yRadius, double rRotation, bool fLargeArc, bool fSweepUp, double xEnd, double yEnd, out Point[] pPt, out int cPieces)
			{
				double num;
				double num1;
				double num2;
				double num3;
				double num4;
				double num5;
				Point point;
				double num6 = 1E-06;
				pPt = new Point[12];
				double num7 = num6 * num6;
				bool flag = false;
				cPieces = -1;
				double num8 = 0.5 * (xEnd - xStart);
				double num9 = 0.5 * (yEnd - yStart);
				double num10 = num8 * num8 + num9 * num9;
				if (num10 < num7)
				{
					return;
				}
				if (!PathSegmentHelper.ArcToBezierHelper.AcceptRadius(num10, num7, ref xRadius) || !PathSegmentHelper.ArcToBezierHelper.AcceptRadius(num10, num7, ref yRadius))
				{
					cPieces = 0;
					return;
				}
				if (Math.Abs(rRotation) >= num6)
				{
					rRotation = -rRotation * 3.14159265358979 / 180;
					num = Math.Cos(rRotation);
					num1 = Math.Sin(rRotation);
					double num11 = num8 * num - num9 * num1;
					num9 = num8 * num1 + num9 * num;
					num8 = num11;
				}
				else
				{
					num = 1;
					num1 = 0;
				}
				num8 = num8 / xRadius;
				num9 = num9 / yRadius;
				num10 = num8 * num8 + num9 * num9;
				if (num10 <= 1)
				{
					double num12 = Math.Sqrt((1 - num10) / num10);
					if (fLargeArc == fSweepUp)
					{
						num4 = num12 * num9;
						num5 = -num12 * num8;
					}
					else
					{
						num4 = -num12 * num9;
						num5 = num12 * num8;
					}
				}
				else
				{
					double num13 = Math.Sqrt(num10);
					xRadius = xRadius * num13;
					yRadius = yRadius * num13;
					double num14 = 0;
					num5 = num14;
					num4 = num14;
					flag = true;
					num8 = num8 / num13;
					num9 = num9 / num13;
				}
				Point point1 = new Point(-num8 - num4, -num9 - num5);
				Point point2 = new Point(num8 - num4, num9 - num5);
				Matrix matrix = new Matrix(num * xRadius, -num1 * xRadius, num1 * yRadius, num * yRadius, 0.5 * (xEnd + xStart), 0.5 * (yEnd + yStart));
				if (!flag)
				{
					matrix.OffsetX = matrix.OffsetX + (matrix.M11 * num4 + matrix.M21 * num5);
					matrix.OffsetY = matrix.OffsetY + (matrix.M12 * num4 + matrix.M22 * num5);
				}
				PathSegmentHelper.ArcToBezierHelper.GetArcAngle(point1, point2, fLargeArc, fSweepUp, out num2, out num3, out cPieces);
				double bezierDistance = PathSegmentHelper.ArcToBezierHelper.GetBezierDistance(num2, 1);
				if (!fSweepUp)
				{
					bezierDistance = -bezierDistance;
				}
				Point point3 = new Point(-bezierDistance * point1.Y, bezierDistance * point1.X);
				int num15 = 0;
				pPt = new Point[cPieces * 3];
				for (int i = 1; i < cPieces; i++)
				{
					Point point4 = new Point(point1.X * num2 - point1.Y * num3, point1.X * num3 + point1.Y * num2);
					point = new Point(-bezierDistance * point4.Y, bezierDistance * point4.X);
					int num16 = num15;
					num15 = num16 + 1;
					pPt[num16] = matrix.Transform(point1.Plus(point3));
					int num17 = num15;
					num15 = num17 + 1;
					pPt[num17] = matrix.Transform(point4.Minus(point));
					int num18 = num15;
					num15 = num18 + 1;
					pPt[num18] = matrix.Transform(point4);
					point1 = point4;
					point3 = point;
				}
				point = new Point(-bezierDistance * point2.Y, bezierDistance * point2.X);
				int num19 = num15;
				num15 = num19 + 1;
				pPt[num19] = matrix.Transform(point1.Plus(point3));
				int num20 = num15;
				num15 = num20 + 1;
				pPt[num20] = matrix.Transform(point2.Minus(point));
				pPt[num15] = new Point(xEnd, yEnd);
			}

			private static void GetArcAngle(Point ptStart, Point ptEnd, bool fLargeArc, bool fSweepUp, out double rCosArcAngle, out double rSinArcAngle, out int cPieces)
			{
				rCosArcAngle = GeometryHelper.Dot(ptStart, ptEnd);
				rSinArcAngle = GeometryHelper.Determinant(ptStart, ptEnd);
				if (rCosArcAngle >= 0)
				{
					if (!fLargeArc)
					{
						cPieces = 1;
						return;
					}
					cPieces = 4;
				}
				else if (!fLargeArc)
				{
					cPieces = 2;
				}
				else
				{
					cPieces = 3;
				}
				double num = Math.Atan2(rSinArcAngle, rCosArcAngle);
				if (fSweepUp)
				{
					if (num < 0)
					{
						num = num + 6.28318530717959;
					}
				}
				else if (num > 0)
				{
					num = num - 6.28318530717959;
				}
				num = num / (double)cPieces;
				rCosArcAngle = Math.Cos(num);
				rSinArcAngle = Math.Sin(num);
			}

			private static double GetBezierDistance(double rDot, double rRadius = 1)
			{
				double num = rRadius * rRadius;
				double num1 = 0;
				double num2 = 0.5 * (num + rDot);
				if (num2 >= 0)
				{
					double num3 = num - num2;
					if (num3 > 0)
					{
						double num4 = Math.Sqrt(num3);
						double num5 = 4 * (rRadius - Math.Sqrt(num2)) / 3;
						num1 = (num5 > num4 * 1E-06 ? num5 / num4 : 0);
					}
				}
				return num1;
			}
		}

		private class BezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			private BezierSegment segment;

			public BezierSegmentImplementation()
			{
			}

			public override PathSegment ApplyTransform(GeneralTransform transform)
			{
				this.segment.Point1 = transform.Transform(this.segment.Point1);
				this.segment.Point2 = transform.Transform(this.segment.Point2);
				this.segment.Point3 = transform.Transform(this.segment.Point3);
				return this.segment;
			}

			public static PathSegmentHelper.PathSegmentImplementation Create(BezierSegment source)
			{
				if (source == null)
				{
					return null;
				}
				return new PathSegmentHelper.BezierSegmentImplementation()
				{
					segment = source
				};
			}

			public override void Flatten(IList<Point> points, double tolerance)
			{
				Point[] start = new Point[] { base.Start, this.segment.Point1, this.segment.Point2, this.segment.Point3 };
				Point[] pointArray = start;
				List<Point> points1 = new List<Point>();
				BezierCurveFlattener.FlattenCubic(pointArray, tolerance, points1, true, null);
				points.AddRange<Point>(points1);
			}

			public override Point GetPoint(int index)
			{
				if (index < -1 || index > 2)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index == 0)
				{
					return this.segment.Point1;
				}
				if (index == 1)
				{
					return this.segment.Point2;
				}
				return this.segment.Point3;
			}

			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				yield return SimpleSegment.Create(base.Start, this.segment.Point1, this.segment.Point2, this.segment.Point3);
			}
		}

		private class LineSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			private LineSegment segment;

			public LineSegmentImplementation()
			{
			}

			public override PathSegment ApplyTransform(GeneralTransform transform)
			{
				this.segment.Point = transform.Transform(this.segment.Point);
				return this.segment;
			}

			public static PathSegmentHelper.PathSegmentImplementation Create(LineSegment source)
			{
				if (source == null)
				{
					return null;
				}
				return new PathSegmentHelper.LineSegmentImplementation()
				{
					segment = source
				};
			}

			public override void Flatten(IList<Point> points, double tolerance)
			{
				points.Add(this.segment.Point);
			}

			public override Point GetPoint(int index)
			{
				if (index < -1 || index > 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.segment.Point;
			}

			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				yield return SimpleSegment.Create(base.Start, this.segment.Point);
			}
		}

		private abstract class PathSegmentImplementation
		{
			public Point Start
			{
				get;
				private set;
			}

			protected PathSegmentImplementation()
			{
			}

			public abstract PathSegment ApplyTransform(GeneralTransform transform);

			public static PathSegmentHelper.PathSegmentImplementation Create(PathSegment segment, Point start)
			{
				PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation = PathSegmentHelper.PathSegmentImplementation.Create(segment);
				pathSegmentImplementation.Start = start;
				return pathSegmentImplementation;
			}

			public static PathSegmentHelper.PathSegmentImplementation Create(PathSegment segment)
			{
				PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation = PathSegmentHelper.BezierSegmentImplementation.Create(segment as BezierSegment);
				PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation1 = pathSegmentImplementation;
				if (pathSegmentImplementation == null)
				{
					PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation2 = PathSegmentHelper.LineSegmentImplementation.Create(segment as LineSegment);
					pathSegmentImplementation1 = pathSegmentImplementation2;
					if (pathSegmentImplementation2 == null)
					{
						PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation3 = PathSegmentHelper.ArcSegmentImplementation.Create(segment as ArcSegment);
						pathSegmentImplementation1 = pathSegmentImplementation3;
						if (pathSegmentImplementation3 == null)
						{
							PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation4 = PathSegmentHelper.PolyLineSegmentImplementation.Create(segment as PolyLineSegment);
							pathSegmentImplementation1 = pathSegmentImplementation4;
							if (pathSegmentImplementation4 == null)
							{
								PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation5 = PathSegmentHelper.PolyBezierSegmentImplementation.Create(segment as PolyBezierSegment);
								pathSegmentImplementation1 = pathSegmentImplementation5;
								if (pathSegmentImplementation5 == null)
								{
									PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation6 = PathSegmentHelper.QuadraticBezierSegmentImplementation.Create(segment as QuadraticBezierSegment);
									pathSegmentImplementation1 = pathSegmentImplementation6;
									if (pathSegmentImplementation6 == null)
									{
										PathSegmentHelper.PathSegmentImplementation pathSegmentImplementation7 = PathSegmentHelper.PolyQuadraticBezierSegmentImplementation.Create(segment as PolyQuadraticBezierSegment);
										pathSegmentImplementation1 = pathSegmentImplementation7;
										if (pathSegmentImplementation7 == null)
										{
											CultureInfo currentCulture = CultureInfo.CurrentCulture;
											string typeNotSupported = ExceptionStringTable.TypeNotSupported;
											object[] fullName = new object[] { segment.GetType().FullName };
											throw new NotSupportedException(string.Format(currentCulture, typeNotSupported, fullName));
										}
									}
								}
							}
						}
					}
				}
				return pathSegmentImplementation1;
			}

			public abstract void Flatten(IList<Point> points, double tolerance);

			public abstract Point GetPoint(int index);

			public abstract IEnumerable<SimpleSegment> GetSimpleSegments();
		}

		private class PolyBezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			private PolyBezierSegment segment;

			public PolyBezierSegmentImplementation()
			{
			}

			public override PathSegment ApplyTransform(GeneralTransform transform)
			{
				this.segment.Points.ApplyTransform(transform);
				return this.segment;
			}

			public static PathSegmentHelper.PathSegmentImplementation Create(PolyBezierSegment source)
			{
				if (source == null)
				{
					return null;
				}
				return new PathSegmentHelper.PolyBezierSegmentImplementation()
				{
					segment = source
				};
			}

			public override void Flatten(IList<Point> points, double tolerance)
			{
				Point start = base.Start;
				int count = this.segment.Points.Count / 3 * 3;
				for (int i = 0; i < count; i = i + 3)
				{
					Point[] item = new Point[] { start, this.segment.Points[i], this.segment.Points[i + 1], this.segment.Points[i + 2] };
					Point[] pointArray = item;
					List<Point> points1 = new List<Point>();
					BezierCurveFlattener.FlattenCubic(pointArray, tolerance, points1, true, null);
					points.AddRange<Point>(points1);
					start = this.segment.Points[i + 2];
				}
			}

			public override Point GetPoint(int index)
			{
				int count = this.segment.Points.Count / 3 * 3;
				if (index < -1 || index > count - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index != -1)
				{
					return this.segment.Points[index];
				}
				return this.segment.Points[count - 1];
			}

			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				Point start = base.Start;
				IList<Point> points = this.segment.Points;
				int num = this.segment.Points.Count / 3;
				for (int i = 0; i < num; i++)
				{
					int num1 = i * 3;
					yield return SimpleSegment.Create(start, points[num1], points[num1 + 1], points[num1 + 2]);
					start = points[num1 + 2];
				}
			}
		}

		private class PolyLineSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			private PolyLineSegment segment;

			public PolyLineSegmentImplementation()
			{
			}

			public override PathSegment ApplyTransform(GeneralTransform transform)
			{
				this.segment.Points.ApplyTransform(transform);
				return this.segment;
			}

			public static PathSegmentHelper.PathSegmentImplementation Create(PolyLineSegment source)
			{
				if (source == null)
				{
					return null;
				}
				return new PathSegmentHelper.PolyLineSegmentImplementation()
				{
					segment = source
				};
			}

			public override void Flatten(IList<Point> points, double tolerance)
			{
				points.AddRange<Point>(this.segment.Points);
			}

			public override Point GetPoint(int index)
			{
				if (index < -1 || index > this.segment.Points.Count - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index == -1)
				{
					return this.segment.Points.Last<Point>();
				}
				return this.segment.Points[index];
			}

			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				Point start = base.Start;
				foreach (Point point in this.segment.Points)
				{
					yield return SimpleSegment.Create(start, point);
					start = point;
				}
			}
		}

		private class PolyQuadraticBezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			private PolyQuadraticBezierSegment segment;

			public PolyQuadraticBezierSegmentImplementation()
			{
			}

			public override PathSegment ApplyTransform(GeneralTransform transform)
			{
				this.segment.Points.ApplyTransform(transform);
				return this.segment;
			}

			public static PathSegmentHelper.PathSegmentImplementation Create(PolyQuadraticBezierSegment source)
			{
				if (source == null)
				{
					return null;
				}
				return new PathSegmentHelper.PolyQuadraticBezierSegmentImplementation()
				{
					segment = source
				};
			}

			public override void Flatten(IList<Point> points, double tolerance)
			{
				Point start = base.Start;
				int count = this.segment.Points.Count / 2 * 2;
				for (int i = 0; i < count; i = i + 2)
				{
					Point[] item = new Point[] { start, this.segment.Points[i], this.segment.Points[i + 1] };
					Point[] pointArray = item;
					List<Point> points1 = new List<Point>();
					BezierCurveFlattener.FlattenQuadratic(pointArray, tolerance, points1, true, null);
					points.AddRange<Point>(points1);
					start = this.segment.Points[i + 1];
				}
			}

			public override Point GetPoint(int index)
			{
				int count = this.segment.Points.Count / 2 * 2;
				if (index < -1 || index > count - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index != -1)
				{
					return this.segment.Points[index];
				}
				return this.segment.Points[count - 1];
			}

			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				Point start = base.Start;
				IList<Point> points = this.segment.Points;
				int num = this.segment.Points.Count / 2;
				for (int i = 0; i < num; i++)
				{
					int num1 = i * 2;
					yield return SimpleSegment.Create(start, points[num1], points[num1 + 1]);
					start = points[num1 + 1];
				}
			}
		}

		private class QuadraticBezierSegmentImplementation : PathSegmentHelper.PathSegmentImplementation
		{
			private QuadraticBezierSegment segment;

			public QuadraticBezierSegmentImplementation()
			{
			}

			public override PathSegment ApplyTransform(GeneralTransform transform)
			{
				this.segment.Point1 = transform.Transform(this.segment.Point1);
				this.segment.Point2 = transform.Transform(this.segment.Point2);
				return this.segment;
			}

			public static PathSegmentHelper.PathSegmentImplementation Create(QuadraticBezierSegment source)
			{
				if (source == null)
				{
					return null;
				}
				return new PathSegmentHelper.QuadraticBezierSegmentImplementation()
				{
					segment = source
				};
			}

			public override void Flatten(IList<Point> points, double tolerance)
			{
				Point[] start = new Point[] { base.Start, this.segment.Point1, this.segment.Point2 };
				Point[] pointArray = start;
				List<Point> points1 = new List<Point>();
				BezierCurveFlattener.FlattenQuadratic(pointArray, tolerance, points1, true, null);
				points.AddRange<Point>(points1);
			}

			public override Point GetPoint(int index)
			{
				if (index < -1 || index > 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index == 0)
				{
					return this.segment.Point1;
				}
				return this.segment.Point2;
			}

			public override IEnumerable<SimpleSegment> GetSimpleSegments()
			{
				yield return SimpleSegment.Create(base.Start, this.segment.Point1, this.segment.Point2);
			}
		}
	}
}