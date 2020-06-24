using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;

namespace ResetV7.Pages.Logging
{
    public class SendMailModel : PageModel
    {
        public async Task OnGet()
        {
            
                await sendMailTo("netanel@braude.ac.il");
            
            //sendMailTo("amitro@braude.ac.il");
        }
        public async Task sendMailTo(String email)
        {
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
    }
}