using System;

namespace WebMoney.Services.Contracts
{
    public interface IFormattingService
    {
        string FormatIdentifier(long identifier);
        string FormatAmount(decimal amount);
        string FormatDateTime(DateTime dateTime);
        string FormatDate(DateTime dateTime);
    }
}