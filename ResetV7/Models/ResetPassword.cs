using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ResetV7.Models
{
    public class ResetPassword
    {
        public int ResetID { get; set; }

        [Required]
        [RegularExpression("^(?=.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).*$", ErrorMessage = "Passwords must be at least 8 characters and the following: upper case (A-Z), lower case (a-z) and number (0-9)")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new Password")]
        [Compare("Password", ErrorMessage = "The new password and conformation password do not match")]
        public string ConfirmPassword { get; set; }
        public int countReset { get; set; }
    }
}
