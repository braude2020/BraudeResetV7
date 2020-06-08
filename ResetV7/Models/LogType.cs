using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResetV7.Models
{
    public class LogType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
        public string description { get; set; }
    }
}
