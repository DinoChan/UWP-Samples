using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Microsoft.Expression.Drawing.Core
{
	internal class MarchLocation
	{
		public double After
		{
			get;
			private set;
		}

		public double Before
		{
			get;
			private set;
		}

		public int Index
		{
			get;
			private set;
		}

		public double Ratio
		{
			get;
			private set;
		}

		public MarchStopReason Reason
		{
			get;
			private set;
		}

		public double Remain
		{
			get;
			private set;
		}

		public MarchLocation()
		{
		}

		public static MarchLocation Create(MarchStopReason reason, int index, double before, double after, double remain)
		{
			double num = before + after;
			MarchLocation marchLocation = new MarchLocation()
			{
				Reason = reason,
				Index = index,
				Remain = remain,
				Before = MathHelper.EnsureRange(before, new double?(0), new double?(num)),
				After = MathHelper.EnsureRange(after, new double?(0), new double?(num)),
				Ratio = MathHelper.EnsureRange(MathHelper.SafeDivide(before, num, 0), new double?(0), new double?(1))
			};
			return marchLocation;
		}

		public double GetArcLength(IList<double> accumulatedLengths)
		{
			return MathHelper.Lerp(accumulatedLengths[this.Index], accumulatedLengths[this.Index + 1], this.Ratio);
		}

		public Vector GetNormal(PolylineData polyline, double cornerRadius = 0)
		{
			return polyline.SmoothNormal(this.Index, this.Ratio, cornerRadius);
		}

		public Point GetPoint(IList<Point> points)
		{
			return GeometryHelper.Lerp(points[this.Index], points[this.Index + 1], this.Ratio);
		}
	}
}