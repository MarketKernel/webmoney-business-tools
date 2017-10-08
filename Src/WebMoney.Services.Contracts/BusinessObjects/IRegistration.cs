using System;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IRegistration
    {
        long Identifier { get; }
        DateTime RegistrationDate { get; }
    }
}
