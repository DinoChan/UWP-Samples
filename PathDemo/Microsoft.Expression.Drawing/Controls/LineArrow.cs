using Microsoft.Expression.Media;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Expression.Controls
{
	public sealed class LineArrow : CompositeShape, ILineArrowGeometrySourceParameters, IGeometrySourceParameters
	{
		public readonly static DependencyProperty BendAmountProperty;

		public readonly static DependencyProperty StartArrowProperty;

		public readonly static DependencyProperty EndArrowProperty;

		public readonly static DependencyProperty StartCornerProperty;

		public readonly static DependencyProperty ArrowSizeProperty;

		public double ArrowSize
		{
			get
			{
				return JustDecompileGenerated_get_ArrowSize();
			}
			set
			{
				JustDecompileGenerated_set_ArrowSize(value);
			}
		}

		public double JustDecompileGenerated_get_ArrowSize()
		{
			return (double)base.GetValue(LineArrow.ArrowSizeProperty);
		}

		public void JustDecompileGenerated_set_ArrowSize(double value)
		{
			base.SetValue(LineArrow.ArrowSizeProperty, value);
		}

		public double BendAmount
		{
			get
			{
				return JustDecompileGenerated_get_BendAmount();
			}
			set
			{
				JustDecompileGenerated_set_BendAmount(value);
			}
		}

		public double JustDecompileGenerated_get_BendAmount()
		{
			return (double)base.GetValue(LineArrow.BendAmountProperty);
		}

		public void JustDecompileGenerated_set_BendAmount(double value)
		{
			base.SetValue(LineArrow.BendAmountProperty, value);
		}

		public ArrowType EndArrow
		{
			get
			{
				return JustDecompileGenerated_get_EndArrow();
			}
			set
			{
				JustDecompileGenerated_set_EndArrow(value);
			}
		}

		public ArrowType JustDecompileGenerated_get_EndArrow()
		{
			return (ArrowType)base.GetValue(LineArrow.EndArrowProperty);
		}

		public void JustDecompileGenerated_set_EndArrow(ArrowType value)
		{
			base.SetValue(LineArrow.EndArrowProperty, value);
		}

		public ArrowType StartArrow
		{
			get
			{
				return JustDecompileGenerated_get_StartArrow();
			}
			set
			{
				JustDecompileGenerated_set_StartArrow(value);
			}
		}

		public ArrowType JustDecompileGenerated_get_StartArrow()
		{
			return (ArrowType)base.GetValue(LineArrow.StartArrowProperty);
		}

		public void JustDecompileGenerated_set_StartArrow(ArrowType value)
		{
			base.SetValue(LineArrow.StartArrowProperty, value);
		}

		public CornerType StartCorner
		{
			get
			{
				return JustDecompileGenerated_get_StartCorner();
			}
			set
			{
				JustDecompileGenerated_set_StartCorner(value);
			}
		}

		public CornerType JustDecompileGenerated_get_StartCorner()
		{
			return (CornerType)base.GetValue(LineArrow.StartCornerProperty);
		}

		public void JustDecompileGenerated_set_StartCorner(CornerType value)
		{
			base.SetValue(LineArrow.StartCornerProperty, value);
		}

		static LineArrow()
		{
			LineArrow.BendAmountProperty = DependencyProperty.Register("BendAmount", typeof(double), typeof(LineArrow), new DrawingPropertyMetadata((object)0.5, DrawingPropertyMetadataOptions.AffectsRender));
			LineArrow.StartArrowProperty = DependencyProperty.Register("StartArrow", typeof(ArrowType), typeof(LineArrow), new DrawingPropertyMetadata((object)ArrowType.NoArrow, DrawingPropertyMetadataOptions.AffectsRender));
			LineArrow.EndArrowProperty = DependencyProperty.Register("EndArrow", typeof(ArrowType), typeof(LineArrow), new DrawingPropertyMetadata((object)ArrowType.Arrow, DrawingPropertyMetadataOptions.AffectsRender));
			LineArrow.StartCornerProperty = DependencyProperty.Register("StartCorner", typeof(CornerType), typeof(LineArrow), new DrawingPropertyMetadata((object)CornerType.TopLeft, DrawingPropertyMetadataOptions.AffectsRender));
			LineArrow.ArrowSizeProperty = DependencyProperty.Register("ArrowSize", typeof(double), typeof(LineArrow), new DrawingPropertyMetadata((object)10, DrawingPropertyMetadataOptions.AffectsRender));
		}

		public LineArrow()
		{
			base.DefaultStyleKey = typeof(LineArrow);
		}

		protected override IGeometrySource CreateGeometrySource()
		{
			return new LineArrowGeometrySource();
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			return base.MeasureOverride(new Size(0, 0));
		}
	}
}