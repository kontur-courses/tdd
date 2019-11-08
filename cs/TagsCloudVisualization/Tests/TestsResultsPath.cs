using System;
using System.IO;
using System.Reflection;

namespace TagsCloudVisualization.Tests
{
    internal static class TestsResultsPath
    {
        public static string GetPathToFailedTestResult(string className, string testName, DateTime testTime)
        {
            var directorySeparator = Path.DirectorySeparatorChar;
            var path = Environment.CurrentDirectory + directorySeparator +
                "TestsResults" + directorySeparator +
                "Failed" + directorySeparator +
                Assembly.GetExecutingAssembly().GetName().Name + directorySeparator +
                className + directorySeparator +
                testTime.ToString("dd.MM.yyyy") + directorySeparator;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path += testTime.ToString("HH-mm-ss") + "_" + testName;
            return path;
        }
    }
}
