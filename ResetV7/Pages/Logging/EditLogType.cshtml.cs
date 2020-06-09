using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResetV7.Models;

namespace ResetV7
{
    public class EditLogTypeModel : PageModel
    {
        private ApplicationDbContext _db;

        public EditLogTypeModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public LogType LogType { get; set; }
        public async Task OnGet(int id)
        {
            LogType = await _db.LogType.FindAsync(id);
        }
        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                var LogTypeFromDb = await _db.LogType.FindAsync(LogType.Id);
                LogTypeFromDb.name = LogType.name;
                LogTypeFromDb.description = LogType.description;


                await _db.SaveChangesAsync();

                return RedirectToPage("IndexType");
            }
            return RedirectToPage();
        }
    }
}