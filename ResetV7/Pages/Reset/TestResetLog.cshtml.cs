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
    public class TestResetLogModel : PageModel
    {
        private readonly ResetV7.Models.ApplicationDbContext _context;

        public TestResetLogModel(ResetV7.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public ResetLog ResetLog { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ResetLog = await _context.ResetLog
                .Include(r => r.LogType).FirstOrDefaultAsync(m => m.ResetID == id);

            if (ResetLog == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
