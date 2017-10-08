using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BasicTypes;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class RequestNumberSettingsFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1GenerationMethodValueLiteraltimestamp = "LiteralTimestamp";
        public const string Control1GenerationMethodValueUnixtimestamp = "UnixTimestamp";

        public RequestNumberGenerationMethod Control1GenerationMethod
        {
            get => (RequestNumberGenerationMethod) Enum.Parse(typeof(RequestNumberGenerationMethod),
                (string) GetValue(0));
            set => SetValue("GenerationMethod", value.ToString());
        }

        public long Control2Increment
        {
            get => (long)(decimal?) GetValue(1);
            set => SetValue("Increment", (decimal)value);
        }

        public RequestNumberSettingsFormValuesWrapper()
        {
        }

        public RequestNumberSettingsFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            ApplyOutcomeValues(values);
        }
    }
}