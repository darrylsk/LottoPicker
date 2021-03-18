using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using LottoPicker.Behaviors.Base;
using Xamarin.Forms;

namespace LottoPicker.Behaviors
{
    /// <summary>
    /// There are two types of behaviours, Xamarin.Forms behaviours and attached behaviours.
    /// Xamarin.Forms behaviours are generally the preferred type of behaviours.
    /// This is an example of a Xamarin.Forms behaviour
    /// Xamarin.Forms behaviours are instance classes that inherit from Behaviour or Behaviour<T>
    ///
    /// [DS] This code is pretty hard to understand, so I don't bother trying to understand it.  I just plug it in and use it.
    /// </summary>
    public class EventToCommandBehavior : BindableBehavior<View>
    {
        public static BindableProperty EventNameProperty =
            BindableProperty.CreateAttached("EventName", typeof(string), typeof(EventToCommandBehavior), null,
                BindingMode.OneWay);

        public static BindableProperty CommandProperty =    
            BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(EventToCommandBehavior), null,
                BindingMode.OneWay);

        public static BindableProperty CommandParameterProperty =
            BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null,
                BindingMode.OneWay);

        public static BindableProperty EventArgsConverterProperty =
            BindableProperty.CreateAttached("EventArgsConverter", typeof(IValueConverter), typeof(EventToCommandBehavior), null,
                BindingMode.OneWay);

        public static BindableProperty EventArgsConverterParameterProperty =
            BindableProperty.CreateAttached("EventArgsConverterParameter", typeof(object), typeof(EventToCommandBehavior), null,
                BindingMode.OneWay);

        protected Delegate _handler;
        private EventInfo _eventInfo;

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter EventArgsConverter
        {
            get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
            set { SetValue(EventArgsConverterProperty, value); }
        }

        public object EventArgsConverterParameter
        {
            get { return GetValue(EventArgsConverterParameterProperty); }
            set { SetValue(EventArgsConverterParameterProperty, value); }
        }

        protected override void OnAttachedTo(View visualElement)
        {
            base.OnAttachedTo(visualElement);

            var events = AssociatedObject.GetType().GetRuntimeEvents().ToArray();
            if (events.Any())
            {
                _eventInfo = events.FirstOrDefault(e => e.Name == EventName);
                if (_eventInfo == null)
                    throw new ArgumentException(String.Format("EventToCommand: Can't find any event named '{0}' on attached type", EventName));

                AddEventHandler(_eventInfo, AssociatedObject, OnFired);
            }
        }

        protected override void OnDetachingFrom(View view)
        {
            if (_handler != null)
                _eventInfo.RemoveEventHandler(AssociatedObject, _handler);

            base.OnDetachingFrom(view);
        }

        private void AddEventHandler(EventInfo eventInfo, object item, Action<object, EventArgs> action)
        {
            var eventParameters = eventInfo.EventHandlerType
                .GetRuntimeMethods().First(m => m.Name == "Invoke")
                .GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType))
                .ToArray();

            var actionInvoke = action.GetType()
                .GetRuntimeMethods().First(m => m.Name == "Invoke");

            _handler = Expression.Lambda(
                eventInfo.EventHandlerType,
                Expression.Call(Expression.Constant(action), actionInvoke, eventParameters[0], eventParameters[1]),
                eventParameters
            )
            .Compile();

            eventInfo.AddEventHandler(item, _handler);
        }

        private void OnFired(object sender, EventArgs eventArgs)
        {
            if (Command == null)
                return;

            //var parameter = CommandParameter;
            var picker = sender as Picker;
            var parameter = picker.SelectedItem;

            if (eventArgs != null && eventArgs != EventArgs.Empty)
            {
                parameter = eventArgs;

                if (EventArgsConverter != null)
                {
                    parameter = EventArgsConverter.Convert(eventArgs, typeof(object), EventArgsConverterParameter, CultureInfo.CurrentUICulture);
                }
            }

            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }
    }
}
