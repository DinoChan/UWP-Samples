using Microsoft.Expression.Drawing.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Media
{
	public sealed class SketchGeometryEffect : GeometryEffect
	{
		private const double expectedLengthMean = 8;

		private const double normalDisturbVariance = 0.5;

		private const double tangentDisturbVariance = 1;

		private const double bsplineWeight = 0.05;

		private readonly long randomSeed = DateTime.Now.Ticks;

		public SketchGeometryEffect()
		{
		}

		protected override GeometryEffect DeepCopy()
		{
			return new SketchGeometryEffect();
		}

		private static void DisturbPoints(RandomEngine random, double scale, IList<Point> points, IList<Vector> normals)
		{
			int count = points.Count;
			for (int i = 1; i < count; i++)
			{
				double num = random.NextGaussian(0, 1 * scale);
				double num1 = random.NextUniform(-0.5, 0.5) * scale;
				double x = points[i].X;
				Vector item = normals[i];
				Vector vector = normals[i];
				double y = points[i].Y;
				Vector item1 = normals[i];
				Vector vector1 = normals[i];
				points[i] = new Point(x + item.X * num1 - vector.Y * num, y + item1.X * num + vector1.Y * num1);
			}
		}

		public override bool Equals(GeometryEffect geometryEffect)
		{
			return geometryEffect is SketchGeometryEffect;
		}

		private IEnumerable<SimpleSegment> GetEffectiveSegments(PathFigure pathFigure)
		{
			Point startPoint = pathFigure.StartPoint;
			foreach (PathSegmentData pathSegmentDatum in pathFigure.AllSegments())
			{
				foreach (SimpleSegment simpleSegment in pathSegmentDatum.PathSegment.GetSimpleSegments(pathSegmentDatum.StartPoint))
				{
					yield return simpleSegment;
					startPoint = simpleSegment.Points.Last<Point>();
				}
			}
			if (pathFigure.IsClosed)
			{
				yield return SimpleSegment.Create(startPoint, pathFigure.StartPoint);
			}
		}

		protected override bool UpdateCachedGeometry(Geometry input)
		{
			bool flag = false;
			PathGeometry pathGeometry = input.AsPathGeometry();
			if (pathGeometry == null)
			{
				this.cachedGeometry = input;
			}
			else
			{
				flag = flag | this.UpdateSketchGeometry(pathGeometry);
			}
			return flag;
		}

		private bool UpdateSketchGeometry(PathGeometry inputPath)
		{
			PathGeometry pathGeometry;
			bool flag = false;
			flag = flag | GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
			flag = flag | pathGeometry.Figures.EnsureListCount<PathFigure>(inputPath.Figures.Count, () => new PathFigure());
			RandomEngine randomEngine = new RandomEngine(this.randomSeed);
			for (int i = 0; i < inputPath.Figures.Count; i++)
			{
				PathFigure item = inputPath.Figures[i];
				bool isClosed = item.IsClosed;
				bool isFilled = item.IsFilled;
				if (item.Segments.Count != 0)
				{
					List<Point> points = new List<Point>(item.Segments.Count * 3);
					foreach (SimpleSegment effectiveSegment in this.GetEffectiveSegments(item))
					{
						List<Point> points1 = new List<Point>()
						{
							effectiveSegment.Points[0]
						};
						effectiveSegment.Flatten(points1, 0, null);
						PolylineData polylineDatum = new PolylineData(points1);
						if (points1.Count <= 1 || polylineDatum.TotalLength <= 4)
						{
							points.AddRange(points1);
							points.RemoveLast<Point>();
						}
						else
						{
							double totalLength = polylineDatum.TotalLength / 8;
							int num2 = (int)Math.Max(2, Math.Ceiling(totalLength));
							double totalLength1 = polylineDatum.TotalLength / (double)num2;
							double num3 = totalLength1 / 8;
							List<Point> points2 = new List<Point>(num2);
							List<Vector> vectors = new List<Vector>(num2);
							int num4 = 0;
							PolylineHelper.PathMarch(polylineDatum, 0, 0, (MarchLocation location) => {
								if (location.Reason == MarchStopReason.CompletePolyline)
								{
									return double.NaN;
								}
								if (location.Reason != MarchStopReason.CompleteStep)
								{
									return location.Remain;
								}
								int num = num4;
								int num1 = num;
								num4 = num + 1;
								if (num1 == num2)
								{
									return double.NaN;
								}
								points2.Add(location.GetPoint(polylineDatum.Points));
								vectors.Add(location.GetNormal(polylineDatum, 0));
								return totalLength1;
							});
							SketchGeometryEffect.DisturbPoints(randomEngine, num3, points2, vectors);
							points.AddRange(points2);
						}
					}
					if (!isClosed)
					{
						points.Add(item.Segments.Last<PathSegment>().GetLastPoint());
					}
					flag = flag | PathFigureHelper.SyncPolylineFigure(pathGeometry.Figures[i], points, isClosed, isFilled);
				}
				else
				{
					flag = flag | pathGeometry.Figures[i].SetIfDifferent(PathFigure.StartPointProperty, item.StartPoint);
					flag = flag | pathGeometry.Figures[i].Segments.EnsureListCount<PathSegment>(0, null);
				}
			}
			if (flag)
			{
				this.cachedGeometry = PathGeometryHelper.FixPathGeometryBoundary(this.cachedGeometry);
			}
			return flag;
		}
	}
}