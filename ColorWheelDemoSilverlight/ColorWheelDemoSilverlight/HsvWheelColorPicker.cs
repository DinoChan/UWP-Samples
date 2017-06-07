using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ColorWheelDemoSilverlight
{
    [TemplatePart(Name =ItemsControlElementName,Type =typeof(ItemsControl))]
    public class HsvWheelColorPicker : ColorPicker
    {
        private const string ItemsControlElementName = "ItemsControlElement";

        public HsvWheelColorPicker()
        {
            this.DefaultStyleKey = typeof(HsvWheelColorPicker);
            _colorPointVisuals = new ObservableCollection<ColorPointVisual>();
        }


        private ObservableCollection<ColorPointVisual> _colorPointVisuals;

        private ItemsControl _itemsControl;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsControl = GetTemplateChild(ItemsControlElementName) as ItemsControl;
            if (_itemsControl != null)
                _itemsControl.ItemsSource = _colorPointVisuals;
        }

        protected override void OnColorPointsChanged(Collection<ColorPoint> oldValue, Collection<ColorPoint> newValue)
        {
            base.OnColorPointsChanged(oldValue, newValue);

            _colorPointVisuals.Clear();
            if (newValue == null)
                return;

            foreach (var item in newValue)
            {
                _colorPointVisuals.Add(CreateColorPointVisual(item));
            }
        }

        protected override void OnColorPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnColorPointsCollectionChanged(sender, e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.Cast<ColorPoint>())
                    {
                        _colorPointVisuals.Add(CreateColorPointVisual(item));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.NewItems.Cast<ColorPoint>())
                    {
                       var colorPointVisual= _colorPointVisuals.FirstOrDefault(v => v.ColorPoint == item);
                        if (colorPointVisual != null)
                            _colorPointVisuals.Remove(colorPointVisual);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException("Do not support Replace action");
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _colorPointVisuals.Clear();
                    break;
                default:
                    break;
            }
        }

        protected override ColorPointVisual CreateColorPointVisual(ColorPoint colorPoint)
        {
            var visual = new ColorPointVisual();
            visual.ColorPoint = colorPoint;
            return visual;
        }
    }
}
