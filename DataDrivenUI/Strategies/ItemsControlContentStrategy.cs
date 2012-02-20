using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace DataDrivenUI.Strategies
{
    public class ItemsControlContentStrategy: ContentStrategyBase
    {
        public override bool Applies(TemplateContext context)
        {
            return context.Element.GetType().IsItselfOrSubclassOf<ItemsControl>();
        }

        public override void Apply(TemplateContext context)
        {
            var itemsControl = context.Element as ItemsControl;

            if (itemsControl == null)
            {
                throw new ArgumentException("You can use DynamicTemplate property only on ItemsControl and its subclasses");
            }

            var descriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));
            descriptor.AddValueChanged(itemsControl,
                    (e, a) => GenerateDataTemplate(itemsControl));
        }

        private void GenerateDataTemplate(ItemsControl itemsControl)
        {
            if (itemsControl.ItemsSource == null) 
                return;
            var boundCollection = itemsControl.ItemsSource;

            Type viewModelType;
            if (boundCollection.GetType().IsGenericType)
            {
                //if collection is generic - get it's first generic argument
                viewModelType = boundCollection.GetType().GetGenericArguments().FirstOrDefault();
            }
            else
            {
                //if collection is non-generic - figure out the type using first element
                //todo: change to discover COMMON type for all elements in collection
                var enumerator = boundCollection.GetEnumerator();
                enumerator.MoveNext();
                viewModelType = enumerator.Current.GetType();
            }

            var template = BuildDataTemplate(viewModelType);
            if (template == null)
            {
                throw new ArgumentException("generated DT should not be null");
            }
            itemsControl.ItemTemplate = template;
        }

        private DataTemplate BuildDataTemplate(Type viewModelType)
        {
            if (viewModelType == null)
            {
                throw new ArgumentNullException("viewModelType");
            }

            const int standardMargin = 2;
            var defaultMargin = new Thickness(standardMargin);

            var allProperties = viewModelType.GetProperties().ToList();
            var commandProperties = allProperties.Where(pi => pi.PropertyType.IsAssignableTo<ICommand>()).ToList();
            var scalarProperties =
                allProperties.Where(pi => !pi.PropertyType.IsAssignableFrom<ICommand>()).ToList();

            var elementFactory = new FrameworkElementFactory(typeof (StackPanel));

            foreach (var scalarProperty in scalarProperties)
            {
                var textLineFactory = new FrameworkElementFactory(typeof (StackPanel));
                textLineFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                string name = scalarProperty.Name;

                //stackpanel for each scalar property contains of 2 textblocks - one for caption, one for value
                var captionTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                captionTextBlockFactory.SetValue(TextBlock.TextProperty, name + ":");
                captionTextBlockFactory.SetValue(FrameworkElement.MarginProperty, defaultMargin);
                textLineFactory.AppendChild(captionTextBlockFactory);

                var valueTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                valueTextBlockFactory.SetBinding(TextBlock.TextProperty, new Binding(name));
                valueTextBlockFactory.SetValue(FrameworkElement.MarginProperty, defaultMargin);
                textLineFactory.AppendChild(valueTextBlockFactory);

                elementFactory.AppendChild(textLineFactory);
            }

            //Create all buttons for commands
            if (commandProperties.Any())
            {
                var buttonPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
                buttonPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
                foreach (var commandProperty in commandProperties)
                {
                    var controlElementFactory = new FrameworkElementFactory(typeof(Button));
                    controlElementFactory.SetBinding(ButtonBase.CommandProperty, new Binding(commandProperty.Name));
                    controlElementFactory.SetValue(ContentControl.ContentProperty, commandProperty.Name);
                    buttonPanelFactory.AppendChild(controlElementFactory);
                }
                elementFactory.AppendChild(buttonPanelFactory);
            }

            return new DataTemplate
                       {
                           DataType = viewModelType,
                           VisualTree = elementFactory
                       };
        }
    }
}
