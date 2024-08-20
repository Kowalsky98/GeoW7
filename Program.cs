using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
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
            bool foundNotAllowed = false;
            bool missingAllowed = false;

            foreach (var (path, isAllow) in directories)
            {
                bool existsAsFile = File.Exists(path);
                bool existsAsDirectory = Directory.Exists(path);

                if (isAllow)
                {
                    if (!existsAsFile && !existsAsDirectory)
                    {
                        Console.WriteLine($"Directorio permitido faltante: {path}");
                        missingAllowed = true;
                    }
                }
                else
                {
                    if (existsAsFile || existsAsDirectory)
                    {
                        alertService.SendGeoAlert(serial, $"Alerta: {path}", latitude, longitude, true);
                        Console.WriteLine($"Alerta enviada para: {path}");
                        foundNotAllowed = true;
                    }
                }
            }

            if (missingAllowed)
            {
                ExecutePostExe(config.PostFilePath);
            }

            if (!foundNotAllowed && !missingAllowed)
            {
                alertService.SendGeoAlert(serial, "todo bien", latitude, longitude, false);
                Console.WriteLine("todo bien.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }

    static void ExecutePostExe(string postFilePath)
    {
        try
        {
            Console.WriteLine($"Ejecutando {postFilePath}...");
            Process.Start(postFilePath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error al ejecutar {postFilePath}: {e.Message}");
        }
    }
}
