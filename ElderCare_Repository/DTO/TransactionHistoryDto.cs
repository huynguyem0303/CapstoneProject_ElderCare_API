using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class TransactionHistoryDto
    {
        //[JsonProperty("transaction_id")]
        public int TransactionId { get; set; }

        //[JsonProperty("figure_money")]
        public double? FigureMoney { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("datetime")]
        public DateTime? Datetime { get; set; }

        //[JsonProperty("contract_id")]
        public int? ContractId { get; set; }

        //[JsonProperty("carer_id")]
        public int? CarerId { get; set; }

        public string? CarerName { get; set; }

        //[JsonProperty("customer_id")]
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }
    }
}
