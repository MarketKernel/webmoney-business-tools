using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace SupportAssistant
{
    internal sealed class MessageEntity : TableEntity
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public DateTime CreateTime { get; set; }

        public MessageEntity()
        {
        }

        public MessageEntity(string exceptionType, string message, string details)
            : base(exceptionType, Guid.NewGuid().ToString())
        {
            Message = message;
            Details = details;
            CreateTime = DateTime.UtcNow;
        }
    }
}
