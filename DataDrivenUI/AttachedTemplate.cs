using System;
using System.Linq;
using System.Windows;

namespace DataDrivenUI
{
    public class AttachedTemplate
    {
        private static readonly IStrategySelector StrategySelector;

        static AttachedTemplate()
        {
            //todo: change for IoC resolution here
            StrategySelector = new DefaultStrategySelector();
        }

        public static DependencyProperty DynamicTemplateProperty = 
            DependencyProperty.RegisterAttached("DynamicTemplate", typeof (TemplateOptions), typeof (AttachedTemplate), 
            new FrameworkPropertyMetadata(TemplateOptions.None, OnPropertyChanged));

        static void OnPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var options = (TemplateOptions) args.NewValue;
            var element = source as UIElement;
            if (element == null)
            {
                throw new ArgumentException("DynamicTemplate attached property is only allowed to be used on UIElement subclasses");
            }

            var templateContext = new TemplateContext()
                            {
                                Element = element, TemplateOptions = options
                            };

            var strategy = StrategySelector.SelectStrategy(templateContext);
            if (strategy != null)
            {
                strategy.Apply(templateContext);
            }
        }

        public static void SetDynamicTemplate(UIElement control, object value)
        {
            control.SetValue(DynamicTemplateProperty, value);
        }

        public static bool GetDynamicTemplate(UIElement control)
        {
            return (bool)control.GetValue(DynamicTemplateProperty);
        }
    }
}