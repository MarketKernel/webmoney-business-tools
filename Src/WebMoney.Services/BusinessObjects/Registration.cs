using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    internal sealed class Registration : IRegistration
    {
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; }

        public DateTime RegistrationDate { get; }

        public Registration(long identifier, DateTime registrationDate)
        {
            Identifier = identifier;
            RegistrationDate = registrationDate;
        }
    }
}
