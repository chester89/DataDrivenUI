using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DataDrivenUI.Strategies;

namespace DataDrivenUI
{
    /// <summary>
    /// Default strategy selector - scans only executing assembly when looking for <see cref="ContentStrategyBase"/> implementations
    /// </summary>
    public class DefaultStrategySelector: IStrategySelector
    {
        List<ContentStrategyBase> strategies = new List<ContentStrategyBase>(); 

        protected virtual void ScanForStrategies()
        {
            strategies =
                Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => typeof(ContentStrategyBase).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t) as ContentStrategyBase).ToList();
        }

        public ContentStrategyBase SelectStrategy(TemplateContext context)
        {
            ScanForStrategies();
            return strategies.SingleOrDefault(s => s.Applies(context));
        }
    }
}
