using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DataDrivenUI.Testing
{
    public class VisualTreeConfiguration
    {
        private readonly FrameworkElementFactory factory;

        public VisualTreeConfiguration(FrameworkElementFactory factory)
        {
            this.factory = factory;
        }

        public IEnumerable<FrameworkElementFactory> Nodes<T>()
        {
            var result = new List<FrameworkElementFactory>();
            var children = new List<FrameworkElementFactory>();

            var currentChild = factory;
            while (currentChild != null)
            {
                if (currentChild.GetType().IsAssignableTo<T>())
                {
                    result.Add(currentChild);
                }
                if (currentChild.FirstChild == null)
                {
                    if (currentChild.NextSibling == null)
                    {
                        break;
                    }
                    currentChild = currentChild.NextSibling;
                    continue;
                }
                if (currentChild.FirstChild.GetType().IsAssignableTo<T>())
                {
                    result.Add(currentChild.FirstChild);
                    currentChild = currentChild.FirstChild.NextSibling;
                    continue;
                }
                children.Add(currentChild.FirstChild);
                if (currentChild.Parent == null)
                {
                    currentChild = currentChild.NextSibling;
                }
                else
                {
                    currentChild = currentChild.FirstChild;
                }
            }

            foreach (var child in children)
            {
                result.AddRange(new VisualTreeConfiguration(child).Nodes<T>());
            }

            return result;
        }
    }
}
