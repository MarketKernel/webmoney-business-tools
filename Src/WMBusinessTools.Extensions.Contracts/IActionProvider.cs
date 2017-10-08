namespace WMBusinessTools.Extensions.Contracts
{
    public interface IActionProvider<in TContext>
        where TContext : class
    {
        bool CheckCompatibility(TContext context);
        void RunAction(TContext context);
    }
}
