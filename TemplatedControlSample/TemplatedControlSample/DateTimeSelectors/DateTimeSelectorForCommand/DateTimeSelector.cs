using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TemplatedControlSample.DateTimeSelectors.DateTimeSelectorForCommand
{
    public class DateTimeSelector : Control
    {
        /// <summary>
        /// 标识 Command 依赖属性。
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DateTimeSelector), new PropertyMetadata(null, OnCommandChanged));

        private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DateTimeSelector target = obj as DateTimeSelector;
            ICommand oldValue = (ICommand)args.OldValue;
            ICommand newValue = (ICommand)args.NewValue;
            if (oldValue != newValue)
                target.OnCommandChanged(oldValue, newValue);
        }

        /// <summary>
        /// 标识 CommandParameter 依赖属性。
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DateTimeSelector), new PropertyMetadata(null, OnCommandParameterChanged));

        private static void OnCommandParameterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DateTimeSelector target = obj as DateTimeSelector;
            object oldValue = (object)args.OldValue;
            object newValue = (object)args.NewValue;
            if (oldValue != newValue)
                target.OnCommandParameterChanged(oldValue, newValue);
        }

        public DateTimeSelector()
        {
            this.DefaultStyleKey = typeof(DateTimeSelector);
        }

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


        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            UpdateIsEnabled();
        }

        private void UpdateIsEnabled()
        {
            IsEnabled = (null == Command) || Command.CanExecute(CommandParameter);
            UpdateVisualState(true);
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
    }
}
