using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TemplatedControlSample
{
public class MenuItem : Control
{
    /// <summary>
    /// 标识 Command 依赖属性。
    /// </summary>
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(MenuItem), new PropertyMetadata(null, OnCommandChanged));

    private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        MenuItem target = obj as MenuItem;
        ICommand oldValue = (ICommand)args.OldValue;
        ICommand newValue = (ICommand)args.NewValue;
        if (oldValue != newValue)
            target.OnCommandChanged(oldValue, newValue);
    }

    /// <summary>
    /// 标识 CommandParameter 依赖属性。
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register("CommandParameter", typeof(object), typeof(MenuItem), new PropertyMetadata(null, OnCommandParameterChanged));

    private static void OnCommandParameterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        MenuItem target = obj as MenuItem;
        object oldValue = (object)args.OldValue;
        object newValue = (object)args.NewValue;
        if (oldValue != newValue)
            target.OnCommandParameterChanged(oldValue, newValue);
    }

    public MenuItem()
    {
        this.DefaultStyleKey = typeof(MenuItem);
    }


    public event RoutedEventHandler Click;

    /// <summary>
    /// 获取或设置Command的值
    /// </summary>  
    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    /// <summary>
    /// 获取或设置CommandParameter的值
    /// </summary>  
    public object CommandParameter
    {
        get { return (object)GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    protected virtual void OnCommandParameterChanged(object oldValue, object newValue)
    {
        UpdateIsEnabled();
    }

    protected virtual void OnCommandChanged(ICommand oldValue, ICommand newValue)
    {
        if (oldValue != null)
            oldValue.CanExecuteChanged -= OnCanExecuteChanged;

        if (newValue != null)
            newValue.CanExecuteChanged += OnCanExecuteChanged;

        UpdateIsEnabled();
    }

    protected virtual void UpdateVisualState(bool useTransitions)
    {
        if (IsEnabled)
        {
            VisualStateManager.GoToState(this, "Normal", useTransitions);
        }
        else
        {
            VisualStateManager.GoToState(this, "Disabled", useTransitions);
        }
    }

    protected override void OnPointerPressed(PointerRoutedEventArgs e)
    {
        base.OnPointerPressed(e);
        Click?.Invoke(this, new RoutedEventArgs());
        if ((null != Command) && Command.CanExecute(CommandParameter))
        {
            Command.Execute(CommandParameter);
        }
    }

    private void OnCanExecuteChanged(object sender, EventArgs e)
    {
        UpdateIsEnabled();
    }

    private void UpdateIsEnabled()
    {
        IsEnabled = (null == Command) || Command.CanExecute(CommandParameter);
        UpdateVisualState(true);
    }
}
}
