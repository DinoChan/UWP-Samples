using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	[DebuggerDisplay("({X}, {Y})")]
	internal struct Vector
	{
		public double Length
		{
			get
			{
				return Math.Sqrt(this.X * this.X + this.Y * this.Y);
			}
		}

		public double LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y;
			}
		}

		public double X
		{
			get;
			set;
		}

		public double Y
		{
			get;
			set;
		}

		public Vector(double x, double y)
		{
			this = new Vector()
			{
				X = x,
				Y = y
			};
		}

		public Vector(Point point)
		{
			this = new Vector()
			{
				X = point.X,
				Y = point.Y
			};
		}

		public static Vector Add(Vector vector1, Vector vector2)
		{
			return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
		}

		public static Point Add(Vector vector, Point point)
		{
			return new Point(point.X + vector.X, point.Y + vector.Y);
		}

		public static double AngleBetween(Vector vector1, Vector vector2)
		{
			double x = vector1.X * vector2.Y - vector2.X * vector1.Y;
			double num = vector1.X * vector2.X + vector1.Y * vector2.Y;
			return Math.Atan2(x, num) * 57.2957795130823;
		}

		public static double CrossProduct(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.Y - vector1.Y * vector2.X;
		}

		public static double Determinant(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.Y - vector1.Y * vector2.X;
		}

		public static Vector Divide(Vector vector, double scalar)
		{
			return vector * (1 / scalar);
		}

		public static bool Equals(Vector vector1, Vector vector2)
		{
			if (!vector1.X.Equals(vector2.X))
			{
				return false;
			}
			return vector1.Y.Equals(vector2.Y);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Vector))
			{
				return false;
			}
			return Vector.Equals(this, (Vector)obj);
		}

		public bool Equals(Vector value)
		{
			return Vector.Equals(this, value);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		public static Vector Multiply(Vector vector, double scalar)
		{
			return new Vector(vector.X * scalar, vector.Y * scalar);
		}

		public static Vector Multiply(double scalar, Vector vector)
		{
			return new Vector(vector.X * scalar, vector.Y * scalar);
		}

		public static Vector Multiply(Vector vector, Matrix matrix)
		{
			Point x = matrix.Transform(new Point(vector.X, vector.Y));
			x.X = x.X - matrix.OffsetX;
			x.Y = x.Y - matrix.OffsetY;
			return new Vector(x);
		}

		public static double Multiply(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y;
		}

		public void Negate()
		{
			this.X = -this.X;
			this.Y = -this.Y;
		}

		public void Normalize()
		{
			this = this / Math.Max(Math.Abs(this.X), Math.Abs(this.Y));
			this = this / this.Length;
		}

		public static Vector operator +(Vector vector1, Vector vector2)
		{
			return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
		}

		public static Point operator +(Vector vector, Point point)
		{
			return new Point(point.X + vector.X, point.Y + vector.Y);
		}

		public static Point operator +(Point point, Vector vector)
		{
			return new Point(point.X + vector.X, point.Y + vector.Y);
		}

		public static Vector operator /(Vector vector, double scalar)
		{
			return vector * (1 / scalar);
		}

		public static bool operator ==(Vector vector1, Vector vector2)
		{
			if (vector1.X != vector2.X)
			{
				return false;
			}
			return vector1.Y == vector2.Y;
		}

		public static explicit operator Size(Vector vector)
		{
			return vector.ToSize();
		}

		public static explicit operator Point(Vector vector)
		{
			return vector.ToPoint();
		}

		public static bool operator !=(Vector vector1, Vector vector2)
		{
			return !(vector1 == vector2);
		}

		public static Vector operator *(Vector vector, double scalar)
		{
			return new Vector(vector.X * scalar, vector.Y * scalar);
		}

		public static Vector operator *(double scalar, Vector vector)
		{
			return new Vector(vector.X * scalar, vector.Y * scalar);
		}

		public static Vector operator *(Vector vector, Matrix matrix)
		{
			Point x = matrix.Transform(new Point(vector.X, vector.Y));
			x.X = x.X - matrix.OffsetX;
			x.Y = x.Y - matrix.OffsetY;
			return new Vector(x);
		}

		public static double operator *(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y;
		}

		public static Vector operator -(Vector vector1, Vector vector2)
		{
			return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);
		}

		public static Point operator -(Point point, Vector vector)
		{
			return new Point(point.X - vector.X, point.Y - vector.Y);
		}

		public static Vector operator -(Vector vector)
		{
			return new Vector(-vector.X, -vector.Y);
		}

		public static Vector Subtract(Vector vector1, Vector vector2)
		{
			return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);
		}

		public Point ToPoint()
		{
			return new Point(this.X, this.Y);
		}

		public Size ToSize()
		{
			return new Size(Math.Abs(this.X), Math.Abs(this.Y));
		}
	}
}