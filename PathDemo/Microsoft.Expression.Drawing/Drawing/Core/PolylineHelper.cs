using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Expression.Drawing.Core
{
	internal static class PolylineHelper
	{
		public static IEnumerable<PolylineData> GetWrappedPolylines(IList<PolylineData> lines, ref double startArcLength)
		{
			int num = 0;
			for (int i = 0; i < lines.Count; i++)
			{
				num = i;
				startArcLength = startArcLength - lines[i].TotalLength;
				if (MathHelper.LessThanOrClose(startArcLength, 0))
				{
					break;
				}
			}
			if (!MathHelper.LessThanOrClose(startArcLength, 0))
			{
				throw new ArgumentOutOfRangeException("startArcLength");
			}
			startArcLength = startArcLength + lines[num].TotalLength;
			return lines.Skip<PolylineData>(num).Concat<PolylineData>(lines.Take<PolylineData>(num + 1));
		}

		public static void PathMarch(PolylineData polyline, double startArcLength, double cornerThreshold, Func<MarchLocation, double> stopCallback)
		{
			if (polyline == null)
			{
				throw new ArgumentNullException("polyline");
			}
			int count = polyline.Count;
			if (count <= 1)
			{
				throw new ArgumentOutOfRangeException("polyline");
			}
			bool flag = false;
			double num = startArcLength;
			double item = 0;
			int num1 = 0;
			double num2 = Math.Cos(cornerThreshold * 3.14159265358979 / 180);
			while (true)
			{
				double item1 = polyline.Lengths[num1];
				if (!MathHelper.IsFiniteDouble(num))
				{
					break;
				}
				if (MathHelper.IsVerySmall(num))
				{
					num = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, num1, item, item1 - item, num));
					flag = true;
				}
				else if (!MathHelper.GreaterThan(num, 0))
				{
					if (MathHelper.LessThan(num, 0))
					{
						if (MathHelper.GreaterThanOrClose(num + item, 0))
						{
							item = item + num;
							num = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, num1, item, item1 - item, 0));
							flag = true;
						}
						else if (num1 <= 0)
						{
							num = num + item;
							item1 = polyline.Lengths[num1];
							item = 0;
							num = stopCallback(MarchLocation.Create(MarchStopReason.CompletePolyline, num1, item, item1 - item, num));
							flag = true;
						}
						else
						{
							num1--;
							num = num + item;
							item = polyline.Lengths[num1];
							if (flag && num2 != 1 && polyline.Angles[num1 + 1] > num2)
							{
								item1 = polyline.Lengths[num1];
								num = stopCallback(MarchLocation.Create(MarchStopReason.CornerPoint, num1, item, item1 - item, num));
							}
						}
					}
				}
				else if (MathHelper.LessThanOrClose(num + item, item1))
				{
					item = item + num;
					num = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, num1, item, item1 - item, 0));
					flag = true;
				}
				else if (num1 >= count - 2)
				{
					num = num - (item1 - item);
					item1 = polyline.Lengths[num1];
					item = polyline.Lengths[num1];
					num = stopCallback(MarchLocation.Create(MarchStopReason.CompletePolyline, num1, item, item1 - item, num));
					flag = true;
				}
				else
				{
					num1++;
					num = num - (item1 - item);
					item = 0;
					if (flag && num2 != 1 && polyline.Angles[num1] > num2)
					{
						item1 = polyline.Lengths[num1];
						num = stopCallback(MarchLocation.Create(MarchStopReason.CornerPoint, num1, item, item1 - item, num));
					}
				}
			}
		}
	}
}