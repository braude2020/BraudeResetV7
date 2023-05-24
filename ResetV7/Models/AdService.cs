using System.DirectoryServices.AccountManagement;
using System;
using System.DirectoryServices;
using Microsoft.Extensions.Options;

namespace ResetV7.Models
{
    public static class AdService
    {
        public static Boolean updateBiz(string username, string password, IOptions<ADServer> _adServer)
        {
            try
            {
                //PrincipalContext context = new PrincipalContext(ContextType.Domain, "192.168.0.2", "OU=Administration,OU=BRDUsers,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
                //PrincipalContext context = new PrincipalContext(ContextType.Domain, "10.168.0.2", "OU=BRDUsers,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
                PrincipalContext context = new PrincipalContext(ContextType.Domain, _adServer.Value.BizAddress, _adServer.Value.BizOu, _adServer.Value.BizUser, _adServer.Value.BizPasswd);


                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);
                user.Enabled = true;
                //user.EmailAddress = password;
                try
                {
                    user.SetPassword(password);
                }
                catch (Exception ex)
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
        public static Boolean updateEdu(string username, string password, IOptions<ADServer> _adServer)
        {
            try
            {
                //OU=edu,OU=BrdUsers,DC=brdeng,DC=ac
                //PrincipalContext context2 = new PrincipalContext(ContextType.Domain, "192.168.130.10", "OU=BrdUsers,DC=brdeng,DC=ac", "ADSyncService", "9eV8H@G4z1XH");
                //PrincipalContext context2 = new PrincipalContext(ContextType.Domain, "10.168.130.10", "OU=edu,OU=BrdUsers,DC=brdeng,DC=ac", "ADSyncService", "9eV8H@G4z1XH");
                PrincipalContext context2 = new PrincipalContext(ContextType.Domain, _adServer.Value.EduAddress, _adServer.Value.EduOu, _adServer.Value.EduUser, _adServer.Value.EduPasswd);

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
        //private static void testAD(IOptions<ADServer> _adServer)
        //{


        //    //var ad = new PrincipalContext(ContextType.Domain, "10.168.0.2", "CN=BizPasswordReset,OU=BRDGroups,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
        //    var ad = new PrincipalContext(ContextType.Domain, "10.168.0.2", "CN=BizPasswordReset,OU=BRDGroups,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");


        //    var u = new UserPrincipal(ad) { SamAccountName = Environment.UserName };

        //    using (var search = new PrincipalSearcher(u))
        //    {
        //        var user = (UserPrincipal)search.FindOne();

        //        DirectoryEntry dirEntry = (DirectoryEntry)user.GetUnderlyingObject();
        //        string dept = dirEntry.Properties["Department"].Value.ToString();
        //        Console.WriteLine(dept);
        //    }
        //}

        public static int checkUser(string username, string mobile, IOptions<ADServer> _adServer)
        {
            //0 - Bad
            //1 - no such user
            //2- Biz user
            //3 - EDU user
            //4 - BIZ EDU user
            //5 - not autorized

            int UserState = 0;
            //PrincipalContext contextEDU = new PrincipalContext(ContextType.Domain, "10.168.130.10", "OU=edu,OU=BrdUsers,DC=brdeng,DC=ac", "ADSyncService", "9eV8H@G4z1XH");
            PrincipalContext contextEDU = new PrincipalContext(ContextType.Domain, _adServer.Value.EduAddress, _adServer.Value.EduOu, _adServer.Value.EduUser, _adServer.Value.EduPasswd);

            UserPrincipal userEDU = UserPrincipal.FindByIdentity(contextEDU, IdentityType.SamAccountName, username);

            //PrincipalContext contextBIZ = new PrincipalContext(ContextType.Domain, "10.168.0.2", "OU=BRDUsers,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
            PrincipalContext contextBIZ = new PrincipalContext(ContextType.Domain, _adServer.Value.BizAddress, _adServer.Value.BizOu, _adServer.Value.BizUser, _adServer.Value.BizPasswd);

            UserPrincipal userBIZ = UserPrincipal.FindByIdentity(contextBIZ, IdentityType.SamAccountName, username);

            if (userBIZ == null && userEDU == null)
                return 1;

            if (userBIZ != null && bizUserCheck(userBIZ, mobile, _adServer))
            {
                if (bizGroupCheck(userBIZ, _adServer))
                    UserState = 2;
                else
                    return 5;
            }
            else
                UserState = 1;
            if (userEDU != null && eduUserCheck(userEDU, mobile, _adServer  ) && UserState != 5)
            {
                if (UserState == 2)
                    return 4;
                else
                    return 3;
            }



            return UserState;
        }

        private static Boolean bizUserCheck(UserPrincipal userBIZ, string checkMobile, IOptions<ADServer> _adServer)
        {
            //PrincipalContext contextGroupBIZ = new PrincipalContext(ContextType.Domain, "10.168.0.2", "CN=BizPasswordReset,OU=BRDGroups,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
            //PrincipalContext contextBIZ = new PrincipalContext(ContextType.Domain, _adServer.Value.BizAddress, _adServer.Value.BizOu, _adServer.Value.BizUser, _adServer.Value.BizPasswd);
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
        private static Boolean bizGroupCheck(UserPrincipal userBIZ, IOptions<ADServer> _adServer)
        {
            //PrincipalContext contextGroupBIZ = new PrincipalContext(ContextType.Domain, "10.168.0.2", "CN=BizPasswordReset,OU=BRDGroups,DC=BRD,DC=AC", "ADSyncService", "9eV8H@G4z1XH");
            PrincipalContext contextGroupBIZ = new PrincipalContext(ContextType.Domain, _adServer.Value.BizAddress, _adServer.Value.BizGroupOU, _adServer.Value.BizUser, _adServer.Value.BizPasswd);

            var directoryEntry = userBIZ.GetUnderlyingObject() as DirectoryEntry;

            GroupPrincipal group = GroupPrincipal.FindByIdentity(contextGroupBIZ, IdentityType.Name, _adServer.Value.BizGroup);

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
        private static Boolean eduUserCheck(UserPrincipal userEDU, string checkMobile, IOptions<ADServer> _adServer)
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
    }
}
