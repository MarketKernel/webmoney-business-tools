namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class OriginalPayment
    {
        public int PaymentId { get; set; }
        public string TargetPurse { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
