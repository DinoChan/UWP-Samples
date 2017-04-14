using System;

namespace Microsoft.Expression.Drawing.Core
{
	internal static class MathHelper
	{
		public const double Epsilon = 1E-06;

		public const double TwoPI = 6.28318530717959;

		public const double PentagramInnerRadius = 0.47211;

		public static bool AreClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			return MathHelper.IsVerySmall(value1 - value2);
		}

		public static double DoubleFromMantissaAndExponent(double x, int exp)
		{
			return x * Math.Pow(2, (double)exp);
		}

		public static double EnsureRange(double value, double? min, double? max)
		{
			if (min.HasValue && value < min.Value)
			{
				return min.Value;
			}
			if (!max.HasValue || value <= max.Value)
			{
				return value;
			}
			return max.Value;
		}

		public static bool GreaterThan(double value1, double value2)
		{
			if (value1 <= value2)
			{
				return false;
			}
			return !MathHelper.AreClose(value1, value2);
		}

		public static bool GreaterThanOrClose(double value1, double value2)
		{
			if (value1 > value2)
			{
				return true;
			}
			return MathHelper.AreClose(value1, value2);
		}

		public static double Hypotenuse(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}

		public static bool IsFiniteDouble(double x)
		{
			if (double.IsInfinity(x))
			{
				return false;
			}
			return !double.IsNaN(x);
		}

		public static bool IsVerySmall(double value)
		{
			return Math.Abs(value) < 1E-06;
		}

		public static double Lerp(double x, double y, double alpha)
		{
			return x * (1 - alpha) + y * alpha;
		}

		public static bool LessThan(double value1, double value2)
		{
			if (value1 >= value2)
			{
				return false;
			}
			return !MathHelper.AreClose(value1, value2);
		}

		public static bool LessThanOrClose(double value1, double value2)
		{
			if (value1 < value2)
			{
				return true;
			}
			return MathHelper.AreClose(value1, value2);
		}

		public static double SafeDivide(double lhs, double rhs, double fallback)
		{
			if (MathHelper.IsVerySmall(rhs))
			{
				return fallback;
			}
			return lhs / rhs;
		}
	}
}