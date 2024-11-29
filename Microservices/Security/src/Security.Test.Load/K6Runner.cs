using System.Diagnostics;

namespace Security.Test.Load;


public static class K6Runner
{
    public static void RunK6Test()
    {
        // Full path to the k6 executable
        var k6Path = @"C:\Program Files\k6\k6.exe";

        // Full path to the test script
        var testScript = @"C:\Projects\Libraries\Microservices\Security\src\Security.Test.Load\K6Tests\login-loadtest.js";

        // Directory to store the test results
        var resultDirectory = @"C:\Projects\Libraries\Microservices\Security\src\Security.Test.Load\ResultTest";

        // Ensure the ResultTest directory exists
        Directory.CreateDirectory(resultDirectory);

        // Generate the output result file path
        var resultFile = Path.Combine(resultDirectory, "k6-test-result.json");

        // Build the start info for k6
        var startInfo = new ProcessStartInfo
        {
            FileName = k6Path,
            Arguments = $"run \"{testScript}\" --out json=\"{resultFile}\"", // Store the results in JSON format
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        if (process != null)
        {
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output); // Print the output of the test run
            process.WaitForExit();
        }
    }
}

