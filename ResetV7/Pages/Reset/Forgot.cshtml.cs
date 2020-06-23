using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public String ErrMessage { get; set; }

        public ForgotModel(ApplicationDbContext db)
        {
            _db = db;
        }
        //public void OnGet()
        //{
        //}
        public async Task<IActionResult> OnGet(int id)
        {
            var serverCheck = new DC();
            HttpClient client = new HttpClient();
            var checkingResponse = await client.GetAsync("https://simplesms.co.il/");
            ////    if (!checkingResponse.IsSuccessStatusCode)
            ////    {
            ////if(!serverCheck.isBizDcUp() || !serverCheck.isEduDcUp() || !serverCheck.isMailBitUp())
            try
            {
                if (!serverCheck.isBizDcUp())
                {
                    return Redirect("/Reset/Error");
                }
                if (!serverCheck.isEduDcUp())
                {
                    return Redirect("/Reset/Error");
                }
                if (!checkingResponse.IsSuccessStatusCode)
                {
                    return Redirect("/Reset/Error");
                }
            }
            catch(Exception ex)
            {
                return Redirect("/Reset/Error");
            }



            ResetLog = await _db.ResetLog.FindAsync(id);
            
            return Page();
            //if(ResetLog == null)
            //    return RedirectToPage("/Reset/Forgot");

            //return RedirectToPage("/Reset/Forgot", new { id = ResetLog.ResetID });
        }
        public async Task<IActionResult> OnPost()
        {
            //check if server is available 

            //












            var ResetLogFromDb = await _db.ResetLog.FindAsync(ResetLog.ResetID);

            if (ResetLogFromDb == null)
            {

                ResetLog.LogTypeId = 2;
                ResetLog.logTime = System.DateTime.Now;
                ResetLog.countForgot = 1;
                await _db.ResetLog.AddAsync(ResetLog);
                await _db.SaveChangesAsync();
                ResetLogFromDb = await _db.ResetLog.FindAsync(ResetLog.ResetID);
            }
            else
            {
                if(ResetLogFromDb.isSessionStillValide(ResetLogFromDb.logTime))
                {
                    ResetLogFromDb.LogTypeId = 17;
                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Reset/Error", new { id = ResetLogFromDb.ResetID });
                }
                ResetLogFromDb.countForgot = ResetLogFromDb.countForgot + 1;
                ResetLogFromDb.username = ResetLog.username;
                ResetLogFromDb.mobile = ResetLog.mobile;

            }


            //if(ResetLogFromDb.countForgot >= 3)
            //{
            //    //ResetLogFromDb.LogTypeId = 3;
            //    await _db.SaveChangesAsync();
            //    return RedirectToPage("/Reset/Error", new { id = ResetLog.ResetID });
            //}


            //    //ResetLog.checkUser(username, mobile)
            //    //0 - Somthing went very wrong    DB(5)
            //    //1 - no such user                DB(6)
            //    //2- Biz user                     DB(7)
            //    //3 - EDU user                    DB(8)
            //    //4 - BIZ EDU user                DB(9)
            //    //5 - not autorized               DB(10)


            //    int userCheck = 4;

            ResetLog.username = ResetLog.username.Replace("@braude.ac.il","");
            ResetLog.username = ResetLog.username.Replace("@s.braude.ac.il", "");
            ResetLog.username = ResetLog.username.Replace("@e.braude.ac.il", "");

            int userCheck = ResetLog.checkUser(ResetLog.username, ResetLog.mobile);
                userCheck = userCheck + 5;

            if (userCheck == 7 || userCheck == 9)
                ResetLogFromDb.bizUser = true;
            if (userCheck == 8 || userCheck == 9)
                ResetLogFromDb.eduUser = true;


            if(userCheck == 5 || userCheck == 6)
            {
                ResetLogFromDb.LogTypeId = userCheck;
                await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/Forgot", new { id = ResetLog.ResetID });
            }
            if (userCheck == 10)
            {
                ResetLogFromDb.LogTypeId = userCheck;
                await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/Error", new { id = ResetLog.ResetID });
            }


            //ResetLogFromDb.sessionToken = "123456";
            //Send SMS
            //ResetLog.sessionToken = ResetLog.generateToken();
            //ResetLog.sendSMS(ResetLog.mobile, ResetLog.sessionTokenCheck);

            if(userCheck == 7 || userCheck == 9 || userCheck == 8)
            {
                //ResetLogFromDb.sessionToken = "123456";
                //ResetLogFromDb.LogTypeId = 11;

                //Send SMS
                ResetLogFromDb.sessionToken = ResetLog.generateToken();
                ResetLogFromDb.sendSMS(ResetLogFromDb.mobile, ResetLogFromDb.sessionToken);
                
                await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/OTP", new { id = ResetLog.ResetID });
            }






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
            //        return RedirectToPage("/ResetV2/ErrorV2", new { id = ResetLog.sessionId });271800
            //    }
            //    if (userCheck == 4 || userCheck == 6)
            //        ResetLog.bizUser = true;
            //    if (userCheck == 5 || userCheck == 6)
            //        ResetLog.eduUser = true;


            //    ResetLog.sessionTokenCheck = "123456";
            //    //Send SMS
            //    //ResetLog.sessionTokenCheck = ResetLog.generateToken();
            //    //ResetLog.sendSMS(ResetLog.mobile, ResetLog.sessionTokenCheck);







            await _db.SaveChangesAsync();

            if (ResetLogFromDb.countForgot >= 3)
            {
                //ResetLogFromDb.LogTypeId = 3;
                //await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/Error", new { id = ResetLog.ResetID });
            }



            return RedirectToPage("/Reset/Forgot", new { id = ResetLog.ResetID });
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