using System;
using System.IO;
using System.Reflection;

namespace TagsCloudVisualization.Tests
{
    internal static class TestsResultsPath
    {
        public static string GetPathToFailedTestResult(string className, string testName, DateTime testTime)
        {
            var directorySeparator = Path.DirectorySeparatorChar.ToString();
            var path = string.Join(
                directorySeparator,
                Environment.CurrentDirectory,
                "TestsResults",
                "Failed",
                Assembly.GetExecutingAssembly().GetName().Name,
                className,
                testTime.ToString("dd.MM.yyyy"));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return $"{path}{directorySeparator}{testTime:HH-mm-ss}_{testName}";
        }
    }
}
