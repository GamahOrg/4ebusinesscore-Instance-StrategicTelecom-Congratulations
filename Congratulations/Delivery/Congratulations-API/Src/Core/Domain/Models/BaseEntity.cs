using _4EPlatform.Domain.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace _4EPlatform.Domain.Models
{
    public class BaseEntity<TKey>
    {
        public TKey Id { set; get; }

        public State State { set; get; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
