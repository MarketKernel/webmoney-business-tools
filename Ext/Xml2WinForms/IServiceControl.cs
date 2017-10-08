using System;

namespace Xml2WinForms
{
    public interface IServiceControl
    {
        event EventHandler<CommandEventArgs> ServiceCommand;
    }
}
