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
        public LogType LogType { get; set; }

        public ErrorModel(ApplicationDbContext db)
        {
            _db = db;
        }
        //public void OnGet()
        //{
        //}
        public async Task OnGet(int id)
        {
            int i;
            ResetLog = await _db.ResetLog.FindAsync(id);

            if (ResetLog != null)
            {
                LogType = await _db.LogType.FindAsync(ResetLog.LogTypeId);
                DateTime tmp = ResetLog.logTime.AddMinutes(5);


                if (tmp > System.DateTime.Now)
                    i = 1;
                else
                    ResetLog = null;
                // if (!ResetLog.isSessionStillValide(ResetLog.logTime))
                //     ResetLog = null;
            }    

            

            

            
            
        }
    }
}