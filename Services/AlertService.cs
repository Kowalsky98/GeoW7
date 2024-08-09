using System.Net.Http;
using Newtonsoft.Json;

public class AlertService
{
    private HttpClient _client = new HttpClient();
    private AppConfig _config;

    public AlertService(AppConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public void SendGeoAlert(string serial, string alertType, float latitude, float longitude, bool alertStatus)
    {
        var payload = new
        {
            serial = serial,
            alert = alertStatus,
            alertType = alertType,
            latitude = latitude,
            longitude = longitude
        };
        var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");
        var response = _client.PostAsync(_config.AlertApiUrl, content).Result;
        response.EnsureSuccessStatusCode();
    }
}
