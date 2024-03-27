using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class PushTicketRequestDto
    {
        public List<string> To { get; set; }

        //public object Data { get; set; }

        public string Title { get; set; }

        public string body { get; set; }

        //public int? Ttl { get; set; }

        //public int? Expiration { get; set; }

        public string? Priority { get; set; } //'default' | 'normal' | 'high'

        public string SubTitle { get; set; }

        public string? Sound { get; set; } //'default' | null

        public int? Badge { get; set; }

        public string ChannelId { get; set; }

        //public string CategoryId { get; set; }


        public bool MutableContent { get; set; } = true;
    }
}
