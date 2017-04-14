using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	public static class GeometryHelper
	{
		internal static PathGeometry EmptyGeometry
		{
			get
			{
				return new PathGeometry();
			}
		}

		internal static Point AbsoluteToRelativePoint(Rect bound, Point absolute)
		{
			return new Point(MathHelper.SafeDivide(absolute.X - bound.X, bound.Width, 1), MathHelper.SafeDivide(absolute.Y - bound.Y, bound.Height, 1));
		}

		internal static Rect ActualBounds(this FrameworkElement element)
		{
			return new Rect(0, 0, element.ActualWidth, element.ActualHeight);
		}

		internal static void ApplyTransform(this IList<Point> points, GeneralTransform transform)
		{
			for (int i = 0; i < points.Count; i++)
			{
				points[i] = transform.Transform(points[i]);
			}
		}

		private static bool ArcSegmentEquals(ArcSegment firstArcSegment, ArcSegment secondArcSegment)
		{
			if (!(firstArcSegment.Point == secondArcSegment.Point) || firstArcSegment.IsLargeArc != secondArcSegment.IsLargeArc || firstArcSegment.RotationAngle != secondArcSegment.RotationAngle || !(firstArcSegment.Size == secondArcSegment.Size))
			{
				return false;
			}
			return firstArcSegment.SweepDirection == secondArcSegment.SweepDirection;
		}

		private static bool BezierSegmentEquals(BezierSegment firstBezierSegment, BezierSegment secondBezierSegment)
		{
			if (!(firstBezierSegment.Point1 == secondBezierSegment.Point1) || !(firstBezierSegment.Point2 == secondBezierSegment.Point2))
			{
				return false;
			}
			return firstBezierSegment.Point3 == secondBezierSegment.Point3;
		}

		internal static Point BottomLeft(this Rect rect)
		{
			return new Point(rect.Left, rect.Bottom);
		}

		internal static Point BottomRight(this Rect rect)
		{
			return new Point(rect.Right, rect.Bottom);
		}

		internal static Rect Bounds(this Size size)
		{
			return new Rect(0, 0, size.Width, size.Height);
		}

		internal static Point Center(this Rect rect)
		{
			return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
		}

		public static PathGeometry ConvertToPathGeometry(string abbreviatedGeometry)
		{
			return PathGeometryHelper.ConvertToPathGeometry(abbreviatedGeometry);
		}

		internal static double Determinant(Point lhs, Point rhs)
		{
			return lhs.X * rhs.Y - lhs.Y * rhs.X;
		}

		internal static double Distance(Point lhs, Point rhs)
		{
			double x = lhs.X - rhs.X;
			double y = lhs.Y - rhs.Y;
			return Math.Sqrt(x * x + y * y);
		}

		internal static double Dot(Vector lhs, Vector rhs)
		{
			return lhs.X * rhs.X + lhs.Y * rhs.Y;
		}

		internal static double Dot(Point lhs, Point rhs)
		{
			return lhs.X * rhs.X + lhs.Y * rhs.Y;
		}

		private static bool EllipseGeometryEquals(EllipseGeometry firstGeometry, EllipseGeometry secondGeometry)
		{
			if (!(firstGeometry.Center == secondGeometry.Center) || firstGeometry.RadiusX != secondGeometry.RadiusX)
			{
				return false;
			}
			return firstGeometry.RadiusY == secondGeometry.RadiusY;
		}

		internal static bool EnsureGeometryType<T>(out T result, ref Geometry value, Func<T> factory)
		where T : Geometry
		{
			result = (T)(value as T);
			if (result != null)
			{
				return false;
			}
			T t = factory();
			T t1 = t;
			result = t;
			value = t1;
			return true;
		}

		internal static bool EnsureGeometryType<T>(out T result, IList<Geometry> value, int index, Func<T> factory)
		where T : Geometry
		{
			result = (T)(value[index] as T);
			if (result != null)
			{
				return false;
			}
			T t = factory();
			T t1 = t;
			result = t;
			value[index] = t1;
			return true;
		}

		internal static bool EnsureSegmentType<T>(out T result, IList<PathSegment> list, int index, Func<T> factory)
		where T : PathSegment
		{
			result = (T)(list[index] as T);
			if (result != null)
			{
				return false;
			}
			T t = factory();
			T t1 = t;
			result = t;
			list[index] = t1;
			return true;
		}

		public static void FlattenFigure(PathFigure figure, IList<Point> points, double tolerance)
		{
			PathFigureHelper.FlattenFigure(figure, points, tolerance, false);
		}

		internal static bool GeometryEquals(Geometry firstGeometry, Geometry secondGeometry)
		{
			if (firstGeometry == secondGeometry)
			{
				return true;
			}
			if (firstGeometry == null || secondGeometry == null)
			{
				return false;
			}
			if (firstGeometry.GetType() != secondGeometry.GetType())
			{
				return false;
			}
			if (!firstGeometry.Transform.TransformEquals(secondGeometry.Transform))
			{
				return false;
			}
			PathGeometry pathGeometry = firstGeometry as PathGeometry;
			PathGeometry pathGeometry1 = secondGeometry as PathGeometry;
			if (pathGeometry != null && pathGeometry1 != null)
			{
				string str = pathGeometry.ToString();
				string str1 = pathGeometry1.ToString();
				if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str1))
				{
					return str == str1;
				}
				return GeometryHelper.PathGeometryEquals(pathGeometry, pathGeometry1);
			}
			LineGeometry lineGeometry = firstGeometry as LineGeometry;
			LineGeometry lineGeometry1 = secondGeometry as LineGeometry;
			if (lineGeometry != null && lineGeometry1 != null)
			{
				return GeometryHelper.LineGeometryEquals(lineGeometry, lineGeometry1);
			}
			RectangleGeometry rectangleGeometry = firstGeometry as RectangleGeometry;
			RectangleGeometry rectangleGeometry1 = secondGeometry as RectangleGeometry;
			if (rectangleGeometry != null && rectangleGeometry1 != null)
			{
				return GeometryHelper.RectangleGeometryEquals(rectangleGeometry, rectangleGeometry1);
			}
			EllipseGeometry ellipseGeometry = firstGeometry as EllipseGeometry;
			EllipseGeometry ellipseGeometry1 = secondGeometry as EllipseGeometry;
			if (ellipseGeometry != null && ellipseGeometry1 != null)
			{
				return GeometryHelper.EllipseGeometryEquals(ellipseGeometry, ellipseGeometry1);
			}
			GeometryGroup geometryGroup = firstGeometry as GeometryGroup;
			GeometryGroup geometryGroup1 = secondGeometry as GeometryGroup;
			if (geometryGroup == null || geometryGroup1 == null)
			{
				return false;
			}
			return GeometryHelper.GeometryGroupEquals(geometryGroup, geometryGroup1);
		}

		private static bool GeometryGroupEquals(GeometryGroup firstGeometry, GeometryGroup secondGeometry)
		{
			if (firstGeometry.FillRule != secondGeometry.FillRule)
			{
				return false;
			}
			if (firstGeometry.Children.Count != secondGeometry.Children.Count)
			{
				return false;
			}
			for (int i = 0; i < firstGeometry.Children.Count; i++)
			{
				if (!GeometryHelper.GeometryEquals(firstGeometry.Children[i], secondGeometry.Children[i]))
				{
					return false;
				}
			}
			return true;
		}

		internal static double GetArcAngle(Point point)
		{
			return Math.Atan2(point.Y - 0.5, point.X - 0.5) * 180 / 3.14159265358979 + 90;
		}

		internal static double GetArcAngle(Point point, Rect bound)
		{
			return GeometryHelper.GetArcAngle(GeometryHelper.AbsoluteToRelativePoint(bound, point));
		}

		internal static Point GetArcPoint(double degree)
		{
			double num = degree * 3.14159265358979 / 180;
			return new Point(0.5 + 0.5 * Math.Sin(num), 0.5 - 0.5 * Math.Cos(num));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static Point GetArcPoint(double degree, Rect bound)
		{
			return GeometryHelper.RelativeToAbsolutePoint(bound, GeometryHelper.GetArcPoint(degree));
		}

		internal static Rect GetStretchBound(Rect logicalBound, Stretch stretch, Size aspectRatio)
		{
			if (stretch == Stretch.None)
			{
				stretch = Stretch.Fill;
			}
			if (stretch == Stretch.Fill || !aspectRatio.HasValidArea())
			{
				return logicalBound;
			}
			Point point = logicalBound.Center();
			if (stretch == Stretch.Uniform)
			{
				if (aspectRatio.Width * logicalBound.Height >= logicalBound.Width * aspectRatio.Height)
				{
					logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
				}
				else
				{
					logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
				}
			}
			else if (stretch == Stretch.UniformToFill)
			{
				if (aspectRatio.Width * logicalBound.Height >= logicalBound.Width * aspectRatio.Height)
				{
					logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
				}
				else
				{
					logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
				}
			}
			return new Rect(point.X - logicalBound.Width / 2, point.Y - logicalBound.Height / 2, logicalBound.Width, logicalBound.Height);
		}

		internal static bool HasValidArea(this Size size)
		{
			if (size.Width <= 0 || size.Height <= 0 || double.IsInfinity(size.Width))
			{
				return false;
			}
			return !double.IsInfinity(size.Height);
		}

		internal static Rect Inflate(Rect rect, double offset)
		{
			return GeometryHelper.Inflate(rect, new Thickness(offset));
		}

		internal static Rect Inflate(Rect rect, double offsetX, double offsetY)
		{
			return GeometryHelper.Inflate(rect, new Thickness(offsetX, offsetY, offsetX, offsetY));
		}

		internal static Rect Inflate(Rect rect, Size size)
		{
			return GeometryHelper.Inflate(rect, new Thickness(size.Width, size.Height, size.Width, size.Height));
		}

		internal static Rect Inflate(Rect rect, Thickness thickness)
		{
			double width = rect.Width + thickness.Left + thickness.Right;
			double height = rect.Height + thickness.Top + thickness.Bottom;
			double x = rect.X - thickness.Left;
			if (width < 0)
			{
				x = x + width / 2;
				width = 0;
			}
			double y = rect.Y - thickness.Top;
			if (height < 0)
			{
				y = y + height / 2;
				height = 0;
			}
			return new Rect(x, y, width, height);
		}

		internal static Point Lerp(Point pointA, Point pointB, double alpha)
		{
			return new Point(MathHelper.Lerp(pointA.X, pointB.X, alpha), MathHelper.Lerp(pointA.Y, pointB.Y, alpha));
		}

		internal static Vector Lerp(Vector vectorA, Vector vectorB, double alpha)
		{
			return new Vector(MathHelper.Lerp(vectorA.X, vectorB.X, alpha), MathHelper.Lerp(vectorA.Y, vectorB.Y, alpha));
		}

		private static bool LineGeometryEquals(LineGeometry firstGeometry, LineGeometry secondGeometry)
		{
			if (firstGeometry.StartPoint != secondGeometry.StartPoint)
			{
				return false;
			}
			return firstGeometry.EndPoint == secondGeometry.EndPoint;
		}

		private static bool LineSegmentEquals(LineSegment firstLineSegment, LineSegment secondLineSegment)
		{
			return firstLineSegment.Point == secondLineSegment.Point;
		}

		internal static Point Midpoint(Point lhs, Point rhs)
		{
			return new Point((lhs.X + rhs.X) / 2, (lhs.Y + rhs.Y) / 2);
		}

		internal static Point Minus(this Point lhs, Point rhs)
		{
			return new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		internal static Thickness Negate(this Thickness thickness)
		{
			return new Thickness(-thickness.Left, -thickness.Top, -thickness.Right, -thickness.Bottom);
		}

		internal static Vector Normal(Point lhs, Point rhs)
		{
			return (new Vector(lhs.Y - rhs.Y, rhs.X - lhs.X)).Normalized();
		}

		internal static Vector Normalized(this Vector vector)
		{
			Vector vector1 = new Vector(vector.X, vector.Y);
			double length = vector1.Length;
			if (MathHelper.IsVerySmall(length))
			{
				return new Vector(0, 1);
			}
			return vector1 / length;
		}

		private static bool PathFigureEquals(PathFigure firstFigure, PathFigure secondFigure)
		{
			if (firstFigure.IsClosed != secondFigure.IsClosed)
			{
				return false;
			}
			if (firstFigure.IsFilled != secondFigure.IsFilled)
			{
				return false;
			}
			if (firstFigure.StartPoint != secondFigure.StartPoint)
			{
				return false;
			}
			for (int i = 0; i < firstFigure.Segments.Count; i++)
			{
				if (!GeometryHelper.PathSegmentEquals(firstFigure.Segments[i], secondFigure.Segments[i]))
				{
					return false;
				}
			}
			return true;
		}

		internal static bool PathGeometryEquals(PathGeometry firstGeometry, PathGeometry secondGeometry)
		{
			if (firstGeometry.FillRule != secondGeometry.FillRule)
			{
				return false;
			}
			if (firstGeometry.Figures.Count != secondGeometry.Figures.Count)
			{
				return false;
			}
			for (int i = 0; i < firstGeometry.Figures.Count; i++)
			{
				if (!GeometryHelper.PathFigureEquals(firstGeometry.Figures[i], secondGeometry.Figures[i]))
				{
					return false;
				}
			}
			return true;
		}

		private static bool PathSegmentEquals(PathSegment firstSegment, PathSegment secondSegment)
		{
			if (firstSegment == secondSegment)
			{
				return true;
			}
			if (firstSegment == null || secondSegment == null)
			{
				return false;
			}
			if (firstSegment.GetType() != secondSegment.GetType())
			{
				return false;
			}
			if (firstSegment.IsStroked() != secondSegment.IsStroked())
			{
				return false;
			}
			if (firstSegment.IsSmoothJoin() != secondSegment.IsSmoothJoin())
			{
				return false;
			}
			LineSegment lineSegment = firstSegment as LineSegment;
			LineSegment lineSegment1 = secondSegment as LineSegment;
			if (lineSegment != null && lineSegment1 != null)
			{
				return GeometryHelper.LineSegmentEquals(lineSegment, lineSegment1);
			}
			BezierSegment bezierSegment = firstSegment as BezierSegment;
			BezierSegment bezierSegment1 = secondSegment as BezierSegment;
			if (bezierSegment != null && bezierSegment1 != null)
			{
				return GeometryHelper.BezierSegmentEquals(bezierSegment, bezierSegment1);
			}
			QuadraticBezierSegment quadraticBezierSegment = firstSegment as QuadraticBezierSegment;
			QuadraticBezierSegment quadraticBezierSegment1 = secondSegment as QuadraticBezierSegment;
			if (quadraticBezierSegment != null && quadraticBezierSegment1 != null)
			{
				return GeometryHelper.QuadraticBezierSegmentEquals(quadraticBezierSegment, quadraticBezierSegment1);
			}
			ArcSegment arcSegment = firstSegment as ArcSegment;
			ArcSegment arcSegment1 = secondSegment as ArcSegment;
			if (arcSegment != null && arcSegment1 != null)
			{
				return GeometryHelper.ArcSegmentEquals(arcSegment, arcSegment1);
			}
			PolyLineSegment polyLineSegment = firstSegment as PolyLineSegment;
			PolyLineSegment polyLineSegment1 = secondSegment as PolyLineSegment;
			if (polyLineSegment != null && polyLineSegment1 != null)
			{
				return GeometryHelper.PolyLineSegmentEquals(polyLineSegment, polyLineSegment1);
			}
			PolyBezierSegment polyBezierSegment = firstSegment as PolyBezierSegment;
			PolyBezierSegment polyBezierSegment1 = secondSegment as PolyBezierSegment;
			if (polyBezierSegment != null && polyBezierSegment1 != null)
			{
				return GeometryHelper.PolyBezierSegmentEquals(polyBezierSegment, polyBezierSegment1);
			}
			PolyQuadraticBezierSegment polyQuadraticBezierSegment = firstSegment as PolyQuadraticBezierSegment;
			PolyQuadraticBezierSegment polyQuadraticBezierSegment1 = secondSegment as PolyQuadraticBezierSegment;
			if (polyQuadraticBezierSegment == null || polyQuadraticBezierSegment1 == null)
			{
				return false;
			}
			return GeometryHelper.PolyQuadraticBezierSegmentEquals(polyQuadraticBezierSegment, polyQuadraticBezierSegment1);
		}

		internal static Vector Perpendicular(this Vector vector)
		{
			return new Vector(-vector.Y, vector.X);
		}

		internal static Point Plus(this Point lhs, Point rhs)
		{
			return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		private static bool PolyBezierSegmentEquals(PolyBezierSegment firstPolyBezierSegment, PolyBezierSegment secondPolyBezierSegment)
		{
			if (firstPolyBezierSegment.Points.Count != secondPolyBezierSegment.Points.Count)
			{
				return false;
			}
			for (int i = 0; i < firstPolyBezierSegment.Points.Count; i++)
			{
				if (firstPolyBezierSegment.Points[i] != secondPolyBezierSegment.Points[i])
				{
					return false;
				}
			}
			return true;
		}

		private static bool PolyLineSegmentEquals(PolyLineSegment firstPolyLineSegment, PolyLineSegment secondPolyLineSegment)
		{
			if (firstPolyLineSegment.Points.Count != secondPolyLineSegment.Points.Count)
			{
				return false;
			}
			for (int i = 0; i < firstPolyLineSegment.Points.Count; i++)
			{
				if (firstPolyLineSegment.Points[i] != secondPolyLineSegment.Points[i])
				{
					return false;
				}
			}
			return true;
		}

		private static bool PolyQuadraticBezierSegmentEquals(PolyQuadraticBezierSegment firstPolyQuadraticBezierSegment, PolyQuadraticBezierSegment secondPolyQuadraticBezierSegment)
		{
			if (firstPolyQuadraticBezierSegment.Points.Count != secondPolyQuadraticBezierSegment.Points.Count)
			{
				return false;
			}
			for (int i = 0; i < firstPolyQuadraticBezierSegment.Points.Count; i++)
			{
				if (firstPolyQuadraticBezierSegment.Points[i] != secondPolyQuadraticBezierSegment.Points[i])
				{
					return false;
				}
			}
			return true;
		}

		private static bool QuadraticBezierSegmentEquals(QuadraticBezierSegment firstQuadraticBezierSegment, QuadraticBezierSegment secondQuadraticBezierSegment)
		{
			if (firstQuadraticBezierSegment.Point1 != secondQuadraticBezierSegment.Point1)
			{
				return false;
			}
			return firstQuadraticBezierSegment.Point1 == secondQuadraticBezierSegment.Point1;
		}

		private static bool RectangleGeometryEquals(RectangleGeometry firstGeometry, RectangleGeometry secondGeometry)
		{
			if (!(firstGeometry.Rect == secondGeometry.Rect) || firstGeometry.RadiusX != secondGeometry.RadiusX)
			{
				return false;
			}
			return firstGeometry.RadiusY == secondGeometry.RadiusY;
		}

		internal static Point RelativeToAbsolutePoint(Rect bound, Point relative)
		{
			return new Point(bound.X + relative.X * bound.Width, bound.Y + relative.Y * bound.Height);
		}

		internal static Transform RelativeTransform(Rect from, Rect to)
		{
			Point point = from.Center();
			Point point1 = to.Center();
			TransformGroup transformGroup = new TransformGroup();
			TransformCollection transformCollection = new TransformCollection();
			TranslateTransform translateTransform = new TranslateTransform()
			{
				X = -point.X,
				Y = -point.Y
			};
			transformCollection.Add(translateTransform);
			ScaleTransform scaleTransform = new ScaleTransform()
			{
				ScaleX = MathHelper.SafeDivide(to.Width, from.Width, 1),
				ScaleY = MathHelper.SafeDivide(to.Height, from.Height, 1)
			};
			transformCollection.Add(scaleTransform);
			TranslateTransform translateTransform1 = new TranslateTransform()
			{
				X = point1.X,
				Y = point1.Y
			};
			transformCollection.Add(translateTransform1);
			transformGroup.Children = transformCollection;
			return transformGroup;
		}

		internal static GeneralTransform RelativeTransform(UIElement from, UIElement to)
		{
			GeneralTransform visual;
			if (from == null || to == null)
			{
				return null;
			}
			try
			{
				visual = from.TransformToVisual(to);
			}
			catch (ArgumentException argumentException)
			{
				visual = null;
			}
			catch (InvalidOperationException invalidOperationException)
			{
				visual = null;
			}
			return visual;
		}

		internal static Rect Resize(this Rect rect, double ratio)
		{
			return rect.Resize(ratio, ratio);
		}

		internal static Rect Resize(this Rect rect, double ratioX, double ratioY)
		{
			Point point = rect.Center();
			double width = rect.Width * ratioX;
			double height = rect.Height * ratioY;
			return new Rect(point.X - width / 2, point.Y - height / 2, width, height);
		}

		internal static Point SafeTransform(GeneralTransform transform, Point point)
		{
			Point point1 = point;
			if (transform != null && transform.TryTransform(point, out point1))
			{
				return point1;
			}
			return point;
		}

		internal static Size Size(this Rect rect)
		{
			return new Size(rect.Width, rect.Height);
		}

		internal static double SquaredDistance(Point lhs, Point rhs)
		{
			double x = lhs.X - rhs.X;
			double y = lhs.Y - rhs.Y;
			return x * x + y * y;
		}

		internal static Thickness Subtract(this Rect lhs, Rect rhs)
		{
			return new Thickness(rhs.Left - lhs.Left, rhs.Top - lhs.Top, lhs.Right - rhs.Right, lhs.Bottom - rhs.Bottom);
		}

		internal static Vector Subtract(this Point lhs, Point rhs)
		{
			return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		internal static Point TopLeft(this Rect rect)
		{
			return new Point(rect.Left, rect.Top);
		}

		internal static Point TopRight(this Rect rect)
		{
			return new Point(rect.Right, rect.Top);
		}
	}
}