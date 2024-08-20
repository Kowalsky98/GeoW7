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
        return output.Split('\n')[1].Trim();
    }
}
