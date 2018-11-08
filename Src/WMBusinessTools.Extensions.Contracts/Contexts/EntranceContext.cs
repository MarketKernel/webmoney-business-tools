using System;
using ExtensibilityAssistant;
using Unity;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class EntranceContext
    {
        public ExtensionManager ExtensionManager { get; }
        public IUnityContainer UnityContainer { get; }

        public EntranceContext(EntranceContext origin)
        {
            if (null == origin)
                throw new ArgumentNullException(nameof(origin));

            ExtensionManager = origin.ExtensionManager;
            UnityContainer = origin.UnityContainer;
        }

        public EntranceContext(ExtensionManager extensionManager, IUnityContainer unityContainer)
        {
            ExtensionManager = extensionManager ?? throw new ArgumentNullException(nameof(extensionManager));
            UnityContainer = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));
        }
    }
}
