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
        //public LogType LogType;
        public IList<ResetLog> ResetLog { get; set; }
        public IList<LogType> LogType { get; set; }
        public IndexResetLogModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGet()
        {



            ResetLog = await _db.ResetLog.ToListAsync();
            LogType = await _db.LogType.ToListAsync();
            //ResetLog.
            return Page();
        }
        //public async Task<String> getLogName(int i)
        //{
        //    if(i == null)
        //        return "NULL";
        //    LogType = await _db.LogType.FindAsync(i);
        //    if(LogType != null)
        //        return LogType.name;
        //    return "NULL";
        //}
    }
}