﻿using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.ExternalServices.Contracts
{
    public interface IExternalTrustService
    {
        IEnumerable<ITrust> SelectTrusts();
    }
}
