using ElderCare_Domain.Enums;
using ElderCare_Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class StaffAccountCreateDto
    {
        public string? Username { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //[DataType(DataType.Password)]
        //[StringLength(maximumLength: 50, MinimumLength = 8, ErrorMessage = "Password cannot be longer than 40 characters and less than 8 characters")]
        //public string Password { get; set; }
        //[DataType(DataType.Password)]
        //[Compare(nameof(Password), ErrorMessage = "please make sure your password match.")]
        //public string PasswordConfirm { get; set; }
        public int RoleId { get; set; }
        public string? Address { get; set; }
    }
}
