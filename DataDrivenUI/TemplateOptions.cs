using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataDrivenUI
{
    public enum TemplateOptions
    {
        /// <summary>
        /// Default value - provides no additional functionality
        /// </summary>
        None,
        /// <summary>
        /// Generates controls that don't allow user input 
        /// </summary>
        ReadOnly,
        /// <summary>
        /// Generates controls that do allow user input
        /// </summary>
        ReadWrite
    }
}
