using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResetV7.Models;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;


namespace ResetV7
{
    public class ResetModel : PageModel
    {
        [ViewData]
        public string Message { get; set; }

        private readonly ApplicationDbContext _db;
        private Boolean emailSend = false;

        [BindProperty]
        //public ResetLog ResetLog { get; set; }
        public ResetPassword ResetPassword { get; set; }

        public ResetModel(ApplicationDbContext db)
        {
            _db = db;
        }
        //public void OnGet()
        //{
        //    int test = 44;
        //}
        public async Task<IActionResult> OnGet(Guid id, string otp)
        {
            if(id == Guid.Empty)
            {
                return RedirectToPage("/Reset/Error", "Error");
            }
            else
            {
                if (ResetPassword == null)
                    ResetPassword = new ResetPassword();
                ResetPassword.ResetID = id;

                var ResetLogFromDb = await _db.ResetLog.FindAsync(ResetPassword.ResetID);
                if(ResetLogFromDb.sessionToken == null || !ResetLogFromDb.sessionToken.Equals(otp)) 
                {
                    return RedirectToPage("/Reset/Error", "Error");
                }

                if (ResetLogFromDb.sessionTokenCheck == null || !ResetLogFromDb.sessionTokenCheck.Equals("OK"))
                {
                    return RedirectToPage("/Reset/Error", "Error");
                }

            }

            return Page();





            //ResetLog = await _db.ResetLog.FindAsync(id);
        }
        public async Task<IActionResult> OnPost()
        {

            


            var ResetLogFromDb = await _db.ResetLog.FindAsync(ResetPassword.ResetID);
            bool PasswdResetError = false;

            string uName = ResetLogFromDb.username;
            string uPasswd = ResetPassword.Password;

            if (uPasswd.ToUpper().Contains(uName.ToUpper()))
            {
                
                Message = "Problem: Password contains username";
                return Page();
            //    //ViewData["Message"] = "Problem: Password contains username";

                
            }






            if (ResetLogFromDb.isSessionStillValide(ResetLogFromDb.logTime))
            {
                ResetLogFromDb.LogTypeId = 17;
                await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/Error", new { id = ResetLogFromDb.ResetID });
            }


            if (!ModelState.IsValid)
            {
                ResetLogFromDb.LogTypeId = 14;
                ResetLogFromDb.countReset++;

            }
            else
            {
                


                if (ResetLogFromDb.bizUser && ResetLogFromDb.eduUser)
                { 
                    if (updateBiz(ResetLogFromDb.username, ResetPassword.Password) && updateEdu(ResetLogFromDb.username, ResetPassword.Password))
                    {
                        String messageText = "איפוס סיסמה בוצע בחשבונותך:" + "\r\n" + ResetLogFromDb.username + "@braude.ac.il \r\n" + ResetLogFromDb.username + "@e.braude.ac.il \r\n" + "אם השינוי לא בוצע על ידיך אנא פנה באופן מיידי לאבטחת מידע " + "\r\n" + "security@braude.ac.il";
                        await sendMailTo(ResetLogFromDb.username + "@braude.ac.il", "איפוס סיסמה בוצע בחשבונך", messageText);
                        ResetLogFromDb.LogTypeId = 15;
                    }
                    else
                    {
                        PasswdResetError = true;
                        ResetLogFromDb.LogTypeId = 14;
                    }
                        
                }
                else if (ResetLogFromDb.bizUser && !(ResetLogFromDb.eduUser))
                {
                    if (updateBiz(ResetLogFromDb.username, ResetPassword.Password))
                    {
                        String messageText = "איפוס סיסמה בוצע בחשבונותך:" + "<br>" + ResetLogFromDb.username + "@braude.ac.il <br>" +  "אם השינוי לא בוצע על ידיך אנא פנה באופן מיידי לאבטחת מידע " + "<br>" + "security@braude.ac.il";

                        await sendMailTo(ResetLogFromDb.username + "@braude.ac.il", "איפוס סיסמה בוצע בחשבונך", messageText);
                        ResetLogFromDb.LogTypeId = 15;
                    }
                    else
                    {
                        PasswdResetError = true;
                        ResetLogFromDb.LogTypeId = 14;
                    }
                        
                }
                else if(!(ResetLogFromDb.bizUser) && ResetLogFromDb.eduUser)
                {
                    if (updateEdu(ResetLogFromDb.username, ResetPassword.Password))
                    {
                        String messageText = "איפוס סיסמה בוצע בחשבונותך:" + "<br>"  + ResetLogFromDb.username + "@e.braude.ac.il <br>" + "אם השינוי לא בוצע על ידיך אנא פנה באופן מיידי לאבטחת מידע " + "<br>" + "security@braude.ac.il";

                        await sendMailTo(ResetLogFromDb.username + "@e.braude.ac.il", "איפוס סיסמה בוצע בחשבונך", messageText);
                        ResetLogFromDb.LogTypeId = 15;
                    }
                    else
                    {
                        
                            PasswdResetError = true;
                            ResetLogFromDb.LogTypeId = 14;
                        
                    }
                        
                }
                if(!PasswdResetError)
                {
                    ResetLogFromDb.LogTypeId = 15;
                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Reset/Done", new { id = ResetPassword.ResetID });
                }
                else
                {
                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Reset/Error", new { id = ResetLogFromDb.ResetID });
                }
                
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("/Reset/Reset", new { id = ResetPassword.ResetID }); ;
        }
        public Boolean updateBiz(string username, string password)
        {
            try
            {
                //PrincipalContext context = new PrincipalContext(ContextType.Domain, "192.168.0.2", "OU=Administration,OU=BRDUsers,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
                PrincipalContext context = new PrincipalContext(ContextType.Domain, "10.168.0.2", "OU=BRDUsers,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);
                user.Enabled = true;
                //user.EmailAddress = password;
                try
                {
                    user.SetPassword(password);
                }catch(Exception ex)
                {
                    return false;
                    var test = ex;
                }
                
                user.Save();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public Boolean updateEdu(string username, string password)
        {
            try
            {
                //OU=edu,OU=BrdUsers,DC=brdeng,DC=ac
                //PrincipalContext context2 = new PrincipalContext(ContextType.Domain, "192.168.130.10", "OU=BrdUsers,DC=brdeng,DC=ac", "ADSyncService", "9eV8H@G4z1XH");
                PrincipalContext context2 = new PrincipalContext(ContextType.Domain, "10.168.130.10", "OU=edu,OU=BrdUsers,DC=brdeng,DC=ac", "ADSyncService", "9eV8H@G4z1XH");
                UserPrincipal user2 = UserPrincipal.FindByIdentity(context2, IdentityType.SamAccountName, username);
                user2.Enabled = true;
                //user2.EmailAddress = password;
                try
                {
                    //user2.pa
                    user2.SetPassword(password);
                }
                catch (Exception ex)
                {
                    return false;
                    var test = ex;
                }


                user2.Save();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task SendMailGraphAPI(String email, String subject, String emailMessage)
        {

        }


        public async Task sendMailTo(String email, String subject, String emailMessage)
        {
            if (emailSend == true)
                return;
            else
                emailSend = true;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Password reset", "password.reset2@braude.ac.il"));
            message.To.Add(new MailboxAddress("Braude", email));


            




            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = emailMessage
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    //192.168.0.6 25
                    client.Connect("192.168.0.6", 25, false);

                    //client.Connect("smtp.office365.com", 587, false);
                    //client.Authenticate("password.reset@braude.ac.il", "3wtI$g&T0235!19zl$@rA5F0!");
                    
                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
                catch(Exception ex)
                {
                    var t = ex.Message;
                }
                
            }
        }
        //public async Task sendMailToAsync(String email)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Password reset", "security@braude.ac.il"));
        //    message.To.Add(new MailboxAddress("Braude", email));
        //    message.Subject = "You Password was Reset";
        //    message.Body = new TextPart("plain")
        //    {
        //        Text = "Your Password was reset"
        //    };
        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect("smtp.office365.com", 587, false);
        //        client.Authenticate("security@braude.ac.il", "3D0w&rBo");
        //        await client.SendAsync(message);
        //        client.Disconnect(true);
        //    }
        //}
    }
}