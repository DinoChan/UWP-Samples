using Microsoft.Expression.Drawing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Controls;

namespace Microsoft.Expression.Drawing.Core
{
	internal static class PathGeometryHelper
	{
		public static void ApplyTransform(this PathGeometry pathGeometry)
		{
			Transform transform = pathGeometry.Transform;
			if (transform != null && !transform.IsIdentity())
			{
				foreach (PathFigure figure in pathGeometry.Figures)
				{
					figure.ApplyTransform(transform);
				}
				pathGeometry.Transform = TransformHelper.IdentityTransform;
			}
		}

		public static PathGeometry AsPathGeometry(this Geometry original)
		{
			if (original == null)
			{
				return null;
			}
			PathGeometry pathGeometry = original as PathGeometry;
			if (pathGeometry == null)
			{
				if (!original.Bounds.Size().HasValidArea())
				{
					return GeometryHelper.EmptyGeometry;
				}
				PathGeometry pathGeometry1 = PathGeometryHelper.ConvertToPathGeometry(original as RectangleGeometry);
				pathGeometry = pathGeometry1;
				if (pathGeometry1 == null)
				{
					PathGeometry pathGeometry2 = PathGeometryHelper.ConvertToPathGeometry(original as EllipseGeometry);
					pathGeometry = pathGeometry2;
					if (pathGeometry2 == null)
					{
						PathGeometry pathGeometry3 = PathGeometryHelper.ConvertToPathGeometry(original as LineGeometry);
						pathGeometry = pathGeometry3;
						if (pathGeometry3 == null)
						{
							PathGeometry pathGeometry4 = PathGeometryHelper.ConvertToPathGeometry(original as GeometryGroup);
							pathGeometry = pathGeometry4;
							if (pathGeometry4 == null)
							{
								CultureInfo currentCulture = CultureInfo.CurrentCulture;
								string typeNotSupported = ExceptionStringTable.TypeNotSupported;
								object[] fullName = new object[] { original.GetType().FullName };
								throw new NotSupportedException(string.Format(currentCulture, typeNotSupported, fullName));
							}
						}
					}
				}
			}
			return pathGeometry;
		}

		private static ArcSegment CloneArcSegment(ArcSegment arcSegment)
		{
			ArcSegment arcSegment1 = new ArcSegment()
			{
				IsLargeArc = arcSegment.IsLargeArc,
				Point = arcSegment.Point,
				RotationAngle = arcSegment.RotationAngle,
				Size = arcSegment.Size,
				SweepDirection = arcSegment.SweepDirection
			};
			return arcSegment1;
		}

		private static BezierSegment CloneBezierSegment(BezierSegment bezierSegment)
		{
			BezierSegment bezierSegment1 = new BezierSegment()
			{
				Point1 = bezierSegment.Point1,
				Point2 = bezierSegment.Point2,
				Point3 = bezierSegment.Point3
			};
			return bezierSegment1;
		}

		public static Geometry CloneCurrentValue(this Geometry geometry)
		{
			if (geometry == null)
			{
				return null;
			}
			PathGeometry pathGeometry = geometry as PathGeometry;
			if (pathGeometry != null)
			{
				return PathGeometryHelper.ClonePathGeometry(pathGeometry);
			}
			LineGeometry lineGeometry = geometry as LineGeometry;
			if (lineGeometry != null)
			{
				return PathGeometryHelper.CloneLineGeometry(lineGeometry);
			}
			EllipseGeometry ellipseGeometry = geometry as EllipseGeometry;
			if (ellipseGeometry != null)
			{
				return PathGeometryHelper.CloneEllipseGeometry(ellipseGeometry);
			}
			RectangleGeometry rectangleGeometry = geometry as RectangleGeometry;
			if (rectangleGeometry != null)
			{
				return PathGeometryHelper.CloneRectangleGeometry(rectangleGeometry);
			}
			GeometryGroup geometryGroup = geometry as GeometryGroup;
			if (geometryGroup != null)
			{
				return PathGeometryHelper.CloneGeometryGroup(geometryGroup);
			}
			return geometry.DeepCopy<Geometry>();
		}

		private static EllipseGeometry CloneEllipseGeometry(EllipseGeometry ellipseGeometry)
		{
			EllipseGeometry ellipseGeometry1 = new EllipseGeometry()
			{
				Center = ellipseGeometry.Center,
				RadiusX = ellipseGeometry.RadiusX,
				RadiusY = ellipseGeometry.RadiusY,
				Transform = ellipseGeometry.Transform.CloneTransform()
			};
			return ellipseGeometry1;
		}

		private static GeometryGroup CloneGeometryGroup(GeometryGroup geometryGroup)
		{
			GeometryGroup geometryGroup1 = new GeometryGroup()
			{
				FillRule = geometryGroup.FillRule,
				Transform = geometryGroup.Transform.CloneTransform()
			};
			GeometryGroup geometryGroup2 = geometryGroup1;
			foreach (Geometry child in geometryGroup.Children)
			{
				geometryGroup2.Children.Add(child.CloneCurrentValue());
			}
			return geometryGroup2;
		}

		private static LineGeometry CloneLineGeometry(LineGeometry lineGeometry)
		{
			LineGeometry lineGeometry1 = new LineGeometry()
			{
				StartPoint = lineGeometry.StartPoint,
				EndPoint = lineGeometry.EndPoint,
				Transform = lineGeometry.Transform.CloneTransform()
			};
			return lineGeometry1;
		}

		private static LineSegment CloneLineSegment(LineSegment lineSegment)
		{
			return new LineSegment()
			{
				Point = lineSegment.Point
			};
		}

		private static PathFigure ClonePathFigure(PathFigure pathFigure)
		{
			PathFigure pathFigure1 = new PathFigure()
			{
				IsClosed = pathFigure.IsClosed,
				IsFilled = pathFigure.IsFilled,
				StartPoint = pathFigure.StartPoint
			};
			PathFigure pathFigure2 = pathFigure1;
			foreach (PathSegment segment in pathFigure.Segments)
			{
				pathFigure2.Segments.Add(PathGeometryHelper.ClonePathSegment(segment));
			}
			return pathFigure2;
		}

		private static PathGeometry ClonePathGeometry(PathGeometry pathGeometry)
		{
			PathGeometry pathGeometry1 = new PathGeometry()
			{
				FillRule = pathGeometry.FillRule,
				Transform = pathGeometry.Transform.CloneTransform()
			};
			PathGeometry pathGeometry2 = pathGeometry1;
			foreach (PathFigure figure in pathGeometry.Figures)
			{
				pathGeometry2.Figures.Add(PathGeometryHelper.ClonePathFigure(figure));
			}
			return pathGeometry2;
		}

		private static PathSegment ClonePathSegment(PathSegment pathSegment)
		{
			if (pathSegment == null)
			{
				return null;
			}
			LineSegment lineSegment = pathSegment as LineSegment;
			if (lineSegment != null)
			{
				return PathGeometryHelper.CloneLineSegment(lineSegment);
			}
			BezierSegment bezierSegment = pathSegment as BezierSegment;
			if (bezierSegment != null)
			{
				return PathGeometryHelper.CloneBezierSegment(bezierSegment);
			}
			QuadraticBezierSegment quadraticBezierSegment = pathSegment as QuadraticBezierSegment;
			if (quadraticBezierSegment != null)
			{
				return PathGeometryHelper.CloneQuadraticBezierSegment(quadraticBezierSegment);
			}
			ArcSegment arcSegment = pathSegment as ArcSegment;
			if (arcSegment != null)
			{
				return PathGeometryHelper.CloneArcSegment(arcSegment);
			}
			PolyLineSegment polyLineSegment = pathSegment as PolyLineSegment;
			if (polyLineSegment != null)
			{
				return PathGeometryHelper.ClonePolyLineSegment(polyLineSegment);
			}
			PolyBezierSegment polyBezierSegment = pathSegment as PolyBezierSegment;
			if (polyBezierSegment != null)
			{
				return PathGeometryHelper.ClonePolyBezierSegment(polyBezierSegment);
			}
			PolyQuadraticBezierSegment polyQuadraticBezierSegment = pathSegment as PolyQuadraticBezierSegment;
			if (polyQuadraticBezierSegment != null)
			{
				return PathGeometryHelper.ClonePolyQuadraticBezierSegment(polyQuadraticBezierSegment);
			}
			return pathSegment.DeepCopy<PathSegment>();
		}

		private static PolyBezierSegment ClonePolyBezierSegment(PolyBezierSegment polyBezierSegment)
		{
			PolyBezierSegment polyBezierSegment1 = new PolyBezierSegment();
			foreach (Point point in polyBezierSegment.Points)
			{
				polyBezierSegment1.Points.Add(point);
			}
			return polyBezierSegment1;
		}

		private static PolyLineSegment ClonePolyLineSegment(PolyLineSegment polyLineSegment)
		{
			PolyLineSegment polyLineSegment1 = new PolyLineSegment();
			foreach (Point point in polyLineSegment.Points)
			{
				polyLineSegment1.Points.Add(point);
			}
			return polyLineSegment1;
		}

		private static PolyQuadraticBezierSegment ClonePolyQuadraticBezierSegment(PolyQuadraticBezierSegment polyQuadraticBezierSegment)
		{
			PolyQuadraticBezierSegment polyQuadraticBezierSegment1 = new PolyQuadraticBezierSegment();
			foreach (Point point in polyQuadraticBezierSegment.Points)
			{
				polyQuadraticBezierSegment1.Points.Add(point);
			}
			return polyQuadraticBezierSegment1;
		}

		private static QuadraticBezierSegment CloneQuadraticBezierSegment(QuadraticBezierSegment quadraticBezierSegment)
		{
			QuadraticBezierSegment quadraticBezierSegment1 = new QuadraticBezierSegment()
			{
				Point1 = quadraticBezierSegment.Point1,
				Point2 = quadraticBezierSegment.Point2
			};
			return quadraticBezierSegment1;
		}

		private static RectangleGeometry CloneRectangleGeometry(RectangleGeometry rectangleGeometry)
		{
			RectangleGeometry rectangleGeometry1 = new RectangleGeometry()
			{
				Rect = rectangleGeometry.Rect,
				RadiusX = rectangleGeometry.RadiusX,
				RadiusY = rectangleGeometry.RadiusY,
				Transform = rectangleGeometry.Transform.CloneTransform()
			};
			return rectangleGeometry1;
		}

		internal static PathGeometry ConvertToPathGeometry(string abbreviatedGeometry)
		{
			if (abbreviatedGeometry == null)
			{
				throw new ArgumentNullException("abbreviatedGeometry");
			}
			PathGeometry pathGeometry = new PathGeometry()
			{
				Figures = new PathFigureCollection()
			};
			int num = 0;
			while (num < abbreviatedGeometry.Length && char.IsWhiteSpace(abbreviatedGeometry, num))
			{
				num++;
			}
			if (num < abbreviatedGeometry.Length && abbreviatedGeometry[num] == 'F')
			{
				num++;
				while (num < abbreviatedGeometry.Length && char.IsWhiteSpace(abbreviatedGeometry, num))
				{
					num++;
				}
				if (num == abbreviatedGeometry.Length || abbreviatedGeometry[num] != '0' && abbreviatedGeometry[num] != '1')
				{
					throw new FormatException();
				}
				pathGeometry.FillRule = (abbreviatedGeometry[num] == '0' ? FillRule.EvenOdd : FillRule.Nonzero);
				num++;
			}
			(new PathGeometryHelper.AbbreviatedGeometryParser(pathGeometry)).Parse(abbreviatedGeometry, num);
			return pathGeometry;
		}

		private static PathGeometry ConvertToPathGeometry(EllipseGeometry ellipseGeometry)
		{
			if (ellipseGeometry == null)
			{
				return null;
			}
			Rect bounds = ellipseGeometry.Bounds;
			Point point = GeometryHelper.Lerp(bounds.TopLeft(), bounds.TopRight(), 0.5);
			Point point1 = GeometryHelper.Lerp(bounds.BottomLeft(), bounds.BottomRight(), 0.5);
			Size size = new Size(ellipseGeometry.RadiusX, ellipseGeometry.RadiusY);
			PathGeometry pathGeometry = new PathGeometry()
			{
				Transform = ellipseGeometry.Transform
			};
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			PathFigure pathFigure = new PathFigure()
			{
				StartPoint = point,
				IsClosed = true,
				IsFilled = true
			};
			PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
			ArcSegment arcSegment = new ArcSegment()
			{
				Point = point1,
				IsLargeArc = true,
				Size = size,
				SweepDirection = SweepDirection.Clockwise
			};
			pathSegmentCollection.Add(arcSegment);
			ArcSegment arcSegment1 = new ArcSegment()
			{
				Point = point,
				IsLargeArc = true,
				Size = size,
				SweepDirection = SweepDirection.Clockwise
			};
			pathSegmentCollection.Add(arcSegment1);
			pathFigure.Segments = pathSegmentCollection;
			pathFigureCollection.Add(pathFigure);
			pathGeometry.Figures = pathFigureCollection;
			return pathGeometry;
		}

		private static PathGeometry ConvertToPathGeometry(RectangleGeometry rectangleGeometry)
		{
			if (rectangleGeometry == null)
			{
				return null;
			}
			Rect bounds = rectangleGeometry.Bounds;
			PathGeometry pathGeometry = new PathGeometry()
			{
				Transform = rectangleGeometry.Transform
			};
			PathFigure pathFigure = new PathFigure()
			{
				IsClosed = true,
				IsFilled = true
			};
			PathFigure point = pathFigure;
			pathGeometry.Figures.Add(point);
			if (rectangleGeometry.RadiusX * rectangleGeometry.RadiusY == 0)
			{
				point.StartPoint = bounds.TopLeft();
				PathSegmentCollection segments = point.Segments;
				PolyLineSegment polyLineSegment = new PolyLineSegment();
				PointCollection pointCollection = new PointCollection()
				{
					bounds.TopRight(),
					bounds.BottomRight(),
					bounds.BottomLeft()
				};
				polyLineSegment.Points = pointCollection;
				segments.Add(polyLineSegment);
				return pathGeometry;
			}
			bool flag = Math.Abs(rectangleGeometry.RadiusX) < bounds.Width / 2;
			bool flag1 = Math.Abs(rectangleGeometry.RadiusY) < bounds.Height / 2;
			double num = Math.Min(Math.Abs(rectangleGeometry.RadiusX), bounds.Width / 2);
			double num1 = Math.Min(Math.Abs(rectangleGeometry.RadiusY), bounds.Height / 2);
			Size size = new Size(num, num1);
			point.StartPoint = new Point(bounds.Left, bounds.Top + num1);
			point.Segments.Add(PathGeometryHelper.CreateCorner(new Point(bounds.Left + num, bounds.Top), size));
			if (flag)
			{
				PathSegmentCollection pathSegmentCollection = point.Segments;
				LineSegment lineSegment = new LineSegment()
				{
					Point = new Point(bounds.Right - num, bounds.Top)
				};
				pathSegmentCollection.Add(lineSegment);
			}
			point.Segments.Add(PathGeometryHelper.CreateCorner(new Point(bounds.Right, bounds.Top + num1), size));
			if (flag1)
			{
				PathSegmentCollection segments1 = point.Segments;
				LineSegment lineSegment1 = new LineSegment()
				{
					Point = new Point(bounds.Right, bounds.Bottom - num1)
				};
				segments1.Add(lineSegment1);
			}
			point.Segments.Add(PathGeometryHelper.CreateCorner(new Point(bounds.Right - num, bounds.Bottom), size));
			if (flag)
			{
				PathSegmentCollection pathSegmentCollection1 = point.Segments;
				LineSegment lineSegment2 = new LineSegment()
				{
					Point = new Point(bounds.Left + num, bounds.Bottom)
				};
				pathSegmentCollection1.Add(lineSegment2);
			}
			point.Segments.Add(PathGeometryHelper.CreateCorner(new Point(bounds.Left, bounds.Bottom - num1), size));
			return pathGeometry;
		}

		private static PathGeometry ConvertToPathGeometry(LineGeometry lineGeometry)
		{
			if (lineGeometry == null)
			{
				return null;
			}
			PathGeometry pathGeometry = new PathGeometry()
			{
				Transform = lineGeometry.Transform
			};
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			PathFigure pathFigure = new PathFigure()
			{
				StartPoint = lineGeometry.StartPoint,
				IsClosed = false,
				IsFilled = false
			};
			PathSegmentCollection pathSegmentCollection = new PathSegmentCollection()
			{
				new LineSegment()
				{
					Point = lineGeometry.EndPoint
				}
			};
			pathFigure.Segments = pathSegmentCollection;
			pathFigureCollection.Add(pathFigure);
			pathGeometry.Figures = pathFigureCollection;
			return pathGeometry;
		}

		private static PathGeometry ConvertToPathGeometry(GeometryGroup geometryGroup)
		{
			if (geometryGroup == null)
			{
				return null;
			}
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.SetIfDifferent(PathGeometry.FillRuleProperty, geometryGroup.FillRule);
			LinkedList<Geometry> geometries = new LinkedList<Geometry>();
			geometries.AddLast(geometryGroup);
			while (geometries.Count > 0)
			{
				LinkedListNode<Geometry> first = geometries.First;
				Geometry value = first.Value;
				GeometryGroup geometryGroup1 = value as GeometryGroup;
				if (geometryGroup1 == null)
				{
					PathGeometry pathGeometry1 = value.CopyAsPathGeometry();
					if (pathGeometry1 != null)
					{
						pathGeometry1.ApplyTransform();
						for (int i = pathGeometry1.Figures.Count - 1; i >= 0; i--)
						{
							PathFigure item = pathGeometry1.Figures[i];
							pathGeometry1.Figures.RemoveAt(i);
							pathGeometry.Figures.Add(item);
						}
					}
				}
				else
				{
					foreach (Geometry child in geometryGroup1.Children)
					{
						geometries.AddAfter(first, child);
					}
				}
				geometries.RemoveFirst();
			}
			return pathGeometry;
		}

		public static PathGeometry CopyAsPathGeometry(this Geometry original)
		{
			PathGeometry pathGeometry = original.AsPathGeometry();
			if (pathGeometry != null && pathGeometry == original)
			{
				pathGeometry = (PathGeometry)pathGeometry.CloneCurrentValue();
			}
			return pathGeometry;
		}

		private static ArcSegment CreateCorner(Point endPoint, Size cornerSize)
		{
			ArcSegment arcSegment = new ArcSegment()
			{
				IsLargeArc = false,
				SweepDirection = SweepDirection.Clockwise,
				Point = endPoint,
				Size = cornerSize
			};
			return arcSegment;
		}

		internal static Geometry FixPathGeometryBoundary(Geometry geometry)
		{
			PathGeometry pathGeometry = geometry as PathGeometry;
			if (pathGeometry != null)
			{
				PathFigureCollection figures = pathGeometry.Figures;
				pathGeometry.Figures = null;
				pathGeometry = PathGeometryHelper.ClonePathGeometry(pathGeometry);
				pathGeometry.Figures = figures;
				geometry = pathGeometry;
			}
			return geometry;
		}

		public static bool IsFrozen(this Geometry geometry)
		{
			return true;
		}

		public static bool IsSmoothJoin(this PathSegment pathSegment)
		{
			return false;
		}

		public static bool IsStroked(this PathSegment pathSegment)
		{
			return true;
		}

		public static bool SyncPolylineGeometry(ref Geometry geometry, IList<Point> points, bool isClosed)
		{
			PathFigure pathFigure;
			bool flag = false;
			PathGeometry pathGeometry = geometry as PathGeometry;
			if (pathGeometry != null && pathGeometry.Figures.Count == 1)
			{
				PathFigure item = pathGeometry.Figures[0];
				pathFigure = item;
				if (item != null)
				{
					flag = flag | PathFigureHelper.SyncPolylineFigure(pathFigure, points, isClosed, true);
					return flag;
				}
			}
			PathGeometry pathGeometry1 = new PathGeometry();
			pathGeometry = pathGeometry1;
			geometry = pathGeometry1;
			PathFigureCollection figures = pathGeometry.Figures;
			PathFigure pathFigure1 = new PathFigure();
			pathFigure = pathFigure1;
			figures.Add(pathFigure1);
			flag = true;
			flag = flag | PathFigureHelper.SyncPolylineFigure(pathFigure, points, isClosed, true);
			return flag;
		}

		private class AbbreviatedGeometryParser
		{
			private PathGeometry geometry;

			private PathFigure figure;

			private Point lastPoint;

			private Point secondLastPoint;

			private string buffer;

			private int index;

			private int length;

			private char token;

			public AbbreviatedGeometryParser(PathGeometry geometry)
			{
				this.geometry = geometry;
			}

			private void ArcTo(Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection, Point point)
			{
				ArcSegment arcSegment = new ArcSegment()
				{
					Size = size,
					RotationAngle = rotationAngle,
					IsLargeArc = isLargeArc,
					SweepDirection = sweepDirection,
					Point = point
				};
				this.figure.Segments.Add(arcSegment);
			}

			private void BeginFigure(Point startPoint)
			{
				this.FinishFigure(false);
				this.EnsureFigure();
				this.figure.StartPoint = startPoint;
				this.figure.IsFilled = true;
			}

			private void BezierTo(Point point1, Point point2, Point point3)
			{
				BezierSegment bezierSegment = new BezierSegment()
				{
					Point1 = point1,
					Point2 = point2,
					Point3 = point3
				};
				this.figure.Segments.Add(bezierSegment);
			}

			private void EnsureFigure()
			{
				if (this.figure == null)
				{
					this.figure = new PathFigure()
					{
						Segments = new PathSegmentCollection()
					};
				}
			}

			private void FinishFigure(bool figureExplicitlyClosed)
			{
				if (this.figure != null)
				{
					if (figureExplicitlyClosed)
					{
						this.figure.IsClosed = true;
					}
					this.geometry.Figures.Add(this.figure);
					this.figure = null;
				}
			}

			private Point GetSmoothBeizerFirstPoint()
			{
				Point x = this.lastPoint;
				if (this.figure.Segments.Count > 0)
				{
					BezierSegment item = this.figure.Segments[this.figure.Segments.Count - 1] as BezierSegment;
					if (item != null)
					{
						Point point2 = item.Point2;
						x.X = x.X + (this.lastPoint.X - point2.X);
						x.Y = x.Y + (this.lastPoint.Y - point2.Y);
					}
				}
				return x;
			}

			private bool IsNumber(bool allowComma)
			{
				bool flag = this.SkipWhitespace(allowComma);
				if (this.index < this.length)
				{
					this.token = this.buffer[this.index];
					if (this.token == '.' || this.token == '-' || this.token == '+' || this.token >= '0' && this.token <= '9' || this.token == 'I' || this.token == 'N')
					{
						return true;
					}
				}
				if (flag)
				{
					throw new FormatException();
				}
				return false;
			}

			private void LineTo(Point point)
			{
				LineSegment lineSegment = new LineSegment()
				{
					Point = point
				};
				this.figure.Segments.Add(lineSegment);
			}

			public void Parse(string data, int startIndex)
			{
			//	char chr;
			//	char chr1;
			//	this.buffer = data;
			//	this.length = data.Length;
			//	this.index = startIndex;
			//	bool flag = true;
			//	char chr2 = ' ';
			//	while (true)
			//	{
			//	Label11:
			//		if (!this.ReadToken())
			//		{
			//			this.FinishFigure(false);
			//			return;
			//		}
			//		chr = this.token;
			//		if (flag)
			//		{
			//			if (chr != 'M' && chr != 'm')
			//			{
			//				throw new FormatException();
			//			}
			//			flag = false;
			//		}
			//		chr1 = chr;
			//		if (chr1 > 'Z')
			//		{
			//			if (chr1 <= 'm')
			//			{
			//				goto Label9;
			//			}
			//			switch (chr1)
			//			{
			//				case 'q':
			//				{
			//					goto Label0;
			//				}
			//				case 'r':
			//				{
			//					throw new NotSupportedException();
			//				}
			//				case 's':
			//				{
			//					goto Label2;
			//				}
			//				default:
			//				{
			//					if (chr1 == 'v')
			//					{
			//						goto Label3;
			//					}
			//					if (chr1 == 'z')
			//					{
			//						break;
			//					}
			//					throw new NotSupportedException();
			//				}
			//			}
			//		}
			//		else
			//		{
			//			if (chr1 <= 'M')
			//			{
			//				goto Label10;
			//			}
			//			switch (chr1)
			//			{
			//				case 'Q':
			//				{
			//					goto Label0;
			//				}
			//				case 'R':
			//				{
			//					throw new NotSupportedException();
			//				}
			//				case 'S':
			//				{
			//					goto Label2;
			//				}
			//				default:
			//				{
			//					if (chr1 == 'V')
			//					{
			//						goto Label3;
			//					}
			//					if (chr1 == 'Z')
			//					{
			//						break;
			//					}
			//					throw new NotSupportedException();
			//				}
			//			}
			//		}
			//		this.FinishFigure(true);
			//	}
			//	throw new NotSupportedException();
			//Label0:
			//	this.EnsureFigure();
			//	do
			//	{
			//		Point point = this.ReadPoint(chr, false);
			//		this.lastPoint = this.ReadPoint(chr, true);
			//		this.QuadraticBezierTo(point, this.lastPoint);
			//		chr2 = 'Q';
			//	}
			//	while (this.IsNumber(true));
			//	goto Label11;
			//Label2:
			//	this.EnsureFigure();
			//	do
			//	{
			//		Point smoothBeizerFirstPoint = this.GetSmoothBeizerFirstPoint();
			//		Point point1 = this.ReadPoint(chr, false);
			//		this.lastPoint = this.ReadPoint(chr, true);
			//		this.BezierTo(smoothBeizerFirstPoint, point1, this.lastPoint);
			//		chr2 = 'S';
			//	}
			//	while (this.IsNumber(true));
			//	goto Label11;
			//Label3:
			//	this.EnsureFigure();
			//	do
			//	{
			//		double y = this.ReadDouble(false);
			//		if (chr == 'v')
			//		{
			//			y = y + this.lastPoint.Y;
			//		}
			//		this.lastPoint.Y = y;
			//		this.LineTo(this.lastPoint);
			//	}
			//	while (this.IsNumber(true));
			//	goto Label11;
			//Label4:
			//	do
			//	{
			//		Size size = this.ReadSize(false);
			//		double num = this.ReadDouble(true);
			//		bool flag1 = this.ReadBool01(true);
			//		SweepDirection sweepDirection = (this.ReadBool01(true) ? SweepDirection.Clockwise : SweepDirection.Counterclockwise);
			//		this.lastPoint = this.ReadPoint(chr, true);
			//		this.ArcTo(size, num, flag1, sweepDirection, this.lastPoint);
			//		chr2 = 'A';
			//	}
			//	while (this.IsNumber(true));
			//	this.EnsureFigure();
			//	goto Label11;
			//Label5:
			//	this.EnsureFigure();
			//	do
			//	{
			//		Point point2 = this.ReadPoint(chr, false);
			//		this.secondLastPoint = this.ReadPoint(chr, true);
			//		this.lastPoint = this.ReadPoint(chr, true);
			//		this.BezierTo(point2, this.secondLastPoint, this.lastPoint);
			//		chr2 = 'C';
			//	}
			//	while (this.IsNumber(true));
			//	goto Label11;
			//Label6:
			//	this.EnsureFigure();
			//	do
			//	{
			//		double x = this.ReadDouble(false);
			//		if (chr == 'h')
			//		{
			//			x = x + this.lastPoint.X;
			//		}
			//		this.lastPoint.X = x;
			//		this.LineTo(this.lastPoint);
			//	}
			//	while (this.IsNumber(true));
			//	goto Label11;
			//Label7:
			//	this.EnsureFigure();
			//	do
			//	{
			//		this.lastPoint = this.ReadPoint(chr, false);
			//		this.LineTo(this.lastPoint);
			//	}
			//	while (this.IsNumber(true));
			//	goto Label11;
			//Label8:
			//	this.lastPoint = this.ReadPoint(chr, false);
			//	this.BeginFigure(this.lastPoint);
			//	chr2 = 'M';
			//	while (this.IsNumber(true))
			//	{
			//		this.lastPoint = this.ReadPoint(chr2, false);
			//		this.LineTo(this.lastPoint);
			//		chr2 = 'L';
			//	}
			//	goto Label11;
			//Label9:
			//	switch (chr1)
			//	{
			//		case 'a':
			//		{
			//			goto Label4;
			//		}
			//		case 'b':
			//		{
			//			throw new NotSupportedException();
			//		}
			//		case 'c':
			//		{
			//			goto Label5;
			//		}
			//		default:
			//		{
			//			if (chr1 == 'h')
			//			{
			//				goto Label6;
			//			}
			//			switch (chr1)
			//			{
			//				case 'l':
			//				{
			//					goto Label7;
			//				}
			//				case 'm':
			//				{
			//					goto Label8;
			//				}
			//				default:
			//				{
			//					throw new NotSupportedException();
			//				}
			//			}
			//			break;
			//		}
			//	}
			//Label10:
			//	switch (chr1)
			//	{
			//		case 'A':
			//		{
			//			goto Label4;
			//		}
			//		case 'B':
			//		{
			//			throw new NotSupportedException();
			//		}
			//		case 'C':
			//		{
			//			goto Label5;
			//		}
			//		default:
			//		{
			//			if (chr1 == 'H')
			//			{
			//				goto Label6;
			//			}
			//			switch (chr1)
			//			{
			//				case 'L':
			//				{
			//					goto Label7;
			//				}
			//				case 'M':
			//				{
			//					goto Label8;
			//				}
			//				default:
			//				{
			//					throw new NotSupportedException();
			//				}
			//			}
			//			break;
			//		}
			//	}
			}

			private void QuadraticBezierTo(Point point1, Point point2)
			{
				QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment()
				{
					Point1 = point1,
					Point2 = point2
				};
				this.figure.Segments.Add(quadraticBezierSegment);
			}

			private bool ReadBool01(bool allowComma)
			{
				double num = this.ReadDouble(allowComma);
				if (num == 0)
				{
					return false;
				}
				if (num != 1)
				{
					throw new FormatException();
				}
				return true;
			}

			private double ReadDouble(bool allowComma)
			{
				double num;
				if (!this.IsNumber(allowComma))
				{
					throw new FormatException();
				}
				bool flag = true;
				int num1 = this.index;
				if (this.index < this.length && (this.buffer[this.index] == '-' || this.buffer[this.index] == '+'))
				{
					PathGeometryHelper.AbbreviatedGeometryParser abbreviatedGeometryParser = this;
					abbreviatedGeometryParser.index = abbreviatedGeometryParser.index + 1;
				}
				if (this.index < this.length && this.buffer[this.index] == 'I')
				{
					this.index = Math.Min(this.index + 8, this.length);
					flag = false;
				}
				else if (this.index >= this.length || this.buffer[this.index] != 'N')
				{
					this.SkipDigits(false);
					if (this.index < this.length && this.buffer[this.index] == '.')
					{
						flag = false;
						PathGeometryHelper.AbbreviatedGeometryParser abbreviatedGeometryParser1 = this;
						abbreviatedGeometryParser1.index = abbreviatedGeometryParser1.index + 1;
						this.SkipDigits(false);
					}
					if (this.index < this.length && (this.buffer[this.index] == 'E' || this.buffer[this.index] == 'e'))
					{
						flag = false;
						PathGeometryHelper.AbbreviatedGeometryParser abbreviatedGeometryParser2 = this;
						abbreviatedGeometryParser2.index = abbreviatedGeometryParser2.index + 1;
						this.SkipDigits(true);
					}
				}
				else
				{
					this.index = Math.Min(this.index + 3, this.length);
					flag = false;
				}
				if (!flag || this.index > num1 + 8)
				{
					string str = this.buffer.Substring(num1, this.index - num1);
					try
					{
						num = Convert.ToDouble(str, CultureInfo.InvariantCulture);
					}
					catch (FormatException formatException)
					{
						throw new FormatException();
					}
					return num;
				}
				int num2 = 1;
				if (this.buffer[num1] == '+')
				{
					num1++;
				}
				else if (this.buffer[num1] == '-')
				{
					num1++;
					num2 = -1;
				}
				int num3 = 0;
				while (num1 < this.index)
				{
					num3 = num3 * 10 + (this.buffer[num1] - 48);
					num1++;
				}
				return (double)(num3 * num2);
			}

			private Point ReadPoint(char command, bool allowComma)
			{
				double x = this.ReadDouble(allowComma);
				double y = this.ReadDouble(true);
				if (command >= 'a')
				{
					x = x + this.lastPoint.X;
					y = y + this.lastPoint.Y;
				}
				return new Point(x, y);
			}

			private Size ReadSize(bool allowComma)
			{
				double num = this.ReadDouble(allowComma);
				return new Size(num, this.ReadDouble(true));
			}

			private bool ReadToken()
			{
				this.SkipWhitespace(false);
				if (this.index >= this.length)
				{
					return false;
				}
				string str = this.buffer;
				PathGeometryHelper.AbbreviatedGeometryParser abbreviatedGeometryParser = this;
				int num = abbreviatedGeometryParser.index;
				int num1 = num;
				abbreviatedGeometryParser.index = num + 1;
				this.token = str[num1];
				return true;
			}

			private void SkipDigits(bool signAllowed)
			{
				if (signAllowed && this.index < this.length && (this.buffer[this.index] == '-' || this.buffer[this.index] == '+'))
				{
					PathGeometryHelper.AbbreviatedGeometryParser abbreviatedGeometryParser = this;
					abbreviatedGeometryParser.index = abbreviatedGeometryParser.index + 1;
				}
				while (this.index < this.length && this.buffer[this.index] >= '0' && this.buffer[this.index] <= '9')
				{
					PathGeometryHelper.AbbreviatedGeometryParser abbreviatedGeometryParser1 = this;
					abbreviatedGeometryParser1.index = abbreviatedGeometryParser1.index + 1;
				}
			}

			private bool SkipWhitespace(bool allowComma)
			{
				bool flag = false;
				while (this.index < this.length)
				{
					char chr = this.buffer[this.index];
					char chr1 = chr;
					switch (chr1)
					{
						case '\t':
						case '\n':
						case '\r':
						{
							PathGeometryHelper.AbbreviatedGeometryParser abbreviatedGeometryParser = this;
							abbreviatedGeometryParser.index = abbreviatedGeometryParser.index + 1;
							continue;
						}
						case '\v':
						case '\f':
						{
							if (chr > ' ' && chr <= 'z')
							{
								return flag;
							}
							if (char.IsWhiteSpace(chr))
							{
								goto case '\r';
							}
							return flag;
						}
						default:
						{
							if (chr1 == ' ')
							{
								goto case '\r';
							}
							if (chr1 != ',')
							{
								goto case '\f';
							}
							if (!allowComma)
							{
								throw new FormatException();
							}
							flag = true;
							allowComma = false;
							goto case '\r';
						}
					}
				}
				return false;
			}
		}
	}
}