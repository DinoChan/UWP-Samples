using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace Microsoft.Expression.Media
{
	internal class DrawingPropertyMetadata : PropertyMetadata
	{
		private DrawingPropertyMetadataOptions options;

		private System.Windows.PropertyChangedCallback propertyChangedCallback;

		public bool AffectsMeasure
		{
			get
			{
				return (this.options & DrawingPropertyMetadataOptions.AffectsMeasure) != DrawingPropertyMetadataOptions.None;
			}
		}

		public bool AffectsRender
		{
			get
			{
				return (this.options & DrawingPropertyMetadataOptions.AffectsRender) != DrawingPropertyMetadataOptions.None;
			}
		}

		static DrawingPropertyMetadata()
		{
			DrawingPropertyMetadata.DrawingPropertyChanged += new EventHandler<DrawingPropertyChangedEventArgs>((object sender, DrawingPropertyChangedEventArgs args) => {
				IShape shape = sender as IShape;
				if (shape != null && args.Metadata.AffectsRender)
				{
					InvalidateGeometryReasons invalidateGeometryReason = InvalidateGeometryReasons.PropertyChanged;
					if (args.IsAnimated)
					{
						invalidateGeometryReason = invalidateGeometryReason | InvalidateGeometryReasons.IsAnimated;
					}
					shape.InvalidateGeometry(invalidateGeometryReason);
				}
			});
		}

		public DrawingPropertyMetadata(object defaultValue) : this(defaultValue, DrawingPropertyMetadataOptions.None, null)
		{
		}

		public DrawingPropertyMetadata(System.Windows.PropertyChangedCallback propertyChangedCallback) : this(DependencyProperty.UnsetValue, DrawingPropertyMetadataOptions.None, propertyChangedCallback)
		{
		}

		public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options) : this(defaultValue, options, null)
		{
		}

		public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options, System.Windows.PropertyChangedCallback propertyChangedCallback) : base(defaultValue, DrawingPropertyMetadata.AttachCallback(defaultValue, options, propertyChangedCallback))
		{
		}

		private DrawingPropertyMetadata(DrawingPropertyMetadataOptions options, object defaultValue) : base(defaultValue)
		{
		}

		private static System.Windows.PropertyChangedCallback AttachCallback(object defaultValue, DrawingPropertyMetadataOptions options, System.Windows.PropertyChangedCallback propertyChangedCallback)
		{
			DrawingPropertyMetadata drawingPropertyMetadatum = new DrawingPropertyMetadata(options, defaultValue)
			{
				options = options,
				propertyChangedCallback = propertyChangedCallback
			};
			return new System.Windows.PropertyChangedCallback(drawingPropertyMetadatum.InternalCallback);
		}

		private void InternalCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (DrawingPropertyMetadata.DrawingPropertyChanged != null)
			{
				EventHandler<DrawingPropertyChangedEventArgs> eventHandler = DrawingPropertyMetadata.DrawingPropertyChanged;
				DrawingPropertyChangedEventArgs drawingPropertyChangedEventArg = new DrawingPropertyChangedEventArgs()
				{
					Metadata = this,
					IsAnimated = sender.GetAnimationBaseValue(e.Property) != e.NewValue
				};
				eventHandler(sender, drawingPropertyChangedEventArg);
			}
			if (this.propertyChangedCallback != null)
			{
				this.propertyChangedCallback(sender, e);
			}
		}

		public static event EventHandler<DrawingPropertyChangedEventArgs> DrawingPropertyChanged;
	}
}