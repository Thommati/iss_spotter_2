using System.Net.Http;
using System.Threading.Tasks;

namespace iss_spotter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new();
            Iss iss = new(client);
            await iss.NextISSTimesForMyLocationAsync();
        }
    }
}
