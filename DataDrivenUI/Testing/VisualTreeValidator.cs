using System;
using System.Windows;

namespace DataDrivenUI.Testing
{
    public class VisualTreeValidator
    {
        private FrameworkElementFactory factory;

        public VisualTreeValidator(FrameworkElementFactory factory)
        {
            this.factory = factory;
        }

        public void RootElementIs<T>()
        {
            if (factory.Type != typeof(T))
            {
                throw new ArgumentException(string.Format("Root element in factory should be of type {0}", typeof(T).FullName));
            }
        }

        public void RootElementIs<T>(Action<VisualTreeConfiguration> configuration)
        {
            RootElementIs<T>();
            var cfg = new VisualTreeConfiguration(factory);
            configuration(cfg);
        }
    }
}
