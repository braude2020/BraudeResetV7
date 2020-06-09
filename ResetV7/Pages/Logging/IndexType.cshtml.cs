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
    public class IndexTypeModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IEnumerable<LogType> LogTypes { get; set; }
        public IndexTypeModel(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task OnGet()
        {
            LogTypes = await _db.LogType.ToListAsync();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var logTypeFromDb = await _db.LogType.FindAsync(id);

            if (logTypeFromDb == null)
                return NotFound();

             _db.LogType.Remove(logTypeFromDb);
            await _db.SaveChangesAsync();

            return RedirectToPage("IndexType");
        } 
    }
}