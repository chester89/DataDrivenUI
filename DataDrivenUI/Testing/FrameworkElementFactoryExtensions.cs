using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace DataDrivenUI.Testing
{
    public static class FrameworkElementFactoryExtensions
    {
        public static FrameworkElementFactory AppendNew<T>(this FrameworkElementFactory factory, int count = 1) where T: UIElement
        {
            for (int i = 0; i < count; i++)
            {
                factory.AppendChild(new FrameworkElementFactory(typeof(T)));
            }
            return factory;
        }

        public static FrameworkElementFactory Create<T>() where T: UIElement
        {
            return new FrameworkElementFactory(typeof(T));
        }

        //public static void ValidateBindings(this IEnumerable<FrameworkElementFactory> factories)
        //{
        //    foreach (var factory in factories)
        //    {
        //        ValidateBindings(factory);
        //        var innerFactories = GetChildFactories(factory);
        //        ValidateBindings(innerFactories);
        //    }
        //}

        public static IEnumerable<FrameworkElementFactory> GetChildFactories<T>(this FrameworkElementFactory factory, int level = 1)
        {
            var result = new List<FrameworkElementFactory>();
            var firstChild = factory.FirstChild;

            int currentLevel = 1;

            while (firstChild != null)
            {
                if (firstChild.Type.IsAssignableTo<T>())
                {
                    result.Add(firstChild);
                }
                if (level > currentLevel)
                {
                    result.AddRange(firstChild.GetChildFactories<T>(level));
                    currentLevel++;
                }
                firstChild = firstChild.NextSibling;
            }

            return result;
        }

        static void ValidateBindings(FrameworkElementFactory frameworkElementFactory)
        {
            Debug.WriteLine("");
        }
    }
}