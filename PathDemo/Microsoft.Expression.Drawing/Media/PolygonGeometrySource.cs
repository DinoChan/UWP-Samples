using Microsoft.Expression.Drawing.Core;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Expression.Media
{
	internal class PolygonGeometrySource : GeometrySource<IPolygonGeometrySourceParameters>
	{
		private List<Point> cachedPoints = new List<Point>();

		public PolygonGeometrySource()
		{
		}

		protected override Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
		{
			Rect rect = base.ComputeLogicalBounds(layoutBounds, parameters);
			return GeometryHelper.GetStretchBound(rect, parameters.Stretch, new Size(1, 1));
		}

		protected override bool UpdateCachedGeometry(IPolygonGeometrySourceParameters parameters)
		{
			bool flag = false;
			int num = Math.Max(3, Math.Min(100, (int)Math.Round(parameters.PointCount)));
			double num1 = 360 / (double)num;
			double num2 = Math.Max(0, Math.Min(1, parameters.InnerRadius));
			if (num2 >= 1)
			{
				this.cachedPoints.EnsureListCount<Point>(num, null);
				for (int i = 0; i < num; i++)
				{
					double num3 = num1 * (double)i;
					this.cachedPoints[i] = GeometryHelper.GetArcPoint(num3, base.LogicalBounds);
				}
			}
			else
			{
				double num4 = Math.Cos(3.14159265358979 / (double)num);
				double num5 = num2 * num4;
				double num6 = num1 / 2;
				this.cachedPoints.EnsureListCount<Point>(num * 2, null);
				Rect rect = base.LogicalBounds.Resize(num5);
				for (int j = 0; j < num; j++)
				{
					double num7 = num1 * (double)j;
					this.cachedPoints[j * 2] = GeometryHelper.GetArcPoint(num7, base.LogicalBounds);
					this.cachedPoints[j * 2 + 1] = GeometryHelper.GetArcPoint(num7 + num6, rect);
				}
			}
			flag = flag | PathGeometryHelper.SyncPolylineGeometry(ref this.cachedGeometry, this.cachedPoints, true);
			return flag;
		}
	}
}