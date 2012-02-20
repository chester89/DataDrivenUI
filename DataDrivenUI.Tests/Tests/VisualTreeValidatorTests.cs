using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DataDrivenUI.Testing;
using NUnit.Framework;

namespace DataDrivenUI.Tests.Tests
{
    [TestFixture]
    public class VisualTreeValidatorTests
    {
        [Test]
        public void ChecksRootElement()
        {
            var factory = FrameworkElementFactoryExtensions.Create<StackPanel>();
            var treeValidator = new VisualTreeValidator(factory);

            Assert.DoesNotThrow(treeValidator.RootElementIs<StackPanel>);
        }

        [Test]
        public void ThrowsOnRootElementMismatch()
        {
            var factory = FrameworkElementFactoryExtensions.Create<DatePicker>();
            var treeValidator = new VisualTreeValidator(factory);

            Assert.Throws<ArgumentException>(treeValidator.RootElementIs<StackPanel>);
        }

        [Test]
        public void ValidatesVisualTreeStructure()
        {
            var factory = FrameworkElementFactoryExtensions.Create<StackPanel>();
            const int textBoxCount = 5;
            const int buttonCount = 3;
            factory = factory.AppendNew<TextBox>(textBoxCount).AppendNew<Button>(buttonCount);

            var coreFactory = FrameworkElementFactoryExtensions.Create<StackPanel>();
            coreFactory.AppendChild(factory);

            var dt = new DataTemplate() { VisualTree = coreFactory};

            using (var streamWriter = new StreamWriter(File.Create(@"D:\Samples\visualTreeValidator.xaml")))
            {
                using (var textWriter = new IndentedTextWriter(streamWriter))
                {
                    System.Windows.Markup.XamlWriter.Save(dt, textWriter);
                }
            }

            var treeValidator = new VisualTreeValidator(dt.VisualTree);
            
            treeValidator.RootElementIs<StackPanel>(cfg =>
                                                    {
                                                        Assert.True(cfg.Nodes<TextBox>().Count() == textBoxCount);
                                                        Assert.True(cfg.Nodes<Button>().Count() == buttonCount);
                                                    });
        }

        [Ignore]
        [Test]
        public void ValidatesBindings()
        {
            var factory = FrameworkElementFactoryExtensions.Create<StackPanel>();
            const int textBoxCount = 5;
            const int buttonCount = 3;
            factory.AppendNew<Button>(buttonCount).AppendNew<TextBox>(textBoxCount);

            var treeValidator = new VisualTreeValidator(factory);

            //treeValidator.RootElementIs<StackPanel>(cfg => 
            //    Assert.DoesNotThrow(cfg.Nodes<Control>().ValidateBindings));
        }
    }
}
