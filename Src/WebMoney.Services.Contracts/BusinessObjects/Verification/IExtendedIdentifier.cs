using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IExtendedIdentifier
    {
        ExtendedIdentifierType Type { get; }
        string Value { get; }
    }
}
