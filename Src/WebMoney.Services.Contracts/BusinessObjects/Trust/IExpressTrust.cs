namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IExpressTrust
    {
        int TrustId { get; }
        string SlavePurse { get; }
        long SlaveIdentifier { get; }
        string PublicMessage { get; }
    }
}
