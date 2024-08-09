using System.Diagnostics;

public class SystemService
{
    public string GetSystemSerial()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = "bios get serialnumber",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        var lines = output.Split('\n');
        return lines.Length > 1 ? lines[1].Trim() : "Unknown";
    }
}
