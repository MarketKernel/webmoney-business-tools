using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface IRequestNumberSettings
    {
        RequestNumberGenerationMethod Method { get; set; }
        long Increment { get; set; }
    }
}
