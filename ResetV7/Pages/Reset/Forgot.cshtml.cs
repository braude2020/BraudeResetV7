using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResetV7.Models;

namespace ResetV7
{
    public class ForgotModel : PageModel
    {
       

        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ResetLog ResetLog { get; set; }

        public ForgotModel(ApplicationDbContext db)
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
        public async Task<IActionResult> OnPost()
        {
            var ResetLogFromDb = await _db.ResetLog.FindAsync(ResetLog.ResetID);

            if(ResetLogFromDb == null)
            {
                
                ResetLog.LogTypeId = 2;
                await _db.ResetLog.AddAsync(ResetLog);
                await _db.SaveChangesAsync();
                ResetLogFromDb = await _db.ResetLog.FindAsync(ResetLog.ResetID);
            }
            ResetLogFromDb.username = ResetLog.username;

            //if (ModelState.IsValid)
            //{
            //    var LogTypeFromDb = await _db.LogType.FindAsync(LogType.Id);
            //    LogTypeFromDb.name = LogType.name;
            //    LogTypeFromDb.description = LogType.description;


            //    await _db.SaveChangesAsync();

            //    return RedirectToPage("IndexType");
            //}
            return RedirectToPage();
        }
        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return Page();
        //    }

        //    ResetLog = await _db.ResetLog.FirstOrDefaultAsync(m => m.sessionId == id);

        //    if (ResetLog == null)
        //    {
        //        return NotFound();
        //    }
        //    return Page();
        //}
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    //int loginCount = 0;

        //    //if (HttpContext.Session.GetInt32("forgetCount") == null)
        //    //    loginCount = 1;  //(int)HttpContext.Session.GetInt32("forgetCount");
        //    //else
        //    //{
        //    //    loginCount = (int)HttpContext.Session.GetInt32("forgetCount");
        //    //    loginCount += 1;
        //    //}

        //    //if (loginCount > 2)
        //    //{
        //    //    _db.ResetLog.Update(ResetLog);
        //    //    await _db.SaveChangesAsync(); }
        //    //    //return RedirectToPage("Error");

        //    //HttpContext.Session.SetInt32("forgetCount", loginCount);

        //    if (ResetLog.countLogin == 0)
        //        ResetLog.countLogin = 1;
        //    else
        //        ResetLog.countLogin++;




        //    //ResetLog.sessionTokenCheck = ResetLog.generateToken();

        //    //Send SMS
        //    //ResetLog.sendSMS(ResetLog.mobile, ResetLog.sessionTokenCheck);
        //    //if (ResetLog.countLogin == 0)
        //    //    ResetLog.countLogin = 1;
        //    //else if(ResetLog.countLogin == 1)
        //    //    ResetLog.countLogin = 2;
        //    //ResetLog.countLogin = loginCount;

        //    //ResetLog.countLogin = ResetLog.countLogin +1;
        //    if (ResetLog.sessionId == 0)
        //    {
        //        ResetLog.logTime = System.DateTime.Now;
        //        _db.ResetLog.Add(ResetLog);
        //    }
        //    else
        //        _db.ResetLog.Update(ResetLog);

        //    //if more the 3 logins
        //    if (ResetLog.countLogin > 2)
        //    {
        //        if (ResetLog.err == 3)
        //            ResetLog.err = 3;
        //        else if (ResetLog.err == 7)
        //            ResetLog.err = 7;
        //        else
        //            ResetLog.err = 2;
        //        _db.ResetLog.Update(ResetLog);
        //        await _db.SaveChangesAsync();
        //        return RedirectToPage("/ResetV2/ErrorV2", new { id = ResetLog.sessionId });
        //    }

        //    //ResetLog.checkUser(username, mobile)
        //    //0 - Bad           DB()
        //    //1 - no such user  DB(3)
        //    //2- Biz user       DB(4)
        //    //3 - EDU user      DB(5)
        //    //4 - BIZ EDU user  DB(6)
        //    //5 - not autorized DB(7)



        //    //int userCheck = 4;
        //    int userCheck = ResetLog.checkUser(ResetLog.username, ResetLog.mobile);
        //    userCheck = userCheck + 2;


        //    if (userCheck == 3 || userCheck == 0)
        //    {
        //        ResetLog.err = userCheck;
        //        //_db.ResetLog.Update(ResetLog);
        //        await _db.SaveChangesAsync();
        //        return RedirectToPage("/ResetV2/ForgotV2", new { id = ResetLog.sessionId });
        //    }


        //    if (userCheck == 3 || userCheck == 7)
        //    {
        //        ResetLog.err = userCheck;
        //        //_db.ResetLog.Update(ResetLog);
        //        await _db.SaveChangesAsync();
        //        return RedirectToPage("/ResetV2/ErrorV2", new { id = ResetLog.sessionId });
        //    }
        //    if (userCheck == 4 || userCheck == 6)
        //        ResetLog.bizUser = true;
        //    if (userCheck == 5 || userCheck == 6)
        //        ResetLog.eduUser = true;


        //    ResetLog.sessionTokenCheck = "123456";
        //    //Send SMS
        //    //ResetLog.sessionTokenCheck = ResetLog.generateToken();
        //    //ResetLog.sendSMS(ResetLog.mobile, ResetLog.sessionTokenCheck);



        //    //_db.ResetLog.Update(ResetLog);
        //    await _db.SaveChangesAsync();

        //    return RedirectToPage("/ResetV2/OTPV2", new { id = ResetLog.sessionId });
        //    //resetLog.sessionId = 1;
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return Page();
        //    //}
        //    //resetLog.logTime = System.DateTime.Now;
        //    //_db.ResetLog.Add(ResetLog);
        //    //await _db.SaveChangesAsync();
        //    //_db.ResetLog.Add(resetLog);
        //    //await _db.Sav


        //    //return Page();

        //    //return RedirectToPage("/ResetV2/ForgotV2", new { id = ResetLog.sessionId });


        //    //int count = 0;

        //    //if (HttpContext.Session.GetInt32("forgetCount") == null)
        //    //    count = 1;  //(int)HttpContext.Session.GetInt32("forgetCount");
        //    //else
        //    //{
        //    //    count = (int)HttpContext.Session.GetInt32("forgetCount");
        //    //    count += 1;
        //    //}

        //    //if (count > 2)
        //    //    return RedirectToPage("Error");

        //    //HttpContext.Session.SetInt32("forgetCount", count);



        //    //if (ModelState.IsValid)
        //    //{ 

        //    //    int type = aduser.checkUser(aduser.username, aduser.mobile);

        //    //    if (type == 5 || type == 1 || type == 0)
        //    //        return Page();

        //    //    if (type == 2 || type == 3 || type == 4)
        //    //    {
        //    //        string token = aduser.generateToken();
        //    //        aduser.sendSMS(aduser.mobile, token);

        //    //        HttpContext.Session.SetString("token", token);
        //    //        HttpContext.Session.SetString("username", "" + aduser.username);
        //    //        HttpContext.Session.SetString("mobile", "" + aduser.mobile);
        //    //        HttpContext.Session.SetInt32("type", type);


        //    //        return RedirectToPage("OTP");

        //    //    }
        //    //}

        //    //return Page();


        //}
    }
}