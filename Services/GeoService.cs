using System.Net.Http;
using Newtonsoft.Json.Linq;

public class GeoService
{
    private HttpClient _client = new HttpClient();
    private string _apiKey = "70d8fb8086de4c69a7e8227f7c3b31c9"; // Tu clave de API

    public string GetIpAddress()
    {
        var response = _client.GetAsync("https://api64.ipify.org?format=json").Result;
        response.EnsureSuccessStatusCode();
        var data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
        return data["ip"]?.ToString() ?? "Unknown";
    }

    public (float, float) GetGeolocation(string ipAddress)
    {
        var url = $"https://api.ipgeolocation.io/ipgeo?apiKey={_apiKey}&ip={ipAddress}";
        var response = _client.GetAsync(url).Result;
        response.EnsureSuccessStatusCode();
        var data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
        return (data["latitude"]?.Value<float>() ?? 0, data["longitude"]?.Value<float>() ?? 0);
    }
}
