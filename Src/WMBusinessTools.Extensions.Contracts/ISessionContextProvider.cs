using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions.Contracts
{
    public interface ISessionContextProvider
    {
        bool CheckCompatibility(EntranceContext context);
        SessionContext GetIdentifierContext(EntranceContext context);
    }
}
