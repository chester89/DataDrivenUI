using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DataDrivenUI.Strategies;

namespace DataDrivenUI
{
    /// <summary>
    /// Allows to manage strategy selection for Data-Driven UI feature
    /// </summary>
    public interface IStrategySelector
    {
        ContentStrategyBase SelectStrategy(TemplateContext context);
    }
}
