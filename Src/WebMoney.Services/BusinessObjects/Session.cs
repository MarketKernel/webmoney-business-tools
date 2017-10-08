using System;
using System.ComponentModel.DataAnnotations;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    public sealed class Session : ISession
    {
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long CurrentIdentifier { get; set; }

        public IAuthenticationService AuthenticationService { get; }
        public ISettingsService SettingsService { get; }

        public Session(long identifier, IAuthenticationService authenticationService, ISettingsService settingsService)
        {
            CurrentIdentifier = identifier;
            AuthenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            SettingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }
    }
}
