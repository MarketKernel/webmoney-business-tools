using System;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.Services
{
    public sealed class MessageService : SessionBasedService, IMessageService
    {
        public long SendMessage(long toIdentifier, string subject, string message, bool force)
        {
            if (null == message)
                throw new ArgumentNullException(nameof(message));

            var request = new OriginalMessage((WmId) toIdentifier,(Message) message)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            if (null != subject)
                request.Subject = (Description) subject;

            RecentMessage response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            return response.Id;
        }
    }
}
