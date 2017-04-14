using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Expression.Drawing.Core
{
	internal static class TransformHelper
	{
		private static Transform identityTransform;

		private readonly static Point[] identityTestPoints;

		public static Transform IdentityTransform
		{
			get
			{
				return TransformHelper.identityTransform;
			}
		}

		static TransformHelper()
		{
			TransformHelper.identityTransform = new MatrixTransform();
			Point[] point = new Point[] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) };
			TransformHelper.identityTestPoints = point;
		}

		public static Transform CloneTransform(this Transform transform)
		{
			if (transform == null)
			{
				return null;
			}
			TranslateTransform translateTransform = transform as TranslateTransform;
			if (translateTransform != null)
			{
				TranslateTransform translateTransform1 = new TranslateTransform()
				{
					X = translateTransform.X,
					Y = translateTransform.Y
				};
				return translateTransform1;
			}
			RotateTransform rotateTransform = transform as RotateTransform;
			if (rotateTransform != null)
			{
				RotateTransform rotateTransform1 = new RotateTransform()
				{
					Angle = rotateTransform.Angle,
					CenterX = rotateTransform.CenterX,
					CenterY = rotateTransform.CenterY
				};
				return rotateTransform1;
			}
			ScaleTransform scaleTransform = transform as ScaleTransform;
			if (scaleTransform != null)
			{
				ScaleTransform scaleTransform1 = new ScaleTransform()
				{
					ScaleX = scaleTransform.ScaleX,
					ScaleY = scaleTransform.ScaleY,
					CenterX = scaleTransform.CenterX,
					CenterY = scaleTransform.CenterY
				};
				return scaleTransform1;
			}
			SkewTransform skewTransform = transform as SkewTransform;
			if (skewTransform != null)
			{
				SkewTransform skewTransform1 = new SkewTransform()
				{
					AngleX = skewTransform.AngleX,
					AngleY = skewTransform.AngleY,
					CenterX = skewTransform.CenterX,
					CenterY = skewTransform.CenterY
				};
				return skewTransform1;
			}
			CompositeTransform compositeTransform = transform as CompositeTransform;
			if (compositeTransform != null)
			{
				CompositeTransform compositeTransform1 = new CompositeTransform()
				{
					CenterX = compositeTransform.CenterX,
					CenterY = compositeTransform.CenterY,
					Rotation = compositeTransform.Rotation,
					ScaleX = compositeTransform.ScaleX,
					ScaleY = compositeTransform.ScaleY,
					SkewX = compositeTransform.SkewX,
					SkewY = compositeTransform.SkewY,
					TranslateX = compositeTransform.TranslateX,
					TranslateY = compositeTransform.TranslateY
				};
				return compositeTransform1;
			}
			MatrixTransform matrixTransform = transform as MatrixTransform;
			if (matrixTransform != null)
			{
				return new MatrixTransform()
				{
					Matrix = matrixTransform.Matrix
				};
			}
			TransformGroup transformGroup = transform as TransformGroup;
			if (transformGroup == null)
			{
				return transform.DeepCopy<Transform>();
			}
			TransformGroup transformGroup1 = new TransformGroup();
			foreach (Transform child in transformGroup.Children)
			{
				transformGroup1.Children.Add(child.CloneTransform());
			}
			return transformGroup1;
		}

		private static bool CompositeTransformEquals(CompositeTransform firstTransform, CompositeTransform secondTransform)
		{
			if (firstTransform.CenterX != secondTransform.CenterX || firstTransform.CenterY != secondTransform.CenterY || firstTransform.Rotation != secondTransform.Rotation || firstTransform.ScaleX != secondTransform.ScaleX || firstTransform.ScaleY != secondTransform.ScaleY || firstTransform.SkewX != secondTransform.SkewX || firstTransform.SkewY != secondTransform.SkewY || firstTransform.TranslateX != secondTransform.TranslateX)
			{
				return false;
			}
			return firstTransform.TranslateY == secondTransform.TranslateY;
		}

		public static bool IsIdentity(this GeneralTransform transform)
		{
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			MatrixTransform matrixTransform = transform as MatrixTransform;
			if (matrixTransform != null)
			{
				return matrixTransform.Matrix.IsIdentity;
			}
			TranslateTransform translateTransform = transform as TranslateTransform;
			if (translateTransform != null)
			{
				if (translateTransform.X != 0)
				{
					return false;
				}
				return translateTransform.Y == 0;
			}
			RotateTransform rotateTransform = transform as RotateTransform;
			if (rotateTransform != null)
			{
				return rotateTransform.Angle == 0;
			}
			SkewTransform skewTransform = transform as SkewTransform;
			if (skewTransform != null)
			{
				if (skewTransform.AngleX != 0)
				{
					return false;
				}
				return skewTransform.AngleY == 0;
			}
			ScaleTransform scaleTransform = transform as ScaleTransform;
			if (scaleTransform != null)
			{
				if (scaleTransform.ScaleX != 1)
				{
					return false;
				}
				return scaleTransform.ScaleY == 1;
			}
			CompositeTransform compositeTransform = transform as CompositeTransform;
			if (compositeTransform == null)
			{
				TransformGroup transformGroup = transform as TransformGroup;
				if (transformGroup != null)
				{
					return transformGroup.Value.IsIdentity;
				}
				return TransformHelper.identityTestPoints.All<Point>((Point p) => p == transform.Transform(p));
			}
			if (compositeTransform.Rotation != 0 || compositeTransform.TranslateX != 0 || compositeTransform.TranslateY != 0 || compositeTransform.ScaleX != 1 || compositeTransform.ScaleY != 1 || compositeTransform.SkewX != 0)
			{
				return false;
			}
			return compositeTransform.SkewY == 0;
		}

		private static bool MatrixTransformEquals(MatrixTransform firstTransform, MatrixTransform secondTransform)
		{
			return firstTransform.Matrix == secondTransform.Matrix;
		}

		private static bool RotateTransformEquals(RotateTransform firstTransform, RotateTransform secondTransform)
		{
			if (firstTransform.Angle != secondTransform.Angle || firstTransform.CenterX != secondTransform.CenterX)
			{
				return false;
			}
			return firstTransform.CenterY == secondTransform.CenterY;
		}

		private static bool ScaleTransformEquals(ScaleTransform firstTransform, ScaleTransform secondTransform)
		{
			if (firstTransform.ScaleX != secondTransform.ScaleX || firstTransform.ScaleY != secondTransform.ScaleY || firstTransform.CenterX != secondTransform.CenterX)
			{
				return false;
			}
			return firstTransform.CenterY == secondTransform.CenterY;
		}

		private static bool SkewTransformEquals(SkewTransform firstTransform, SkewTransform secondTransform)
		{
			if (firstTransform.AngleX != secondTransform.AngleX || firstTransform.AngleY != secondTransform.AngleY || firstTransform.CenterX != secondTransform.CenterX)
			{
				return false;
			}
			return firstTransform.CenterY == secondTransform.CenterY;
		}

		public static bool TransformEquals(this Transform firstTransform, Transform secondTransform)
		{
			if (firstTransform == null && secondTransform == null)
			{
				return true;
			}
			if (firstTransform == null || secondTransform == null)
			{
				return false;
			}
			if (firstTransform == secondTransform)
			{
				return true;
			}
			TranslateTransform translateTransform = firstTransform as TranslateTransform;
			TranslateTransform translateTransform1 = secondTransform as TranslateTransform;
			if (translateTransform != null && translateTransform1 != null)
			{
				return TransformHelper.TranslateTransformEquals(translateTransform, translateTransform1);
			}
			RotateTransform rotateTransform = firstTransform as RotateTransform;
			RotateTransform rotateTransform1 = secondTransform as RotateTransform;
			if (rotateTransform != null && rotateTransform1 != null)
			{
				return TransformHelper.RotateTransformEquals(rotateTransform, rotateTransform1);
			}
			ScaleTransform scaleTransform = firstTransform as ScaleTransform;
			ScaleTransform scaleTransform1 = secondTransform as ScaleTransform;
			if (scaleTransform != null && scaleTransform1 != null)
			{
				return TransformHelper.ScaleTransformEquals(scaleTransform, scaleTransform1);
			}
			SkewTransform skewTransform = firstTransform as SkewTransform;
			SkewTransform skewTransform1 = secondTransform as SkewTransform;
			if (skewTransform != null && skewTransform1 != null)
			{
				return TransformHelper.SkewTransformEquals(skewTransform, skewTransform1);
			}
			MatrixTransform matrixTransform = firstTransform as MatrixTransform;
			MatrixTransform matrixTransform1 = secondTransform as MatrixTransform;
			if (matrixTransform != null && matrixTransform1 != null)
			{
				return TransformHelper.MatrixTransformEquals(matrixTransform, matrixTransform1);
			}
			TransformGroup transformGroup = firstTransform as TransformGroup;
			TransformGroup transformGroup1 = secondTransform as TransformGroup;
			if (transformGroup != null && transformGroup1 != null)
			{
				return TransformHelper.TransformGroupEquals(transformGroup, transformGroup1);
			}
			CompositeTransform compositeTransform = firstTransform as CompositeTransform;
			CompositeTransform compositeTransform1 = secondTransform as CompositeTransform;
			if (compositeTransform != null && compositeTransform1 != null)
			{
				return TransformHelper.CompositeTransformEquals(compositeTransform, compositeTransform1);
			}
			TransformGroup transformGroup2 = new TransformGroup();
			transformGroup2.Children.Add(firstTransform);
			TransformGroup transformGroup3 = new TransformGroup();
			transformGroup3.Children.Add(secondTransform);
			return transformGroup2.Value == transformGroup3.Value;
		}

		private static bool TransformGroupEquals(TransformGroup firstTransform, TransformGroup secondTransform)
		{
			if (firstTransform.Children.Count != secondTransform.Children.Count)
			{
				return false;
			}
			for (int i = 0; i < firstTransform.Children.Count; i++)
			{
				if (!firstTransform.Children[i].TransformEquals(secondTransform.Children[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static Point TransformPoint(this IEnumerable<GeneralTransform> transforms, Point point)
		{
			if (transforms == null)
			{
				return point;
			}
			foreach (GeneralTransform transform in transforms)
			{
				point = GeometryHelper.SafeTransform(transform, point);
			}
			return point;
		}

		private static bool TranslateTransformEquals(TranslateTransform firstTransform, TranslateTransform secondTransform)
		{
			if (firstTransform.X != secondTransform.X)
			{
				return false;
			}
			return firstTransform.Y == secondTransform.Y;
		}
	}
}