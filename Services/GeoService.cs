using System.Net.Http;
using Newtonsoft.Json.Linq;

public class GeoService
{
    private HttpClient _client = new HttpClient();

    public string GetIpAddress()
    {
        var response = _client.GetAsync("https://api64.ipify.org?format=json").Result;
        response.EnsureSuccessStatusCode();
        var data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
        return data["ip"]?.ToString() ?? "Unknown";
    }

    public (float, float) GetGeolocation(string ipAddress)
    {
        var response = _client.GetAsync($"https://freegeoip.app/json/{ipAddress}").Result;
        response.EnsureSuccessStatusCode();
        var data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
        return (data["latitude"]?.Value<float>() ?? 0, data["longitude"]?.Value<float>() ?? 0);
    }
}
