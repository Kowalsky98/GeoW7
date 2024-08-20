using Microsoft.Extensions.Configuration;
using System;

public class AppConfig
{
    public string AlertApiUrl { get; }
    public string DirectoriesApiUrl { get; }
    public string PostFilePath { get; }

    public AppConfig()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        AlertApiUrl = config["AlertApiUrl"] ?? throw new InvalidOperationException("AlertApiUrl cannot be null.");
        DirectoriesApiUrl = config["DirectoriesApiUrl"] ?? throw new InvalidOperationException("DirectoriesApiUrl cannot be null.");
        PostFilePath = config["PostFilePath"] ?? throw new InvalidOperationException("PostFilePath cannot be null.");
    }
}
