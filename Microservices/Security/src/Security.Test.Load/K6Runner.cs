using System.Diagnostics;

namespace Security.Test.Load;

public static class K6Runner
{
    public static void RunK6Test()
    {
        var k6Path = @"C:\Program Files\k6\k6.exe";
        var testScript =
            @"C:\Projects\Libraries\Microservices\Security\src\Security.Test.Load\K6Tests\login-stress-test.js";
        var resultDirectory = @"C:\Projects\Libraries\Microservices\Security\src\Security.Test.Load\ResultTest";

        Directory.CreateDirectory(resultDirectory);
        var resultFile = Path.Combine(resultDirectory, "k6-test-result.json");

        var startInfo = new ProcessStartInfo
        {
            FileName = k6Path,
            Arguments = $"run \"{testScript}\" --out json=\"{resultFile}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        if (process == null) return;
        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
        process.WaitForExit();
    }
}