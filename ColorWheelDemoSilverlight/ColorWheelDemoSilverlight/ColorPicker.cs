﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace ColorWheelDemoSilverlight
{
    [ContentProperty(nameof(ColorPoints))]
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
            ColorPointVisualDragStartedCommand = new DelegateCommand(ColorPointVisualDragStarted);
            ColorPointVisualDragDeltaCommand = new DelegateCommand(ColorPointVisualDragDelta);
        }

        public ICommand ColorPointVisualDragStartedCommand { get; private set; }

        public ICommand ColorPointVisualDragDeltaCommand { get; private set; }

        /// <summary>
        ///     获取或设置ColorPoints的值
        /// </summary>
        public Collection<ColorPoint> ColorPoints
        {
            get { return (Collection<ColorPoint>)GetValue(ColorPointsProperty); }
            set { SetValue(ColorPointsProperty, value); }
        }

        private static void OnColorPointsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as ColorPicker;
            var oldValue = (Collection<ColorPoint>)args.OldValue;
            var newValue = (Collection<ColorPoint>)args.NewValue;
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

        private void ColorPointVisualDragStarted(object param)
        {
            var parameter = param as ColorPointVisualDragStartedParameter;
            if (parameter != null)
                OnColorPointVisualDragStarted(parameter.ColorPointVisual, parameter.Position);
        }

        private void ColorPointVisualDragDelta(object param)
        {
            var parameter = param as ColorPointVisualDragDeltaParameter;
            if (parameter != null)
                OnColorPointVisualDragDelta(parameter.ColorPointVisual, parameter.Translation);
        }

        protected virtual void OnColorPointVisualDragStarted(ColorPointVisual colorPointVisual, Point position)
        {

        }

        protected virtual void OnColorPointVisualDragDelta(ColorPointVisual colorPointVisual, Point position)
        {

        }
    }
}