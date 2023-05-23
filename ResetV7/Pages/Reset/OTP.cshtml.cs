using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResetV7.Models;

namespace ResetV7
{
    public class OTPModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ResetLog ResetLog { get; set; }

        public OTPModel(ApplicationDbContext db)
        {
            _db = db;
        }
        //public void OnGet()
        //{
        //}
        public async Task<IActionResult> OnGet(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToPage("/Reset/Error", "Error");
            }

            ResetLog = await _db.ResetLog.FindAsync(id);

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var ResetLogFromDb = await _db.ResetLog.FindAsync(ResetLog.ResetID);

            if (ResetLogFromDb.isSessionStillValide(ResetLogFromDb.logTime))
            {
                ResetLogFromDb.LogTypeId = 17;
                await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/Error", new { id = ResetLogFromDb.ResetID });
            }


            if (ResetLogFromDb.sessionToken == ResetLog.sessionTokenCheck)
            {
                ResetLogFromDb.LogTypeId = 13;
                ResetLogFromDb.sessionTokenCheck = "OK";
                await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/Reset", new { id = ResetLog.ResetID, otp = ResetLog.sessionTokenCheck });

            }
            else
            {
                ResetLogFromDb.countOTP = ResetLogFromDb.countOTP + 1;
                ResetLogFromDb.LogTypeId = 12;
                await _db.SaveChangesAsync();
                if (ResetLogFromDb.countOTP >= 3)
                    return RedirectToPage("/Reset/Error", new { id = ResetLog.ResetID });
            }


            return RedirectToPage("/Reset/OTP", new { id = ResetLog.ResetID });
        }
    }
}