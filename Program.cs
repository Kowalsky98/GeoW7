using System;
using System.Diagnostics;
using System.IO;
using System.Net; 

class Program
{
    static void Main(string[] args)
    {
        
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

        var config = new AppConfig();
        var systemService = new SystemService();
        var geoService = new GeoService();
        var directoryService = new DirectoryService(config);
        var alertService = new AlertService(config);

        try
        {
            string serial = systemService.GetSystemSerial();
            Console.WriteLine($"Serial del sistema: {serial}");

            string ip = geoService.GetIpAddress();
            Console.WriteLine($"IP: {ip}");

            var (latitude, longitude) = geoService.GetGeolocation(ip);
            Console.WriteLine($"Geo - Latitud: {latitude}, Longitud: {longitude}");

            var directories = directoryService.GetDirectoriesFromApi();
            bool foundInvalidDirectory = false;

            foreach (var (path, isAllowed) in directories)
            {
                if (!isAllowed && !string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    alertService.SendGeoAlert(serial, $"Alerta: {path}", latitude, longitude, true);
                    Console.WriteLine($"ALERTA: {path}");
                    foundInvalidDirectory = true;
                }
            }

            if (!foundInvalidDirectory)
            {
                alertService.SendGeoAlert(serial, "OK", latitude, longitude, false);
                Console.WriteLine("OK ");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }
}
