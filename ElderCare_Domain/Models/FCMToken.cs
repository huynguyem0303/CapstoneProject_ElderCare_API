using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Domain.Models
{
    public class FCMToken
    {
        public Guid TokenId { get; set; }
        public int AccountId { get; set; }
        public string TokenValue { get; set; }

        public virtual Account Account { get; set; }
    }
}
