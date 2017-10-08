using System;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using WebMoney.Services.Contracts;

namespace SupportAssistant
{
    public sealed class SupportService : ISupportService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SupportService));

        private const string SasToken =
            "?sv=2017-04-17&" +
            "ss=t&" +
            "srt=o&" +
            "sp=a&" +
            "se=2027-09-30T12:00:00Z&" +
            "st=2017-09-30T12:00:00Z&" +
            "spr=https&" +
            "sig=LKCEohh5rsLxQrMhdKKsZnpiyv9iqI4Ennr1%2BXDgpH4%3D";

        public void SendMessage(string exceptionType, string message, string details)
        {
            if (null == exceptionType)
                throw new ArgumentNullException(nameof(exceptionType));

            if (null == message)
                throw new ArgumentNullException(nameof(message));

            var storageCredentials = new StorageCredentials(SasToken);
            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, "wmbt", null, true);
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            var table = cloudTableClient.GetTableReference("Message");
            var messageEntity = new MessageEntity(exceptionType, message, details);
            var insertOperation = TableOperation.Insert(messageEntity);

            try
            {
                table.Execute(insertOperation);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
                throw;
            }
        }
    }
}
