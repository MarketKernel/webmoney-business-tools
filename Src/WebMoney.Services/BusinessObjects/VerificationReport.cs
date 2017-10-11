using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class VerificationReport : IVerificationReport
    {
        public string Reference { get; }
        public string ClientName { get; }
        public string ClientMiddleName { get; }

        public VerificationReport(string reference, string clientName, string clientMiddleName)
        {
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            ClientName = clientName ?? throw new ArgumentNullException(nameof(clientName));
            ClientMiddleName = clientMiddleName ?? throw new ArgumentNullException(nameof(clientMiddleName));
        }
    }
}
