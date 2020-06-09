using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResetV7.Models;

namespace ResetV7
{
    public class ResetModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        [BindProperty]
        //public ResetLog ResetLog { get; set; }
        public ResetPassword ResetPassword { get; set; }

        public ResetModel(ApplicationDbContext db)
        {
            _db = db;
        }
        //public void OnGet()
        //{
        //}
        public void OnGet(int id)
        {
            if (ResetPassword == null)
                ResetPassword = new ResetPassword();
            ResetPassword.ResetID = id;
            //ResetLog = await _db.ResetLog.FindAsync(id);
        }
        public async Task<IActionResult> OnPost()
        {
            var ResetLogFromDb = await _db.ResetLog.FindAsync(ResetPassword.ResetID);

            if (!ModelState.IsValid)
            {
                ResetLogFromDb.LogTypeId = 14;
                ResetLogFromDb.countReset++;
               
            }
            else
            {
                ResetLogFromDb.LogTypeId = 15;
                await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/Done", new { id = ResetPassword.ResetID });
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("/Reset/Reset", new { id = ResetPassword.ResetID }); ;
        }
    }
}