using System;

namespace ExtensibilityAssistant
{
    public sealed class AssemblyBrief
    {
        private readonly byte[] _publicKeyToken;

        public string AssemblyFullName { get; }

        public AssemblyBrief(string assemblyFullName, byte[] publicKeyToken)
        {
            AssemblyFullName = assemblyFullName ?? throw new ArgumentNullException(nameof(assemblyFullName));
            _publicKeyToken = publicKeyToken ?? throw new ArgumentNullException(nameof(publicKeyToken));
        }

        public byte[] GetPublicKeyToken()
        {
            return _publicKeyToken;
        }
    }
}