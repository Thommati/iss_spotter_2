using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace iss_spotter
{
    public class Iss
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        public Iss(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private async Task<IP> FetchMyIpAsync()
        {
            Uri ipUri = new("https://api.ipify.org/?format=json");
            string responseBody = await httpClient.GetStringAsync(ipUri);
            return JsonSerializer.Deserialize<IP>(responseBody, options);
        }

        private async Task<Coordinates> FetchCoordsByIPAsync(IP ip)
        {
            Uri locationUri = new($"https://freegeoip.app/json/{ip.Ip}");
            string responseBody = await httpClient.GetStringAsync(locationUri);
            return JsonSerializer.Deserialize<Coordinates>(responseBody, options);
        }

        private async Task<ISSPassTimes> FetchISSFlyOverTimes(Coordinates coords)
        {
            Uri flyoverUri = new($"http://api.open-notify.org/iss/v1/?lat={coords.Latitude}&lon={coords.Longitude}&alt=1650");
            string responseBody = await httpClient.GetStringAsync(flyoverUri);
            return JsonSerializer.Deserialize<ISSPassTimes>(responseBody, options);
        }

        public async Task NextISSTimesForMyLocationAsync()
        {
            try
            {
                IP ip = await FetchMyIpAsync();
                Coordinates coords = await FetchCoordsByIPAsync(ip);
                ISSPassTimes passTimes = await FetchISSFlyOverTimes(coords);
                passTimes.PrintPassTimes();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Something went wrong:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
