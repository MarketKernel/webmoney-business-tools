using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IOriginalExpressTrust
    {
        string StorePurse { get; }
        IExtendedIdentifier Identifier { get; }
        decimal DayLimit { get; }
        decimal WeekLimit { get; }
        decimal MonthLimit { get; }
        ConfirmationType ConfirmationType { get; }
    }
}
