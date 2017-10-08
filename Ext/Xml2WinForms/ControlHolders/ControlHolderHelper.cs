using System;
using System.Collections.Generic;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    internal static class ControlHolderHelper
    {
        public static List<IControlHolder> BuildControlHolders(IEnumerable<ControlTemplate> controls)
        {
            if (null == controls)
                throw new ArgumentNullException(nameof(controls));

            var controlHolders = new List<IControlHolder>();

            foreach (var template in controls)
            {
                controlHolders.Add(template.BuildControlHolder());
            }

            return controlHolders;
        }
    }
}
