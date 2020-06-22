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
        [RegularExpression("^(?=.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).*$", ErrorMessage = "סיסמה בת 8 תוים לפחות, אות גדולה, אות קטנה ומספרים")]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמה חדשה")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "אשר סיסמה חדשה")]
        [Compare("Password", ErrorMessage = "The new password and conformation password do not match")]
        public string ConfirmPassword { get; set; }
        public int countReset { get; set; }
    }
}
