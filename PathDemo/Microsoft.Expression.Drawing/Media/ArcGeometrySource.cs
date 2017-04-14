using Microsoft.Expression.Drawing.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	internal class ArcGeometrySource : GeometrySource<IArcGeometrySourceParameters>
	{
		private double relativeThickness;

		private double absoluteThickness;

		public ArcGeometrySource()
		{
		}

		private static bool AreCloseEnough(double angleA, double angleB)
		{
			return Math.Abs(angleA - angleB) < 0.001;
		}

		internal static double[] ComputeAngleRanges(double radiusX, double radiusY, double intersect, double start, double end)
		{
			List<double> nums = new List<double>()
			{
				start,
				end,
				intersect,
				180 - intersect,
				180 + intersect,
				360 - intersect,
				360 + intersect,
				540 - intersect,
				540 + intersect,
				720 - intersect
			};
			List<double> nums1 = nums;
			nums1.Sort();
			int num = nums1.IndexOf(start);
			int num1 = nums1.IndexOf(end);
			if (num1 == num)
			{
				num1++;
			}
			else if (start < end)
			{
				ArcGeometrySource.IncreaseDuplicatedIndex(nums1, ref num);
				ArcGeometrySource.DecreaseDuplicatedIndex(nums1, ref num1);
			}
			else if (start > end)
			{
				ArcGeometrySource.DecreaseDuplicatedIndex(nums1, ref num);
				ArcGeometrySource.IncreaseDuplicatedIndex(nums1, ref num1);
			}
			List<double> nums2 = new List<double>();
			if (num >= num1)
			{
				for (int i = num; i >= num1; i--)
				{
					nums2.Add(nums1[i]);
				}
			}
			else
			{
				for (int j = num; j <= num1; j++)
				{
					nums2.Add(nums1[j]);
				}
			}
			double num2 = ArcGeometrySource.EnsureFirstQuadrant((nums2[0] + nums2[1]) / 2);
			if (radiusX < radiusY && num2 < intersect || radiusX > radiusY && num2 > intersect)
			{
				nums2.RemoveAt(0);
			}
			if (nums2.Count % 2 == 1)
			{
				nums2.RemoveLast<double>();
			}
			if (nums2.Count == 0)
			{
				int num3 = Math.Min(num, num1) - 1;
				if (num3 < 0)
				{
					num3 = Math.Max(num, num1) + 1;
				}
				nums2.Add(nums1[num3]);
				nums2.Add(nums1[num3]);
			}
			return nums2.ToArray();
		}

		protected override Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
		{
			Rect rect = base.ComputeLogicalBounds(layoutBounds, parameters);
			return GeometryHelper.GetStretchBound(rect, parameters.Stretch, new Size(1, 1));
		}

		private static IList<Point> ComputeOneInnerCurve(double start, double end, Rect bounds, double offset)
		{
			double width = bounds.Width / 2;
			double height = bounds.Height / 2;
			Point point = bounds.Center();
			start = start * 3.14159265358979 / 180;
			end = end * 3.14159265358979 / 180;
			double num = 0.174532925199433;
			int num1 = (int)Math.Ceiling(Math.Abs(end - start) / num);
			num1 = Math.Max(2, num1);
			List<Point> points = new List<Point>(num1);
			List<Vector> vectors = new List<Vector>(num1);
			Point x = new Point();
			Point y = new Point();
			Vector vector = new Vector();
			Vector vector1 = new Vector();
			Vector vector2 = new Vector();
			Vector x1 = new Vector();
			for (int i = 0; i < num1; i++)
			{
				double num2 = MathHelper.Lerp(start, end, (double)i / (double)(num1 - 1));
				double num3 = Math.Sin(num2);
				double num4 = Math.Cos(num2);
				x.X = point.X + width * num3;
				x.Y = point.Y - height * num4;
				vector.X = width * num4;
				vector.Y = height * num3;
				vector1.X = -height * num3;
				vector1.Y = width * num4;
				double num5 = height * height * num3 * num3 + width * width * num4 * num4;
				double num6 = Math.Sqrt(num5);
				double num7 = 2 * num3 * num4 * (height * height - width * width);
				vector2.X = -height * num4;
				vector2.Y = -width * num3;
				y.X = x.X + offset * vector1.X / num6;
				y.Y = x.Y + offset * vector1.Y / num6;
				x1.X = vector.X + offset / num6 * (vector2.X - 0.5 * vector1.X / num5 * num7);
				x1.Y = vector.Y + offset / num6 * (vector2.Y - 0.5 * vector1.Y / num5 * num7);
				points.Add(y);
				vectors.Add(-x1.Normalized());
			}
			List<Point> points1 = new List<Point>(num1 * 3 + 1)
			{
				points[0]
			};
			for (int j = 1; j < num1; j++)
			{
				x = points[j - 1];
				y = points[j];
				double num8 = GeometryHelper.Distance(x, y) / 3;
				points1.Add(x + (vectors[j - 1] * num8));
				points1.Add(y - (vectors[j] * num8));
				points1.Add(y);
			}
			return points1;
		}

		private static void DecreaseDuplicatedIndex(IList<double> values, ref int index)
		{
			while (index > 0 && values[index] == values[index - 1])
			{
				index = index - 1;
			}
		}

		internal static double EnsureFirstQuadrant(double angle)
		{
			angle = Math.Abs(angle % 180);
			if (angle <= 90)
			{
				return angle;
			}
			return 180 - angle;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Size GetArcSize(Rect bound)
		{
			return new Size(bound.Width / 2, bound.Height / 2);
		}

		private static void IncreaseDuplicatedIndex(IList<double> values, ref int index)
		{
			while (index < values.Count - 1 && values[index] == values[index + 1])
			{
				index = index + 1;
			}
		}

		internal static double InnerCurveSelfIntersect(double radiusX, double radiusY, double thickness)
		{
			double num = 0;
			double num1 = 1.5707963267949;
			bool flag = radiusX <= radiusY;
			Vector vector = new Vector();
			while (!ArcGeometrySource.AreCloseEnough(num, num1))
			{
				double num2 = (num + num1) / 2;
				double num3 = Math.Cos(num2);
				double num4 = Math.Sin(num2);
				vector.X = radiusY * num4;
				vector.Y = radiusX * num3;
				vector.Normalize();
				if (!flag)
				{
					double num5 = radiusY * num3 - vector.Y * thickness;
					if (num5 >= 0)
					{
						if (num5 <= 0)
						{
							continue;
						}
						num = num2;
					}
					else
					{
						num1 = num2;
					}
				}
				else
				{
					double num6 = radiusX * num4 - vector.X * thickness;
					if (num6 <= 0)
					{
						if (num6 >= 0)
						{
							continue;
						}
						num = num2;
					}
					else
					{
						num1 = num2;
					}
				}
			}
			double num7 = (num + num1) / 2;
			if (ArcGeometrySource.AreCloseEnough(num7, 0))
			{
				return 0;
			}
			if (ArcGeometrySource.AreCloseEnough(num7, 1.5707963267949))
			{
				return 90;
			}
			return num7 * 180 / 3.14159265358979;
		}

		private static double NormalizeAngle(double degree)
		{
			if (degree < 0 || degree > 360)
			{
				degree = degree % 360;
				if (degree < 0)
				{
					degree = degree + 360;
				}
			}
			return degree;
		}

		private void NormalizeThickness(IArcGeometrySourceParameters parameters)
		{
			double width = base.LogicalBounds.Width / 2;
			double height = base.LogicalBounds.Height / 2;
			double num = Math.Min(width, height);
			double arcThickness = parameters.ArcThickness;
			if (parameters.ArcThicknessUnit == UnitType.Pixel)
			{
				arcThickness = MathHelper.SafeDivide(arcThickness, num, 0);
			}
			this.relativeThickness = MathHelper.EnsureRange(arcThickness, new double?(0), new double?(1));
			this.absoluteThickness = num * this.relativeThickness;
		}

		private bool SyncPieceWiseInnerCurves(PathFigure figure, int index, ref Point firstPoint, params double[] angles)
		{
			bool flag = false;
			int length = (int)angles.Length;
			Rect logicalBounds = base.LogicalBounds;
			double num = this.absoluteThickness;
			flag = flag | figure.Segments.EnsureListCount<PathSegment>(index + length / 2, () => new PolyBezierSegment());
			for (int i = 0; i < length / 2; i++)
			{
				IList<Point> points = ArcGeometrySource.ComputeOneInnerCurve(angles[i * 2], angles[i * 2 + 1], logicalBounds, num);
				if (i == 0)
				{
					firstPoint = points[0];
				}
				flag = flag | PathSegmentHelper.SyncPolyBezierSegment(figure.Segments, index + i, points, 1, points.Count - 1);
			}
			return flag;
		}

		protected override bool UpdateCachedGeometry(IArcGeometrySourceParameters parameters)
		{
			bool flag = false;
			this.NormalizeThickness(parameters);
			bool arcThicknessUnit = parameters.ArcThicknessUnit == UnitType.Percent;
			bool flag1 = MathHelper.AreClose(parameters.StartAngle, parameters.EndAngle);
			double num = ArcGeometrySource.NormalizeAngle(parameters.StartAngle);
			double num1 = ArcGeometrySource.NormalizeAngle(parameters.EndAngle);
			if (num1 < num)
			{
				num1 = num1 + 360;
			}
			bool flag2 = this.relativeThickness == 1;
			bool flag3 = this.relativeThickness == 0;
			if (flag1)
			{
				flag = flag | this.UpdateZeroAngleGeometry(arcThicknessUnit, num);
			}
			else if (MathHelper.IsVerySmall((num1 - num) % 360))
			{
				flag = (flag3 || flag2 ? flag | this.UpdateEllipseGeometry(flag2) : flag | this.UpdateFullRingGeometry(arcThicknessUnit));
			}
			else if (!flag2)
			{
				flag = (!flag3 ? flag | this.UpdateRingArcGeometry(arcThicknessUnit, num, num1) : flag | this.UpdateOpenArcGeometry(num, num1));
			}
			else
			{
				flag = flag | this.UpdatePieGeometry(num, num1);
			}
			return flag;
		}

		private bool UpdateEllipseGeometry(bool isFilled)
		{
			PathGeometry pathGeometry;
			ArcSegment arcSegment;
			ArcSegment arcSegment1;
			bool flag = false;
			double top = base.LogicalBounds.Top;
			Rect logicalBounds = base.LogicalBounds;
			double num = MathHelper.Lerp(top, logicalBounds.Bottom, 0.5);
			Point point = new Point(base.LogicalBounds.Left, num);
			Point point1 = new Point(base.LogicalBounds.Right, num);
			flag = flag | GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag = flag | pathGeometry.Figures.EnsureListCount<PathFigure>(1, () => new PathFigure());
			PathFigure item = pathGeometry.Figures[0];
			flag = flag | item.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag = flag | item.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
			flag = flag | item.Segments.EnsureListCount<PathSegment>(2, () => new ArcSegment());
			flag = flag | item.SetIfDifferent(PathFigure.StartPointProperty, point);
			flag = flag | GeometryHelper.EnsureSegmentType<ArcSegment>(out arcSegment, item.Segments, 0, () => new ArcSegment());
			flag = flag | GeometryHelper.EnsureSegmentType<ArcSegment>(out arcSegment1, item.Segments, 1, () => new ArcSegment());
			double width = base.LogicalBounds.Width / 2;
			Rect rect = base.LogicalBounds;
			Size size = new Size(width, rect.Height / 2);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SizeProperty, size);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, point1);
			flag = flag | arcSegment1.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			flag = flag | arcSegment1.SetIfDifferent(ArcSegment.SizeProperty, size);
			flag = flag | arcSegment1.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise);
			flag = flag | arcSegment1.SetIfDifferent(ArcSegment.PointProperty, point);
			return flag;
		}

		private bool UpdateFullRingGeometry(bool relativeMode)
		{
			PathGeometry pathGeometry;
			bool flag = false;
			flag = flag | GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag = flag | pathGeometry.SetIfDifferent(PathGeometry.FillRuleProperty, FillRule.EvenOdd);
			flag = flag | pathGeometry.Figures.EnsureListCount<PathFigure>(2, () => new PathFigure());
			flag = flag | PathFigureHelper.SyncEllipseFigure(pathGeometry.Figures[0], base.LogicalBounds, SweepDirection.Clockwise, true);
			Rect logicalBounds = base.LogicalBounds;
			double width = logicalBounds.Width / 2;
			double height = logicalBounds.Height / 2;
			if (relativeMode || MathHelper.AreClose(width, height))
			{
				Rect rect = base.LogicalBounds.Resize(1 - this.relativeThickness);
				flag = flag | PathFigureHelper.SyncEllipseFigure(pathGeometry.Figures[1], rect, SweepDirection.Counterclockwise, true);
			}
			else
			{
				flag = flag | pathGeometry.Figures[1].SetIfDifferent(PathFigure.IsClosedProperty, true);
				flag = flag | pathGeometry.Figures[1].SetIfDifferent(PathFigure.IsFilledProperty, true);
				Point point = new Point();
				double num = ArcGeometrySource.InnerCurveSelfIntersect(width, height, this.absoluteThickness);
				double[] numArray = ArcGeometrySource.ComputeAngleRanges(width, height, num, 360, 0);
				flag = flag | this.SyncPieceWiseInnerCurves(pathGeometry.Figures[1], 0, ref point, numArray);
				flag = flag | pathGeometry.Figures[1].SetIfDifferent(PathFigure.StartPointProperty, point);
			}
			return flag;
		}

		private bool UpdateOpenArcGeometry(double start, double end)
		{
			PathFigure pathFigure;
			ArcSegment arcSegment;
			bool flag = false;
			PathGeometry pathGeometry = this.cachedGeometry as PathGeometry;
			if (pathGeometry != null && pathGeometry.Figures.Count == 1)
			{
				PathFigure item = pathGeometry.Figures[0];
				pathFigure = item;
				if (item.Segments.Count == 1)
				{
					ArcSegment item1 = pathFigure.Segments[0] as ArcSegment;
					arcSegment = item1;
					if (item1 != null)
					{
						flag = flag | pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, base.LogicalBounds));
						flag = flag | pathFigure.SetIfDifferent(PathFigure.IsFilledProperty, false);
						flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, base.LogicalBounds));
						flag = flag | arcSegment.SetIfDifferent(ArcSegment.SizeProperty, ArcGeometrySource.GetArcSize(base.LogicalBounds));
						flag = flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180);
						return flag;
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
			pathFigure.IsClosed = false;
			arcSegment.SweepDirection = SweepDirection.Clockwise;
			flag = true;
			flag = flag | pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, base.LogicalBounds));
			flag = flag | pathFigure.SetIfDifferent(PathFigure.IsFilledProperty, false);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, base.LogicalBounds));
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SizeProperty, ArcGeometrySource.GetArcSize(base.LogicalBounds));
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180);
			return flag;
		}

		private bool UpdatePieGeometry(double start, double end)
		{
			PathFigure pathFigure;
			ArcSegment arcSegment;
			LineSegment lineSegment;
			bool flag = false;
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
							flag = flag | pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, base.LogicalBounds));
							flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, base.LogicalBounds));
							flag = flag | arcSegment.SetIfDifferent(ArcSegment.SizeProperty, ArcGeometrySource.GetArcSize(base.LogicalBounds));
							flag = flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180);
							flag = flag | lineSegment.SetIfDifferent(LineSegment.PointProperty, base.LogicalBounds.Center());
							return flag;
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
			arcSegment.SweepDirection = SweepDirection.Clockwise;
			flag = true;
			flag = flag | pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, base.LogicalBounds));
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, base.LogicalBounds));
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SizeProperty, ArcGeometrySource.GetArcSize(base.LogicalBounds));
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180);
			flag = flag | lineSegment.SetIfDifferent(LineSegment.PointProperty, base.LogicalBounds.Center());
			return flag;
		}

		private bool UpdateRingArcGeometry(bool relativeMode, double start, double end)
		{
			PathGeometry pathGeometry;
			ArcSegment arcSegment;
			LineSegment lineSegment;
			ArcSegment arcSegment1;
			bool flag = false;
			flag = flag | GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag = flag | pathGeometry.SetIfDifferent(PathGeometry.FillRuleProperty, FillRule.Nonzero);
			flag = flag | pathGeometry.Figures.EnsureListCount<PathFigure>(1, () => new PathFigure());
			PathFigure item = pathGeometry.Figures[0];
			flag = flag | item.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag = flag | item.SetIfDifferent(PathFigure.IsFilledProperty, true);
			flag = flag | item.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(start, base.LogicalBounds));
			flag = flag | item.Segments.EnsureListCountAtLeast<PathSegment>(3, () => new ArcSegment());
			flag = flag | GeometryHelper.EnsureSegmentType<ArcSegment>(out arcSegment, item.Segments, 0, () => new ArcSegment());
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(end, base.LogicalBounds));
			DependencyProperty sizeProperty = ArcSegment.SizeProperty;
			double width = base.LogicalBounds.Width / 2;
			Rect logicalBounds = base.LogicalBounds;
			flag = flag | arcSegment.SetIfDifferent(sizeProperty, new Size(width, logicalBounds.Height / 2));
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180);
			flag = flag | arcSegment.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise);
			flag = flag | GeometryHelper.EnsureSegmentType<LineSegment>(out lineSegment, item.Segments, 1, () => new LineSegment());
			Rect rect = base.LogicalBounds;
			double num = rect.Width / 2;
			double height = rect.Height / 2;
			if (relativeMode || MathHelper.AreClose(num, height))
			{
				Rect rect1 = base.LogicalBounds.Resize(1 - this.relativeThickness);
				flag = flag | lineSegment.SetIfDifferent(LineSegment.PointProperty, GeometryHelper.GetArcPoint(end, rect1));
				flag = flag | item.Segments.EnsureListCount<PathSegment>(3, () => new ArcSegment());
				flag = flag | GeometryHelper.EnsureSegmentType<ArcSegment>(out arcSegment1, item.Segments, 2, () => new ArcSegment());
				flag = flag | arcSegment1.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(start, rect1));
				flag = flag | arcSegment1.SetIfDifferent(ArcSegment.SizeProperty, ArcGeometrySource.GetArcSize(rect1));
				flag = flag | arcSegment1.SetIfDifferent(ArcSegment.IsLargeArcProperty, end - start > 180);
				flag = flag | arcSegment1.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Counterclockwise);
			}
			else
			{
				Point point = new Point();
				double num1 = ArcGeometrySource.InnerCurveSelfIntersect(num, height, this.absoluteThickness);
				double[] numArray = ArcGeometrySource.ComputeAngleRanges(num, height, num1, end, start);
				flag = flag | this.SyncPieceWiseInnerCurves(item, 2, ref point, numArray);
				flag = flag | lineSegment.SetIfDifferent(LineSegment.PointProperty, point);
			}
			return flag;
		}

		private bool UpdateZeroAngleGeometry(bool relativeMode, double angle)
		{
			LineGeometry lineGeometry;
			Point arcPoint;
			bool flag = false;
			Point point = GeometryHelper.GetArcPoint(angle, base.LogicalBounds);
			Rect logicalBounds = base.LogicalBounds;
			double width = logicalBounds.Width / 2;
			double height = logicalBounds.Height / 2;
			if (relativeMode || MathHelper.AreClose(width, height))
			{
				Rect rect = base.LogicalBounds.Resize(1 - this.relativeThickness);
				arcPoint = GeometryHelper.GetArcPoint(angle, rect);
			}
			else
			{
				double num = ArcGeometrySource.InnerCurveSelfIntersect(width, height, this.absoluteThickness);
				double[] numArray = ArcGeometrySource.ComputeAngleRanges(width, height, num, angle, angle);
				double num1 = numArray[0] * 3.14159265358979 / 180;
				Vector vector = new Vector(height * Math.Sin(num1), -width * Math.Cos(num1));
				arcPoint = GeometryHelper.GetArcPoint(numArray[0], base.LogicalBounds) - (vector.Normalized() * this.absoluteThickness);
			}
			flag = flag | GeometryHelper.EnsureGeometryType<LineGeometry>(out lineGeometry, ref this.cachedGeometry, () => new LineGeometry());
			flag = flag | lineGeometry.SetIfDifferent(LineGeometry.StartPointProperty, point);
			flag = flag | lineGeometry.SetIfDifferent(LineGeometry.EndPointProperty, arcPoint);
			return flag;
		}
	}
}