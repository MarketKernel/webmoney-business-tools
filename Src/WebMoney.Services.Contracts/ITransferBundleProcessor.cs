namespace WebMoney.Services.Contracts
{
    public interface ITransferBundleProcessor
    {
        void RunAsync();
        void CancelAsync();
    }
}
