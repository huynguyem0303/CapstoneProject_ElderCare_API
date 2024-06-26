﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class CustomerSignUpDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[JsonProperty("phone_number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [DataType(DataType.Password)]
        [JsonProperty("password")]
        [StringLength(maximumLength: 50, MinimumLength = 8, ErrorMessage = "Password cannot be longer than 40 characters and less than 8 characters")]
        public string Password { get; set; }



        public class CustomerBankInfomationDto
        {
            //[JsonProperty("account_number")]
            [DataType(DataType.CreditCard)]
            public string AccountNumber { get; set; }

            //[JsonProperty("bank_name")]
            public string BankName { get; set; }

            [JsonProperty("branch")]
            public string Branch { get; set; }

            //[JsonProperty("account_name")]
            public string AccountName { get; set; }
        }

        //[JsonProperty("bank_info")]
        public CustomerBankInfomationDto BankInfo { get; set; }
    }
}
