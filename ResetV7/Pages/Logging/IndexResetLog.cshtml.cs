using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ResetV7.Models;

namespace ResetV7
{
    public class IndexResetLogModel : PageModel
    {
        private ApplicationDbContext _db;
        public IList<ResetLog> ResetLog { get; set; }
        public IndexResetLogModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGet()
        {
            ResetLog = await _db.ResetLog.ToListAsync();
            //ResetLog.
            return Page();
        }
    }
}