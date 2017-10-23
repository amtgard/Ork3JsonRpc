using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrk3JsonRpc0
{
    class Program
    {
        static void Main(string[] args)
        {
            Ork3.Ork3JsonRpc rpc = new Ork3.Ork3JsonRpc();

            rpc.Authorize("user", "password");

            Console.WriteLine("KINGDOMS");

            dynamic kingdoms = rpc.GetKingdoms();
            foreach (dynamic kingdom in kingdoms)
            {
                Console.WriteLine(kingdom.Value.KingdomName);
            }

            Console.WriteLine("\nPARKS");

            dynamic response = rpc.JsonRpc("Kingdom/GetParks", new Dictionary<string, string>(){ { "KingdomId", "16" } });
            foreach (dynamic park in response.Parks)
            {
                Console.WriteLine(park.Name + " (" + park.Abbreviation + ")");
            }
            Console.ReadKey();
        }
    }
}
