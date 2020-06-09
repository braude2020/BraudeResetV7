using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResetV7.Models;

namespace ResetV7
{
    public class CreateLogTypeModel : PageModel
    {
        
        [BindProperty]
        public LogType LogType { get; set; }
        private readonly ApplicationDbContext _db;
        public CreateLogTypeModel(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                await _db.LogType.AddAsync(LogType);
                await _db.SaveChangesAsync();
                return RedirectToPage("IndexType");
            }
            else
            {
                return Page();
            }
        }
    }
}