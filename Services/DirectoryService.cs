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

    public List<(string path, bool isAllowed)> GetDirectoriesFromApi()
    {
        var response = _client.GetAsync(_config.DirectoriesApiUrl).Result;
        response.EnsureSuccessStatusCode();
        var jsonResponse = response.Content.ReadAsStringAsync().Result;

        JObject jsonObject = JObject.Parse(jsonResponse);
        JArray items = (JArray)jsonObject["items"];

        if (items == null) {
            throw new InvalidOperationException("The 'items' JSON array is null or not present in the response.");
        }

        var directories = new List<(string path, bool isAllowed)>();
        foreach (var item in items)
        {
            string path = item["path"].ToString().Replace("\\\\", "\\");
            bool isAllowed = (bool)item["isAllow"];
            
            if (File.Exists(path) || Directory.Exists(path))
            {
                directories.Add((path, isAllowed));
            }
        }
        return directories;
    }
}
