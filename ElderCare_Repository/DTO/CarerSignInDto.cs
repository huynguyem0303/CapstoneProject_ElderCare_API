using ElderCare_Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class CarerSignInDto
    {
        [JsonProperty("name")]
        public string Name {  get; set; }

        [JsonProperty("email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("age")]
        [Range(20, 50)]
        public string Age { get; set; }

        [JsonProperty("image_url")]
        public string? Image { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }
        public class BankInfomation
        {
            [JsonProperty("account_number")]
            [DataType(DataType.CreditCard)]
            public string AccountNumber { get; set; }

            [JsonProperty("bank_name")]
            public string BankName { get; set; }

            [JsonProperty("branch")]
            public string Branch { get; set; }

            [JsonProperty("account_name")]
            public string AccountName { get; set; }
        }
        
        [JsonProperty("bank_info")]
        public BankInfomation BankInfo { get; set; }
    }
}
