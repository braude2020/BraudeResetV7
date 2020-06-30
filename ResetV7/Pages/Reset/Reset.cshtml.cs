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
        //}
        public void OnGet(Guid id)
        {
            if (ResetPassword == null)
                ResetPassword = new ResetPassword();
            ResetPassword.ResetID = id;
            //ResetLog = await _db.ResetLog.FindAsync(id);
        }
        public async Task<IActionResult> OnPost()
        {
            var ResetLogFromDb = await _db.ResetLog.FindAsync(ResetPassword.ResetID);


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
                        await sendMailTo(ResetLogFromDb.username + "@braude.ac.il");
                        ResetLogFromDb.LogTypeId = 15;
                    }
                    else
                        ResetLogFromDb.LogTypeId = 14;
                }
                else if (ResetLogFromDb.bizUser && !(ResetLogFromDb.eduUser))
                {
                    if (updateBiz(ResetLogFromDb.username, ResetPassword.Password))
                    {
                        await sendMailTo(ResetLogFromDb.username + "@braude.ac.il");
                        ResetLogFromDb.LogTypeId = 15;
                    }
                    else
                        ResetLogFromDb.LogTypeId = 14;
                }
                else if(!(ResetLogFromDb.bizUser) && ResetLogFromDb.eduUser)
                {
                    if (updateEdu(ResetLogFromDb.username, ResetPassword.Password))
                    {
                        await sendMailTo(ResetLogFromDb.username + "@s.braude.ac.il");
                        ResetLogFromDb.LogTypeId = 15;
                    }
                    else
                        ResetLogFromDb.LogTypeId = 14;
                }
                ResetLogFromDb.LogTypeId = 15;
                await _db.SaveChangesAsync();
                return RedirectToPage("/Reset/Done", new { id = ResetPassword.ResetID });
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("/Reset/Reset", new { id = ResetPassword.ResetID }); ;
        }
        public Boolean updateBiz(string username, string password)
        {
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain, "10.168.0.2", "OU=Administration,OU=BRDUsers,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);
                user.Enabled = true;
                //user.EmailAddress = password;
                user.SetPassword(password);
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
                PrincipalContext context2 = new PrincipalContext(ContextType.Domain, "10.168.130.10", "OU=edu,OU=BrdUsers,DC=brdeng,DC=ac", "ADSyncService", "9eV8H@G4z1XH");
                UserPrincipal user2 = UserPrincipal.FindByIdentity(context2, IdentityType.SamAccountName, username);
                user2.Enabled = true;
                //user2.EmailAddress = password;
                user2.SetPassword(password);
                user2.Save();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task sendMailTo(String email)
        {
            if (emailSend == true)
                return;
            else
                emailSend = true;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Password reset", "security@braude.ac.il"));
            message.To.Add(new MailboxAddress("Braude", email));
            message.Subject = "Your Password was reset!";
            message.Body = new TextPart("plain")
            {
                Text = "Your Password was reset"
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.office365.com", 587, false);
                client.Authenticate("security@braude.ac.il", "3D0w&rBo");
                await client.SendAsync(message);
                client.Disconnect(true);
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