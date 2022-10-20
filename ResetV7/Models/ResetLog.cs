﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ResetV7.Models
{
    public class ResetLog
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Guid Id { get; set; }
        //[Key]
        //public int ResetID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ResetID { get; set; }

        public DateTime logTime { get; set; }
        [Required(ErrorMessage = "שם משתמש הוא שדה חובה"), Display(Name = "שם משתמש") ]
        public string username { get; set; }
        [RegularExpression(@"(?<!\d)\d{10}(?!\d)", ErrorMessage = "המספר הסלולרי שהוכנס אינו תקין")]
        [Required(ErrorMessage = "מספר טלפון נייד הוא שדה חובה"), MinLength(10), MaxLength(10), Display(Name = "מספר טלפון נייד")]

        public string mobile { get; set; }
        
        public int countReset { get; set; }
        public int countOTP { get; set; }
        public int countForgot { get; set; }
        public Boolean bizUser { get; set; }
        public Boolean eduUser { get; set; }
        public String Ip { get; set; }
        
        public string sessionToken { get; set; }
        //[RegularExpression(@"(?<!\d)\d{6}(?!\d)", ErrorMessage = "קוד אימות אינו תקין")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "קוד אימות אינו תקין")]
        [MinLength(6), MaxLength(6), Display(Name = "קוד אימות")]
        public string sessionTokenCheck { get; set; }
        
        public int LogTypeId { get; set; }

        [ForeignKey("LogTypeId")]
        public virtual LogType LogType { get; set; }

        
      
        
        
        public string generateToken()
        {
            string token = "";

            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                token = token + random.Next(0, 9);
            }


            return token;
        }
        private void testAD()
        {
            var ad = new PrincipalContext(ContextType.Domain, "192.168.0.2", "CN=BizPasswordReset,OU=BRDGroups,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
            var u = new UserPrincipal(ad) { SamAccountName = Environment.UserName };

            using (var search = new PrincipalSearcher(u))
            {
                var user = (UserPrincipal)search.FindOne();

                DirectoryEntry dirEntry = (DirectoryEntry)user.GetUnderlyingObject();
                string dept = dirEntry.Properties["Department"].Value.ToString();
                Console.WriteLine(dept);
            }
        }
        public int checkUser(string username, string mobile)
        {
            //0 - Bad
            //1 - no such user
            //2- Biz user
            //3 - EDU user
            //4 - BIZ EDU user
            //5 - not autorized

            int UserState = 0;

            //OU=edu,OU=BrdUsers,DC=brdeng,DC=ac

            //PrincipalContext contextEDU = new PrincipalContext(ContextType.Domain, "192.168.130.10", "OU=BrdUsers,DC=brdeng,DC=ac", "ADSyncService", "9eV8H@G4z1XH");
            PrincipalContext contextEDU = new PrincipalContext(ContextType.Domain, "192.168.130.10", "OU=edu,OU=BrdUsers,DC=brdeng,DC=ac", "ADSyncService", "9eV8H@G4z1XH");
            UserPrincipal userEDU = UserPrincipal.FindByIdentity(contextEDU, IdentityType.SamAccountName, username);

            PrincipalContext contextBIZ = new PrincipalContext(ContextType.Domain, "192.168.0.2", "OU=BRDUsers,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
            UserPrincipal userBIZ = UserPrincipal.FindByIdentity(contextBIZ, IdentityType.SamAccountName, username);

            if (userBIZ == null && userEDU == null)
                return 1;

            if (userBIZ != null && bizUserCheck(userBIZ, mobile))
            {
                if (bizGroupCheck(userBIZ))
                    UserState = 2;
                else
                    return 5;
            }
            else
                UserState = 1;
            if (userEDU != null && eduUserCheck(userEDU, mobile) && UserState != 5)
            {
                if (UserState == 2)
                    return 4;
                else
                    return 3;
            }



            return UserState;
        }

        private Boolean bizUserCheck(UserPrincipal userBIZ, string checkMobile)
        {
            //textBox4.AppendText("TEsting BIZ");
            PrincipalContext contextGroupBIZ = new PrincipalContext(ContextType.Domain, "192.168.0.2", "CN=BizPasswordReset,OU=BRDGroups,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
            DirectoryEntry directoryEntry = userBIZ.GetUnderlyingObject() as DirectoryEntry;

            String mobile = (Convert.ToString(directoryEntry.Properties["mobile"].Value));
            mobile = mobile.Replace("-", String.Empty);
            checkMobile = checkMobile.Replace("-", String.Empty);

            if (String.Equals(checkMobile, ""))
            {
                return false;
            }
            else if (String.Equals(checkMobile, mobile))
                return true;
            

            return false;
        }
        private Boolean bizGroupCheck(UserPrincipal userBIZ)
        {
            PrincipalContext contextGroupBIZ = new PrincipalContext(ContextType.Domain, "192.168.0.2", "CN=BizPasswordReset,OU=BRDGroups,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
            var directoryEntry = userBIZ.GetUnderlyingObject() as DirectoryEntry;

            GroupPrincipal group = GroupPrincipal.FindByIdentity(contextGroupBIZ, IdentityType.Name, "BizPasswordReset");

            foreach (Principal p in group.GetMembers(true))
            {
                if (p.SamAccountName == userBIZ.SamAccountName)
                {
                    //        //textBox4.AppendText("User in Group");
                    return true;
                }
            }


            return false;
        }
        private Boolean eduUserCheck(UserPrincipal userEDU, string checkMobile)
        {
            var directoryEntry = userEDU.GetUnderlyingObject() as DirectoryEntry;

            String mobile = (Convert.ToString(directoryEntry.Properties["mobile"].Value));
            mobile = mobile.Replace("-", String.Empty);
            checkMobile = checkMobile.Replace("-", String.Empty);

            if (String.Equals(checkMobile, ""))
            {
                return false;
            }
            else if (String.Equals(checkMobile, mobile))
                return true;

            return false;
        }


        public async void sendSMSBraude(String mobile, string token)
        {
            //WebRequest request = WebRequest.Create("https://localhost:44397/api/SmsMessage/BraudeReset/" + mobile + "/" + token);
            WebRequest request = WebRequest.Create("https://sms.braude.ac.il/api/SmsMessage/BraudeReset/" + mobile + "/" + token);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";


            string data = "";
            byte[] dataStream = Encoding.UTF8.GetBytes(data);

            request.ContentLength = dataStream.Length;
            Stream newStream = request.GetRequestStream();

            newStream.Write(dataStream, 0, dataStream.Length);
            newStream.Close();

            WebResponse response = request.GetResponse();
        }


        public async void sendSMS(String mobile, string token)
        {
            var httpClient = HttpClientFactory.Create();
            
            //var url = "http://simplesms.co.il/webservice/sendsmsws.asmx/SendSms?UserName=ravids&EncryptPassword=b6d670e996454d91966133c6ba82836f&Subscribers=" + mobile + "&Message=" + token + "&SenderName=Braude&DeliveryDelayInMinutes=0&ExpirationDelayInMinutes=120";

            var url = "https://simplesms.co.il/webservice/smsws.asmx/SendSms?UserName=ravids&EncryptPassword=b6d670e996454d91966133c6ba82836f&Subscribers=" + mobile + "&Message=" + token + "&SenderName=Braude&DeliveryDelayInMinutes=0&ExpirationDelayInMinutes=120&sendid=0";
            //var url = "https://213.8.243.103/webservice/smsws.asmx/SendSms?UserName=ravids&EncryptPassword=b6d670e996454d91966133c6ba82836f&Subscribers=" + mobile + "&Message=" + token + "&SenderName=Braude&DeliveryDelayInMinutes=0&ExpirationDelayInMinutes=120&sendid=0";

            //var url = "https://localhost:44397/api/SmsMessage/BraudeReset/" + mobile + "/" + token;

            //await httpClient.PostAsync()
            //HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(url);
            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //url = "https://simplesms.co.il/webservice/smsws.asmx/SendSms?UserName=ravids&EncryptPassword=b6d670e996454d91966133c6ba82836f&Subscribers=" + mobile + "&Message=" + token + "&SenderName=Braude&DeliveryDelayInMinutes=0&ExpirationDelayInMinutes=120&sendid=0";


                //WebRequest request = WebRequest.Create(url);
                //Stream rs = request.GetResponse().GetResponseStream();
                //StreamReader reader = new StreamReader(rs);


                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
                var test = httpResponseMessage;
            }
            catch(Exception ex)
            {
                var test = ex.Message;
                if(ex.InnerException != null)
                {
                    var test2 = ex.InnerException;
                }
            }
        }
        public Boolean isSessionStillValide(DateTime sessionStartTime)
        {
            if (sessionStartTime.AddMinutes(5) > System.DateTime.Now)
                return false;
            return true;
        }
    }
}