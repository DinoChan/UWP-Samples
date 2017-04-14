using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Expression.Drawing.Core
{
	internal class PolylineData
	{
		private IList<Point> points;

		private IList<Vector> normals;

		private IList<double> angles;

		private IList<double> lengths;

		private IList<double> accumulates;

		private double? totalLength;

		public IList<double> AccumulatedLength
		{
			get
			{
				return this.accumulates ?? this.ComputeAccumulatedLength();
			}
		}

		public IList<double> Angles
		{
			get
			{
				return this.angles ?? this.ComputeAngles();
			}
		}

		public int Count
		{
			get
			{
				return this.points.Count;
			}
		}

		public bool IsClosed
		{
			get
			{
				return this.points[0] == this.points.Last<Point>();
			}
		}

		public IList<double> Lengths
		{
			get
			{
				return this.lengths ?? this.ComputeLengths();
			}
		}

		public IList<Vector> Normals
		{
			get
			{
				return this.normals ?? this.ComputeNormals();
			}
		}

		public IList<Point> Points
		{
			get
			{
				return this.points;
			}
		}

		public double TotalLength
		{
			get
			{
				double? nullable = this.totalLength;
				if (!nullable.HasValue)
				{
					return this.ComputeTotalLength();
				}
				return (double)((double)nullable.GetValueOrDefault());
			}
		}

		public PolylineData(IList<Point> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (points.Count <= 1)
			{
				throw new ArgumentOutOfRangeException("points");
			}
			this.points = points;
		}

		private IList<double> ComputeAccumulatedLength()
		{
			this.accumulates = new double[this.Count];
			this.accumulates[0] = 0;
			for (int i = 1; i < this.Count; i++)
			{
				this.accumulates[i] = this.accumulates[i - 1] + this.Lengths[i - 1];
			}
			this.totalLength = new double?(this.accumulates.Last<double>());
			return this.accumulates;
		}

		private IList<double> ComputeAngles()
		{
			this.angles = new double[this.Count];
			for (int i = 1; i < this.Count - 1; i++)
			{
				this.angles[i] = -GeometryHelper.Dot(this.Normals[i - 1], this.Normals[i]);
			}
			if (!this.IsClosed)
			{
				IList<double> nums = this.angles;
				double num = 1;
				double num1 = num;
				this.angles[this.Count - 1] = num;
				nums[0] = num1;
			}
			else
			{
				double num2 = -GeometryHelper.Dot(this.Normals[0], this.Normals[this.Count - 2]);
				IList<double> nums1 = this.angles;
				double num3 = num2;
				double num4 = num3;
				this.angles[this.Count - 1] = num3;
				nums1[0] = num4;
			}
			return this.angles;
		}

		private IList<double> ComputeLengths()
		{
			this.lengths = new double[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				this.lengths[i] = this.Difference(i).Length;
			}
			return this.lengths;
		}

		private IList<Vector> ComputeNormals()
		{
			this.normals = new Vector[this.points.Count];
			for (int i = 0; i < this.Count - 1; i++)
			{
				this.normals[i] = GeometryHelper.Normal(this.points[i], this.points[i + 1]);
			}
			this.normals[this.Count - 1] = this.normals[this.Count - 2];
			return this.normals;
		}

		private double ComputeTotalLength()
		{
			this.ComputeAccumulatedLength();
			return this.totalLength.Value;
		}

		public Vector Difference(int index)
		{
			int num = (index + 1) % this.Count;
			return this.points[num].Subtract(this.points[index]);
		}

		public Vector SmoothNormal(int index, double fraction, double cornerRadius)
		{
			if (cornerRadius > 0)
			{
				double item = this.Lengths[index];
				if (MathHelper.IsVerySmall(item))
				{
					int count = index - 1;
					if (count < 0 && this.IsClosed)
					{
						count = this.Count - 1;
					}
					int num = index + 1;
					if (this.IsClosed && num >= this.Count - 1)
					{
						num = 0;
					}
					if (count < 0 || num >= this.Count)
					{
						return this.Normals[index];
					}
					return GeometryHelper.Lerp(this.Normals[num], this.Normals[count], 0.5).Normalized();
				}
				double num1 = Math.Min(cornerRadius / item, 0.5);
				if (fraction <= num1)
				{
					int count1 = index - 1;
					if (this.IsClosed && count1 == -1)
					{
						count1 = this.Count - 1;
					}
					if (count1 >= 0)
					{
						double num2 = (num1 - fraction) / (2 * num1);
						return GeometryHelper.Lerp(this.Normals[index], this.Normals[count1], num2).Normalized();
					}
				}
				else if (fraction >= 1 - num1)
				{
					int num3 = index + 1;
					if (this.IsClosed && num3 >= this.Count - 1)
					{
						num3 = 0;
					}
					if (num3 < this.Count)
					{
						double num4 = (fraction + num1 - 1) / (2 * num1);
						return GeometryHelper.Lerp(this.Normals[index], this.Normals[num3], num4).Normalized();
					}
				}
			}
			return this.Normals[index];
		}
	}
}