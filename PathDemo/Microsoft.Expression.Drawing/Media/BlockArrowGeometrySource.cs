using Microsoft.Expression.Drawing.Core;
using System;
using System.Windows;

namespace Microsoft.Expression.Media
{
	internal class BlockArrowGeometrySource : GeometrySource<IBlockArrowGeometrySourceParameters>
	{
		private Point[] points;

		public BlockArrowGeometrySource()
		{
		}

		private void EnsurePoints(int count)
		{
			if (this.points == null || (int)this.points.Length != count)
			{
				this.points = new Point[count];
			}
		}

		private static BlockArrowGeometrySource.ArrowBuilder GetBuilder(ArrowOrientation orientation)
		{
			switch (orientation)
			{
				case ArrowOrientation.Left:
				{
					return new BlockArrowGeometrySource.LeftArrowBuilder();
				}
				case ArrowOrientation.Right:
				{
					return new BlockArrowGeometrySource.RightArrowBuilder();
				}
				case ArrowOrientation.Up:
				{
					return new BlockArrowGeometrySource.UpArrowBuilder();
				}
				case ArrowOrientation.Down:
				{
					return new BlockArrowGeometrySource.DownArrowBuilder();
				}
				default:
				{
					return new BlockArrowGeometrySource.RightArrowBuilder();
				}
			}
		}

		protected override bool UpdateCachedGeometry(IBlockArrowGeometrySourceParameters parameters)
		{
			bool flag = false;
			BlockArrowGeometrySource.ArrowBuilder builder = BlockArrowGeometrySource.GetBuilder(parameters.Orientation);
			double num = builder.ArrowLength(base.LogicalBounds);
			double num1 = builder.ArrowWidth(base.LogicalBounds);
			double num2 = num1 / 2 / num;
			double num3 = MathHelper.EnsureRange(parameters.ArrowheadAngle, new double?(0), new double?(180));
			double num4 = Math.Tan(num3 * 3.14159265358979 / 180 / 2);
			if (num4 >= num2)
			{
				double num5 = num1 / 2 / num4;
				double num6 = MathHelper.EnsureRange(parameters.ArrowBodySize, new double?(0), new double?(1));
				double num7 = num1 / 2 * (1 - num6);
				this.EnsurePoints(7);
				this.points[0] = builder.ComputePointA(num, num1);
				this.points[1] = builder.ComputePointB(num, num5);
				PointPair pointPair = builder.ComputePointCD(num, num5, num7);
				this.points[2] = pointPair.Item1;
				this.points[3] = pointPair.Item2;
				this.points[4] = builder.GetMirrorPoint(this.points[3], num1);
				this.points[5] = builder.GetMirrorPoint(this.points[2], num1);
				this.points[6] = builder.GetMirrorPoint(this.points[1], num1);
			}
			else
			{
				this.EnsurePoints(3);
				this.points[0] = builder.ComputePointA(num, num1);
				this.points[1] = builder.ComputePointB(num, num);
				this.points[2] = builder.GetMirrorPoint(this.points[1], num1);
			}
			for (int i = 0; i < (int)this.points.Length; i++)
			{
				double x = this.points[i].X;
				Rect logicalBounds = base.LogicalBounds;
				this.points[i].X = x + logicalBounds.Left;
				double y = this.points[i].Y;
				Rect rect = base.LogicalBounds;
				this.points[i].Y = y + rect.Top;
			}
			flag = flag | PathGeometryHelper.SyncPolylineGeometry(ref this.cachedGeometry, this.points, true);
			return flag;
		}

		private abstract class ArrowBuilder
		{
			protected ArrowBuilder()
			{
			}

			public abstract double ArrowLength(Rect bounds);

			public abstract double ArrowWidth(Rect bounds);

			public abstract Point ComputePointA(double length, double width);

			public abstract Point ComputePointB(double length, double offset);

			public abstract PointPair ComputePointCD(double length, double offset, double thickness);

			public abstract Point GetMirrorPoint(Point point, double width);
		}

		private class DownArrowBuilder : BlockArrowGeometrySource.VerticalArrowBuilder
		{
			public DownArrowBuilder()
			{
			}

			public override Point ComputePointA(double length, double width)
			{
				return new Point(width / 2, length);
			}

			public override Point ComputePointB(double length, double offset)
			{
				return new Point(0, length - offset);
			}

			public override PointPair ComputePointCD(double length, double offset, double thickness)
			{
				return new PointPair(new Point(thickness, length - offset), new Point(thickness, 0));
			}
		}

		private abstract class HorizontalArrowBuilder : BlockArrowGeometrySource.ArrowBuilder
		{
			protected HorizontalArrowBuilder()
			{
			}

			public override double ArrowLength(Rect bounds)
			{
				return bounds.Width;
			}

			public override double ArrowWidth(Rect bounds)
			{
				return bounds.Height;
			}

			public override Point GetMirrorPoint(Point point, double width)
			{
				return new Point(point.X, width - point.Y);
			}
		}

		private class LeftArrowBuilder : BlockArrowGeometrySource.HorizontalArrowBuilder
		{
			public LeftArrowBuilder()
			{
			}

			public override Point ComputePointA(double length, double width)
			{
				return new Point(0, width / 2);
			}

			public override Point ComputePointB(double length, double offset)
			{
				return new Point(offset, 0);
			}

			public override PointPair ComputePointCD(double length, double offset, double thickness)
			{
				return new PointPair(new Point(offset, thickness), new Point(length, thickness));
			}
		}

		private class RightArrowBuilder : BlockArrowGeometrySource.HorizontalArrowBuilder
		{
			public RightArrowBuilder()
			{
			}

			public override Point ComputePointA(double length, double width)
			{
				return new Point(length, width / 2);
			}

			public override Point ComputePointB(double length, double offset)
			{
				return new Point(length - offset, 0);
			}

			public override PointPair ComputePointCD(double length, double offset, double thickness)
			{
				return new PointPair(new Point(length - offset, thickness), new Point(0, thickness));
			}
		}

		private class UpArrowBuilder : BlockArrowGeometrySource.VerticalArrowBuilder
		{
			public UpArrowBuilder()
			{
			}

			public override Point ComputePointA(double length, double width)
			{
				return new Point(width / 2, 0);
			}

			public override Point ComputePointB(double length, double offset)
			{
				return new Point(0, offset);
			}

			public override PointPair ComputePointCD(double length, double offset, double thickness)
			{
				return new PointPair(new Point(thickness, offset), new Point(thickness, length));
			}
		}

		private abstract class VerticalArrowBuilder : BlockArrowGeometrySource.ArrowBuilder
		{
			protected VerticalArrowBuilder()
			{
			}

			public override double ArrowLength(Rect bounds)
			{
				return bounds.Height;
			}

			public override double ArrowWidth(Rect bounds)
			{
				return bounds.Width;
			}

			public override Point GetMirrorPoint(Point point, double width)
			{
				return new Point(width - point.X, point.Y);
			}
		}
	}
}