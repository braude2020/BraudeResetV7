using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResetV7.Models;

namespace ResetV7
{
    public class ErrorModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ResetLog ResetLog { get; set; }

        public ErrorModel(ApplicationDbContext db)
        {
            _db = db;
        }
        //public void OnGet()
        //{
        //}
        public async Task OnGet(int id)
        {
            ResetLog = await _db.ResetLog.FindAsync(id);
        }
    }
}