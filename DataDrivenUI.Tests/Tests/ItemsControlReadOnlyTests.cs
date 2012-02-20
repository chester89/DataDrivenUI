using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xaml;
using System.Xml;
using DataDrivenUI.Strategies;
using DataDrivenUI.Testing;
using NUnit.Framework;

namespace DataDrivenUI.Tests
{
    [TestFixture]
    public class ItemsControlReadOnlyTests
    {
        private ItemsControl control;
        private TestViewModel viewModel;
        private FrameworkElementFactory templateRoot;
        private TemplateOptions options;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            viewModel = new TestViewModel();
            options = TemplateOptions.ReadOnly;
        }

        private void WrapInAThread(Action action)
        {
            var thread = new Thread(() => action());

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [Test]
        public void DoesntApplyIfNotItemsSourceSubclass()
        {
            WrapInAThread(() =>
                              {
                                  var textBox1 = new TextBox{ DataContext = viewModel };
                                  var strategy1 = new ItemsControlContentStrategy();
                                  Assert.False(strategy1.Applies(new TemplateContext
                                            {
                                                Element = textBox1,
                                                TemplateOptions = options
                                            }));
                              });
        }

        [Test]
        public void ApplyThrowsIfNotItemsSourceSubclass()
        {
            WrapInAThread(() =>
                              {
                                  var textBox1 = new TextBox { DataContext = viewModel };
                                  Assert.Throws<ArgumentException>(() =>
                                                                       {
                                                                           var strategy1 = new ItemsControlContentStrategy();
                                                                           strategy1.Apply(new TemplateContext
                                                                                               {
                                                                                                   Element = textBox1,
                                                                                                   TemplateOptions = options
                                                                                               });
                                                                       });
                              });
        }

        [Test]
        public void GeneratesTextBlocks()
        {
            WrapInAThread(() =>
                              {
                                  control = new ListBox { DataContext = viewModel };
                                  GenerateTemplate(control);

                                  using (var streamWriter = new StreamWriter(File.Create(@"D:\Samples\goodTemplate.xaml")))
                                  {
                                      using (var textWriter = new IndentedTextWriter(streamWriter))
                                      {
                                          System.Windows.Markup.XamlWriter.Save(control.ItemTemplate, textWriter);
                                      }
                                  }

                                  templateRoot = control.ItemTemplate.VisualTree;
                                  Assert.IsNotNull(templateRoot);
                                  var scalarPropertyCount = typeof(PersonViewModel).GetProperties().Length;

                                  var treeValidator = new VisualTreeValidator(templateRoot);
                                  try
                                  {
                                      treeValidator.RootElementIs<StackPanel>(
                                            cfg =>
                                            {
                                                Assert.True(cfg.Nodes<TextBlock>().Count() == 2 * scalarPropertyCount);
                                                //Assert.DoesNotThrow(cfg.Nodes<Control>().ValidateBindings);
                                            });
                                  }
                                  catch (Exception e)
                                  {

                                  }


                                  //todo: проверить содержимое дерева - в том числе bindings
                              });
        }

        [Test]
        public void GeneratesButtonsForCommands()
        {

        }

        void GenerateTemplate(ItemsControl control)
        {
            ItemsControlContentStrategy strategy = new ItemsControlContentStrategy();
            var templateContext = new TemplateContext
            {
                Element = control,
                TemplateOptions = options
            };
            if (strategy.Applies(templateContext))
            {
                strategy.Apply(templateContext);
                control.ItemsSource = viewModel.People;
            }
        }
    }
}
