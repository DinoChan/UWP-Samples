using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Expression.Drawing.Core
{
	internal static class BezierCurveFlattener
	{
		public const double StandardFlatteningTolerance = 0.25;

		private static void DoCubicForwardDifferencing(Point[] controlPoints, double leftParameter, double rightParameter, double inverseErrorTolerance, ICollection<Point> resultPolyline, ICollection<double> resultParameters)
		{
			double num;
			double x = controlPoints[1].X - controlPoints[0].X;
			double y = controlPoints[1].Y - controlPoints[0].Y;
			double x1 = controlPoints[2].X - controlPoints[1].X;
			double y1 = controlPoints[2].Y - controlPoints[1].Y;
			double num1 = controlPoints[3].X - controlPoints[2].X;
			double y2 = controlPoints[3].Y - controlPoints[2].Y;
			double num2 = x1 - x;
			double num3 = y1 - y;
			double num4 = num1 - x1;
			double num5 = y2 - y1;
			double num6 = num4 - num2;
			double num7 = num5 - num3;
			Vector vector = controlPoints[3].Subtract(controlPoints[0]);
			double length = vector.Length;
			num = (MathHelper.IsVerySmall(length) ? Math.Max(0, Math.Max(GeometryHelper.Distance(controlPoints[1], controlPoints[0]), GeometryHelper.Distance(controlPoints[2], controlPoints[0]))) : Math.Max(0, Math.Max(Math.Abs((num2 * vector.Y - num3 * vector.X) / length), Math.Abs((num4 * vector.Y - num5 * vector.X) / length))));
			uint num8 = 0;
			if (num > 0)
			{
				double num9 = num * inverseErrorTolerance;
				num8 = (num9 < 2147483647 ? BezierCurveFlattener.Log4UnsignedInt32((uint)(num9 + 0.5)) : BezierCurveFlattener.Log4Double(num9));
			}
			int num10 = (int)(-num8);
			int num11 = num10 + num10;
			int num12 = num11 + num10;
			double num13 = MathHelper.DoubleFromMantissaAndExponent(3 * num2, num11);
			double num14 = MathHelper.DoubleFromMantissaAndExponent(3 * num3, num11);
			double num15 = MathHelper.DoubleFromMantissaAndExponent(6 * num6, num12);
			double num16 = MathHelper.DoubleFromMantissaAndExponent(6 * num7, num12);
			double num17 = MathHelper.DoubleFromMantissaAndExponent(3 * x, num10) + num13 + 0.166666666666667 * num15;
			double num18 = MathHelper.DoubleFromMantissaAndExponent(3 * y, num10) + num14 + 0.166666666666667 * num16;
			double num19 = 2 * num13 + num15;
			double num20 = 2 * num14 + num16;
			double x2 = controlPoints[0].X;
			double y3 = controlPoints[0].Y;
			Point point = new Point(0, 0);
			int num21 = (int)(1 << (int)(num8 & 31));
			double num22 = (num21 > 0 ? (rightParameter - leftParameter) / (double)num21 : 0);
			double num23 = leftParameter;
			for (int i = 1; i < num21; i++)
			{
				x2 = x2 + num17;
				y3 = y3 + num18;
				point.X = x2;
				point.Y = y3;
				resultPolyline.Add(point);
				num23 = num23 + num22;
				if (resultParameters != null)
				{
					resultParameters.Add(num23);
				}
				num17 = num17 + num19;
				num18 = num18 + num20;
				num19 = num19 + num15;
				num20 = num20 + num16;
			}
		}

		private static void DoCubicMidpointSubdivision(Point[] controlPoints, uint depth, double leftParameter, double rightParameter, double inverseErrorTolerance, ICollection<Point> resultPolyline, ICollection<double> resultParameters)
		{
			Point[] pointArray = new Point[] { controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3] };
			Point[] pointArray1 = pointArray;
			Point[] pointArray2 = new Point[] { default(Point), default(Point), default(Point), pointArray1[3] };
			pointArray1[3] = GeometryHelper.Midpoint(pointArray1[3], pointArray1[2]);
			pointArray1[2] = GeometryHelper.Midpoint(pointArray1[2], pointArray1[1]);
			pointArray1[1] = GeometryHelper.Midpoint(pointArray1[1], pointArray1[0]);
			pointArray2[2] = pointArray1[3];
			pointArray1[3] = GeometryHelper.Midpoint(pointArray1[3], pointArray1[2]);
			pointArray1[2] = GeometryHelper.Midpoint(pointArray1[2], pointArray1[1]);
			pointArray2[1] = pointArray1[3];
			pointArray1[3] = GeometryHelper.Midpoint(pointArray1[3], pointArray1[2]);
			pointArray2[0] = pointArray1[3];
			depth--;
			double num = (leftParameter + rightParameter) * 0.5;
			if (depth <= 0)
			{
				BezierCurveFlattener.DoCubicForwardDifferencing(pointArray1, leftParameter, num, inverseErrorTolerance, resultPolyline, resultParameters);
				resultPolyline.Add(pointArray2[0]);
				if (resultParameters != null)
				{
					resultParameters.Add(num);
				}
				BezierCurveFlattener.DoCubicForwardDifferencing(pointArray2, num, rightParameter, inverseErrorTolerance, resultPolyline, resultParameters);
				return;
			}
			BezierCurveFlattener.DoCubicMidpointSubdivision(pointArray1, depth, leftParameter, num, inverseErrorTolerance, resultPolyline, resultParameters);
			resultPolyline.Add(pointArray2[0]);
			if (resultParameters != null)
			{
				resultParameters.Add(num);
			}
			BezierCurveFlattener.DoCubicMidpointSubdivision(pointArray2, depth, num, rightParameter, inverseErrorTolerance, resultPolyline, resultParameters);
		}

		private static void EnsureErrorTolerance(ref double errorTolerance)
		{
			if (errorTolerance <= 0)
			{
				errorTolerance = 0.25;
			}
		}

		public static void FlattenCubic(Point[] controlPoints, double errorTolerance, ICollection<Point> resultPolyline, bool skipFirstPoint, ICollection<double> resultParameters = null)
		{
			if (resultPolyline == null)
			{
				throw new ArgumentNullException("resultPolyline");
			}
			if (controlPoints == null)
			{
				throw new ArgumentNullException("controlPoints");
			}
			if ((int)controlPoints.Length != 4)
			{
				throw new ArgumentOutOfRangeException("controlPoints");
			}
			BezierCurveFlattener.EnsureErrorTolerance(ref errorTolerance);
			if (!skipFirstPoint)
			{
				resultPolyline.Add(controlPoints[0]);
				if (resultParameters != null)
				{
					resultParameters.Add(0);
				}
			}
			if (!BezierCurveFlattener.IsCubicChordMonotone(controlPoints, errorTolerance * errorTolerance))
			{
				double x = controlPoints[3].X - controlPoints[2].X + controlPoints[1].X - controlPoints[0].X;
				double y = controlPoints[3].Y - controlPoints[2].Y + controlPoints[1].Y - controlPoints[0].Y;
				double num = 1 / errorTolerance;
				uint num1 = BezierCurveFlattener.Log8UnsignedInt32((uint)(MathHelper.Hypotenuse(x, y) * num + 0.5));
				if (num1 > 0)
				{
					num1--;
				}
				if (num1 <= 0)
				{
					BezierCurveFlattener.DoCubicForwardDifferencing(controlPoints, 0, 1, 0.75 * num, resultPolyline, resultParameters);
				}
				else
				{
					BezierCurveFlattener.DoCubicMidpointSubdivision(controlPoints, num1, 0, 1, 0.75 * num, resultPolyline, resultParameters);
				}
			}
			else
			{
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener = new BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener(controlPoints, errorTolerance, errorTolerance, true);
				Point point = new Point();
				double num2 = 0;
				while (adaptiveForwardDifferencingCubicFlattener.Next(ref point, ref num2))
				{
					resultPolyline.Add(point);
					if (resultParameters == null)
					{
						continue;
					}
					resultParameters.Add(num2);
				}
			}
			resultPolyline.Add(controlPoints[3]);
			if (resultParameters != null)
			{
				resultParameters.Add(1);
			}
		}

		public static void FlattenQuadratic(Point[] controlPoints, double errorTolerance, ICollection<Point> resultPolyline, bool skipFirstPoint, ICollection<double> resultParameters = null)
		{
			if (resultPolyline == null)
			{
				throw new ArgumentNullException("resultPolyline");
			}
			if (controlPoints == null)
			{
				throw new ArgumentNullException("controlPoints");
			}
			if ((int)controlPoints.Length != 3)
			{
				throw new ArgumentOutOfRangeException("controlPoints");
			}
			BezierCurveFlattener.EnsureErrorTolerance(ref errorTolerance);
			Point[] pointArray = new Point[] { controlPoints[0], GeometryHelper.Lerp(controlPoints[0], controlPoints[1], 0.666666666666667), GeometryHelper.Lerp(controlPoints[1], controlPoints[2], 0.333333333333333), controlPoints[2] };
			BezierCurveFlattener.FlattenCubic(pointArray, errorTolerance, resultPolyline, skipFirstPoint, resultParameters);
		}

		private static bool IsCubicChordMonotone(Point[] controlPoints, double squaredTolerance)
		{
			double num = GeometryHelper.SquaredDistance(controlPoints[0], controlPoints[3]);
			if (num <= squaredTolerance)
			{
				return false;
			}
			Vector vector = controlPoints[3].Subtract(controlPoints[0]);
			Vector vector1 = controlPoints[1].Subtract(controlPoints[0]);
			double num1 = GeometryHelper.Dot(vector, vector1);
			if (num1 < 0 || num1 > num)
			{
				return false;
			}
			Vector vector2 = controlPoints[2].Subtract(controlPoints[0]);
			double num2 = GeometryHelper.Dot(vector, vector2);
			if (num2 < 0 || num2 > num)
			{
				return false;
			}
			if (num1 > num2)
			{
				return false;
			}
			return true;
		}

		private static uint Log4Double(double d)
		{
			uint num = 0;
			while (d > 1)
			{
				d = d * 0.25;
				num++;
			}
			return num;
		}

		private static uint Log4UnsignedInt32(uint i)
		{
			uint num = 0;
			while (i > 0)
			{
				i = i >> 2;
				num++;
			}
			return num;
		}

		private static uint Log8UnsignedInt32(uint i)
		{
			uint num = 0;
			while (i > 0)
			{
				i = i >> 3;
				num++;
			}
			return num;
		}

		private class AdaptiveForwardDifferencingCubicFlattener
		{
			private double aX;

			private double aY;

			private double bX;

			private double bY;

			private double cX;

			private double cY;

			private double dX;

			private double dY;

			private int numSteps;

			private double flatnessTolerance;

			private double distanceTolerance;

			private bool doParameters;

			private double parameter;

			private double dParameter;

			internal AdaptiveForwardDifferencingCubicFlattener(Point[] controlPoints, double flatnessTolerance, double distanceTolerance, bool doParameters)
			{
				this.flatnessTolerance = 3 * flatnessTolerance;
				this.distanceTolerance = distanceTolerance;
				this.doParameters = doParameters;
				this.aX = -controlPoints[0].X + 3 * (controlPoints[1].X - controlPoints[2].X) + controlPoints[3].X;
				this.aY = -controlPoints[0].Y + 3 * (controlPoints[1].Y - controlPoints[2].Y) + controlPoints[3].Y;
				this.bX = 3 * (controlPoints[0].X - 2 * controlPoints[1].X + controlPoints[2].X);
				this.bY = 3 * (controlPoints[0].Y - 2 * controlPoints[1].Y + controlPoints[2].Y);
				this.cX = 3 * (-controlPoints[0].X + controlPoints[1].X);
				this.cY = 3 * (-controlPoints[0].Y + controlPoints[1].Y);
				this.dX = controlPoints[0].X;
				this.dY = controlPoints[0].Y;
			}

			private AdaptiveForwardDifferencingCubicFlattener()
			{
			}

			private void DoubleStepSize()
			{
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener = this;
				adaptiveForwardDifferencingCubicFlattener.aX = adaptiveForwardDifferencingCubicFlattener.aX * 8;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener1 = this;
				adaptiveForwardDifferencingCubicFlattener1.aY = adaptiveForwardDifferencingCubicFlattener1.aY * 8;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener2 = this;
				adaptiveForwardDifferencingCubicFlattener2.bX = adaptiveForwardDifferencingCubicFlattener2.bX * 4;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener3 = this;
				adaptiveForwardDifferencingCubicFlattener3.bY = adaptiveForwardDifferencingCubicFlattener3.bY * 4;
				this.cX = this.cX + this.cX;
				this.cY = this.cY + this.cY;
				if (this.doParameters)
				{
					BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener4 = this;
					adaptiveForwardDifferencingCubicFlattener4.dParameter = adaptiveForwardDifferencingCubicFlattener4.dParameter * 2;
				}
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener5 = this;
				adaptiveForwardDifferencingCubicFlattener5.numSteps = adaptiveForwardDifferencingCubicFlattener5.numSteps >> 1;
			}

			private void HalveStepSize()
			{
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener = this;
				adaptiveForwardDifferencingCubicFlattener.aX = adaptiveForwardDifferencingCubicFlattener.aX * 0.125;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener1 = this;
				adaptiveForwardDifferencingCubicFlattener1.aY = adaptiveForwardDifferencingCubicFlattener1.aY * 0.125;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener2 = this;
				adaptiveForwardDifferencingCubicFlattener2.bX = adaptiveForwardDifferencingCubicFlattener2.bX * 0.25;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener3 = this;
				adaptiveForwardDifferencingCubicFlattener3.bY = adaptiveForwardDifferencingCubicFlattener3.bY * 0.25;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener4 = this;
				adaptiveForwardDifferencingCubicFlattener4.cX = adaptiveForwardDifferencingCubicFlattener4.cX * 0.5;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener5 = this;
				adaptiveForwardDifferencingCubicFlattener5.cY = adaptiveForwardDifferencingCubicFlattener5.cY * 0.5;
				if (this.doParameters)
				{
					BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener6 = this;
					adaptiveForwardDifferencingCubicFlattener6.dParameter = adaptiveForwardDifferencingCubicFlattener6.dParameter * 0.5;
				}
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener7 = this;
				adaptiveForwardDifferencingCubicFlattener7.numSteps = adaptiveForwardDifferencingCubicFlattener7.numSteps << 1;
			}

			private void IncrementDifferencesAndParameter()
			{
				this.dX = this.aX + this.bX + this.cX + this.dX;
				this.dY = this.aY + this.bY + this.cY + this.dY;
				this.cX = this.aX + this.aX + this.aX + this.bX + this.bX + this.cX;
				this.cY = this.aY + this.aY + this.aY + this.bY + this.bY + this.cY;
				this.bX = this.aX + this.aX + this.aX + this.bX;
				this.bY = this.aY + this.aY + this.aY + this.bY;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener = this;
				adaptiveForwardDifferencingCubicFlattener.numSteps = adaptiveForwardDifferencingCubicFlattener.numSteps - 1;
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener1 = this;
				adaptiveForwardDifferencingCubicFlattener1.parameter = adaptiveForwardDifferencingCubicFlattener1.parameter + this.dParameter;
			}

			private bool MustSubdivide(double flatnessTolerance)
			{
				double num = -(this.aY + this.bY + this.cY);
				double num1 = this.aX + this.bX + this.cX;
				double num2 = Math.Abs(num) + Math.Abs(num1);
				if (num2 <= this.distanceTolerance)
				{
					return false;
				}
				num2 = num2 * flatnessTolerance;
				if (Math.Abs(this.cX * num + this.cY * num1) > num2)
				{
					return true;
				}
				if (Math.Abs((this.bX + this.cX + this.cX) * num + (this.bY + this.cY + this.cY) * num1) <= num2)
				{
					return false;
				}
				return true;
			}

			internal bool Next(ref Point p, ref double u)
			{
				while (this.MustSubdivide(this.flatnessTolerance))
				{
					this.HalveStepSize();
				}
				if ((this.numSteps & 1) == 0)
				{
					while (this.numSteps > 1 && !this.MustSubdivide(this.flatnessTolerance * 0.25))
					{
						this.DoubleStepSize();
					}
				}
				this.IncrementDifferencesAndParameter();
				p.X = this.dX;
				p.Y = this.dY;
				u = this.parameter;
				return this.numSteps != 0;
			}
		}
	}
}