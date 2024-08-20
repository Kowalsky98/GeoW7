using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO;

public class DirectoryService
{
    private HttpClient _client = new HttpClient();
    private readonly AppConfig _config;

    public DirectoryService(AppConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public List<(string path, bool isAllow)> GetDirectoriesFromApi()
    {
        var response = _client.GetAsync(_config.DirectoriesApiUrl).Result;
        response.EnsureSuccessStatusCode();
        var jsonResponse = response.Content.ReadAsStringAsync().Result;

        JObject jsonObject = JObject.Parse(jsonResponse);
        JArray rows = jsonObject["rows"] as JArray; 

        if (rows == null || !rows.HasValues)
        {
            throw new InvalidOperationException("The 'rows' JSON array is null, empty, or not present in the response.");
        }

        var directories = new List<(string path, bool isAllow)>();
        foreach (var row in rows)
        {
            string path = row["path"]?.ToString().Replace("\\\\", "\\") ?? string.Empty;
            bool isAllow = row["isAllow"]?.Value<bool>() ?? false;
            
            if (!string.IsNullOrEmpty(path))
            {
                directories.Add((path, isAllow));
            }
        }

        return directories;
    }
}
