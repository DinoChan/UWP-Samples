using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace TemplatedControlSample
{
[TemplatePart(Name = ItemsPanelPartName, Type = typeof(Panel))]
[ContentProperty(Name = "Items")]
public class SimpleItemsControl : Control
{
    private const string ItemsPanelPartName = "ItemsPanel";
    public SimpleItemsControl()
    {
        this.DefaultStyleKey = typeof(SimpleItemsControl);
        var items = new ObservableCollection<object>();
        items.CollectionChanged += OnItemsCollectionChanged;
        Items = items;
    }

    /// <summary>
    /// 获取或设置ItemTemplate的值
    /// </summary>  
    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    /// <summary>
    /// 标识 ItemTemplate 依赖属性。
    /// </summary>
    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(SimpleItemsControl), new PropertyMetadata(null, OnItemTemplateChanged));

    private static void OnItemTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        SimpleItemsControl target = obj as SimpleItemsControl;
        DataTemplate oldValue = (DataTemplate)args.OldValue;
        DataTemplate newValue = (DataTemplate)args.NewValue;
        if (oldValue != newValue)
            target.OnItemTemplateChanged(oldValue, newValue);
    }

    protected virtual void OnItemTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
    {
        UpdateView();
    }

    public ICollection<object> Items
    {
        get;
    }

    private Panel _itemsPanel;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _itemsPanel = GetTemplateChild(ItemsPanelPartName) as Panel;
        UpdateView();
    }

    private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateView();
    }


    //
    // 摘要:
    //     创建或标识用于显示给定项的元素。
    //
    // 返回结果:
    //     用于显示给定项的元素。
    protected virtual DependencyObject GetContainerForItemOverride()
    {
        return new ContentPresenter();
    }


    //
    // 摘要:
    //     确定指定项是否是为自身的容器，或是否可以作为其自身的容器。
    //
    // 参数:
    //   item:
    //     要检查的项。
    //
    // 返回结果:
    //     如果项是其自己的容器（或可以作为自己的容器），则为 true；否则为 false。
    protected virtual System.Boolean IsItemItsOwnContainerOverride(System.Object item)
    {
        return item is ContentPresenter;
    }

    //
    // 摘要:
    //     准备指定元素以显示指定项。
    //
    // 参数:
    //   element:
    //     用于显示指定项的元素。
    //
    //   item:
    //     要显示的项。
    protected virtual void PrepareContainerForItemOverride(DependencyObject element, System.Object item)
    {
        ContentControl contentControl;
        ContentPresenter contentPresenter;

        if ((contentControl = element as ContentControl) != null)
        {
            contentControl.Content = item;
            contentControl.ContentTemplate = ItemTemplate;
        }
        else if ((contentPresenter = element as ContentPresenter) != null)
        {
            contentPresenter.Content = item;
            contentPresenter.ContentTemplate = ItemTemplate;
        }
    }

    private void UpdateView()
    {
        if (_itemsPanel == null)
            return;

        _itemsPanel.Children.Clear();
        foreach (var item in Items)
        {
            DependencyObject container;
            if (IsItemItsOwnContainerOverride(item))
            {
                container = item as DependencyObject;
            }
            else
            {
                container = GetContainerForItemOverride();
                PrepareContainerForItemOverride(container, item);
            }
               
            if (container is UIElement)
                _itemsPanel.Children.Add(container as UIElement);
        }
    }
}
}
