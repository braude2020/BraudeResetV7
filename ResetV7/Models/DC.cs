using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Http;

namespace ResetV7.Models
{
    public class DC
    {
        public string bizDC = "10.168.0.2";
        public string eduDC = "10.168.130.10";
        public string activeUser = "ADSyncService";
        public string activeUserPass = "9eV8H@G4z1XH";
        public string bizOU = "OU=Administration,OU=BRDUsers,DC=BRD,DC=AC";
        public string eduOU = "OU=edu,OU=BrdUsers,DC=brdeng,DC=ac";
        public string bizGroup = "BizPasswordReset";

        public Boolean isServerAvailable(String server)
        {
            var ping = new Ping();
            var reply = ping.Send(server); // 1 minute time out (in ms)

            if (reply.Status == IPStatus.Success)
                return true;

            return false;
        }
        public Boolean isBizDcUp()
        {
            return isServerAvailable(bizDC);
        }
        public Boolean isEduDcUp()
        {
            return isServerAvailable(eduDC);
        }
        //public async Boolean isMailBitUp()
        //{
        //    HttpClient client = new HttpClient();
        //    var checkingResponse = await client.GetAsync("https://simplesms.co.il/");
        //    if (!checkingResponse.IsSuccessStatusCode)
        //    {
        //        return false;
        //    }




        //    return true;
        //}

    }
}
