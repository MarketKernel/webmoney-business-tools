using System.Collections.Generic;

namespace Xml2WinForms.Templates
{
    public interface IShapeColumnTemplate : ITemplate
    {
        List<ControlTemplate> Controls { get; }
    }
}
