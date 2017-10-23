using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;

namespace Ork3
{
    public class Ork3JsonRpc
    {

        public string Endpoint { get; set; }
        public string Token { get; set; }
        public Ork3JsonRpc()
        {
            Endpoint = "https://esdraelon.amtgard.com/ork/orkservice/Json/index.php?";
        }

        public bool Authorize(string UserName, string Password)
        {
            dynamic response = JsonRpc("Authorization/Authorize", new Dictionary<string, string>() { { "UserName", UserName }, { "Password", Password } });
            if (response.Status.Status == 0)
            {
                Token = response.Token;
                return true;
            }
            return false;
        }

        public dynamic GetKingdoms()
        {
            return JsonRpc("Kingdom/GetKingdoms", new Dictionary<string, string>()).Kingdoms;
        }

        public dynamic GetParks(int KingdomId)
        {
            return JsonRpc("Kingdom/GetParks", new Dictionary<string, string>() { { "KingdomId", KingdomId.ToString() } }).Parks;
        }

        public dynamic SearchMundanes(int KingdomId, int ParkId, string Term)
        {
            return JsonRpc("SearchService/Player", new Dictionary<string, string>() { { "kingdom_id", KingdomId.ToString() }, { "park_id", ParkId.ToString() }, { "search", Term } }).Result;
        }

        public dynamic AddAttendance(int MundaneId, string Persona, int ClassId, DateTime Date, int Credits, string Flavor, string Note, int ParkId, int EventCalendarDetailId)
        {
            return JsonRpc("Attendance/AddAttendance", new Dictionary<string, string>()
            {
                { "Token", Token },
                { "MundaneId", MundaneId.ToString() },
                { "Persona", Persona },
                { "ClassId", ClassId.ToString() },
                { "Date", Date.ToString("yyyy-MM-dd") },
                { "Credits", Credits.ToString() },
                { "Flavor", Flavor },
                { "Note", "" },
                { "ParkId", ParkId.ToString() },
                { "EventCalendarDetailId", "" }
            });
        }

        public string GetClasses()
        {
            throw new Exception("https://github.com/amtgard/orkmobile/blob/master/tpl/parkattendance.html");
        }

        public dynamic JsonRpc(string Call, Dictionary<string, string> Parameters)
        {
            List<string> param = new List<string>();
            foreach (KeyValuePair<string, string> p in Parameters) {
                param.Add("request[" + p.Key + "]=" + p.Value);
            }
            string geturl = Endpoint + "call=" + Call + "&" + (param.Count == 0 ? "request=" : string.Join("&", param));

            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(geturl);
            System.Net.HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return System.Web.Helpers.Json.Decode(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }
    }
}
