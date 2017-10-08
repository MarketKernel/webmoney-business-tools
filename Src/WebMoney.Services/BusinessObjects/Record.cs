using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMoney.Services.BusinessObjects
{
    [Table("Record")]
    internal sealed class Record
    {
        [Key, MaxLength(50)]
        public string Id { get; set; }

        public string Value { get; set; }

        internal Record()
        {
        }

        public Record(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}
