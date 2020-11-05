using System.IO;
using NUnit.Framework;

namespace TagsCloudVisualisationTests.Infrastructure
{
    public static class TestingHelpers
    {
        public static string BaseOutputFilePath =>
            CreateDirectory(Path.Combine(CreateDirectory(TestContext.CurrentContext.WorkDirectory), "tests-output"));

        public static string GetOutputDirectory(string directoryName) =>
            CreateDirectory(Path.Combine(BaseOutputFilePath, directoryName));

        public static void ClearDirectory(string fullPath)
        {
            if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, true);
            CreateDirectory(fullPath);
        }

        private static string CreateDirectory(string fullPath)
        {
            Directory.CreateDirectory(fullPath);
            return fullPath;
        }
    }
}