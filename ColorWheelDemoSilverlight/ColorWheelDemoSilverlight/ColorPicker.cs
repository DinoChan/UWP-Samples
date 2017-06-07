using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace ColorWheelDemoSilverlight
{
    public class ColorPicker : Control
    {
        /// <summary>
        ///     标识 ColorPoints 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ColorPointsProperty =
            DependencyProperty.Register("ColorPoints", typeof(Collection<ColorPoint>), typeof(ColorPicker), new PropertyMetadata(null, OnColorPointsChanged));


        public ColorPicker()
        {
            ColorPoints = new ObservableCollection<ColorPoint>();
        }

        /// <summary>
        ///     获取或设置ColorPoints的值
        /// </summary>
        public Collection<ColorPoint> ColorPoints
        {
            get { return (Collection<ColorPoint>) GetValue(ColorPointsProperty); }
            set { SetValue(ColorPointsProperty, value); }
        }

        private static void OnColorPointsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as ColorPicker;
            var oldValue = (Collection<ColorPoint>) args.OldValue;
            var newValue = (Collection<ColorPoint>) args.NewValue;
            if (oldValue != newValue)
                target.OnColorPointsChanged(oldValue, newValue);
        }


        protected virtual void OnColorPointsChanged(Collection<ColorPoint> oldValue, Collection<ColorPoint> newValue)
        {
            var notifyCollectionChanged = oldValue as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
                notifyCollectionChanged.CollectionChanged -= OnColorPointsCollectionChanged;

            notifyCollectionChanged = newValue as INotifyCollectionChanged;

            if (notifyCollectionChanged != null)
                notifyCollectionChanged.CollectionChanged += OnColorPointsCollectionChanged;
        }

        protected virtual void OnColorPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           
        }

        protected virtual ColorPointVisual CreateColorPointVisual(ColorPoint colorPoint)
        {
            return new ColorPointVisual();
        }
    }
}