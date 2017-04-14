using Microsoft.Expression.Media;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Expression.Controls
{
	public sealed class Callout : CompositeContentShape, ICalloutGeometrySourceParameters, IGeometrySourceParameters
	{
		public readonly static DependencyProperty AnchorPointProperty;

		public readonly static DependencyProperty CalloutStyleProperty;

		public Point AnchorPoint
		{
			get
			{
				return JustDecompileGenerated_get_AnchorPoint();
			}
			set
			{
				JustDecompileGenerated_set_AnchorPoint(value);
			}
		}

		public Point JustDecompileGenerated_get_AnchorPoint()
		{
			return (Point)base.GetValue(Callout.AnchorPointProperty);
		}

		public void JustDecompileGenerated_set_AnchorPoint(Point value)
		{
			base.SetValue(Callout.AnchorPointProperty, value);
		}

		public Microsoft.Expression.Media.CalloutStyle CalloutStyle
		{
			get
			{
				return JustDecompileGenerated_get_CalloutStyle();
			}
			set
			{
				JustDecompileGenerated_set_CalloutStyle(value);
			}
		}

		public Microsoft.Expression.Media.CalloutStyle JustDecompileGenerated_get_CalloutStyle()
		{
			return (Microsoft.Expression.Media.CalloutStyle)base.GetValue(Callout.CalloutStyleProperty);
		}

		public void JustDecompileGenerated_set_CalloutStyle(Microsoft.Expression.Media.CalloutStyle value)
		{
			base.SetValue(Callout.CalloutStyleProperty, value);
		}

		static Callout()
		{
			Type type = typeof(Point);
			Type type1 = typeof(Callout);
			Point point = new Point();
			Callout.AnchorPointProperty = DependencyProperty.Register("AnchorPoint", type, type1, new DrawingPropertyMetadata((object)point, DrawingPropertyMetadataOptions.AffectsRender));
			Callout.CalloutStyleProperty = DependencyProperty.Register("CalloutStyle", typeof(Microsoft.Expression.Media.CalloutStyle), typeof(Callout), new DrawingPropertyMetadata((object)Microsoft.Expression.Media.CalloutStyle.RoundedRectangle, DrawingPropertyMetadataOptions.AffectsRender));
		}

		public Callout()
		{
			base.DefaultStyleKey = typeof(Callout);
		}

		protected override IGeometrySource CreateGeometrySource()
		{
			return new CalloutGeometrySource();
		}
	}
}