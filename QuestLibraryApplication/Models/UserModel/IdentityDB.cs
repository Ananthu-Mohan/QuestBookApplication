using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuestLibraryApplication.Models.UserModel
{
    public class IdentityDB
    {
        [Key]
        public int UserID { get; set; }
        [DisplayName("Username")]
        public string UserName { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [DisplayName("Phone number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Provided phone number not valid")]
        [MinLength(5,ErrorMessage ="Enter a valid phone number"),MaxLength(10, ErrorMessage = "Enter a valid phone number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Account created on")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Last Login Activity on Account")]
        public DateTime LastLoginAt { get; set; }

    }
}