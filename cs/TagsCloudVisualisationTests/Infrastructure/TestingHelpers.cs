using System.Drawing;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudVisualisationTests.Infrastructure
{
    public static class TestingHelpers
    {
        public static string BaseOutputFilePath =>
            CreateDirectory(Path.Combine(CreateDirectory(TestContext.CurrentContext.WorkDirectory), "tests-output"));

        public static string GetOutputDirectory(string directoryName) =>
            CreateDirectory(Path.Combine(BaseOutputFilePath, directoryName));

        public static FileInfo GetTestFile(string fileName) =>
            new FileInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestData", fileName));

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

        public static Color RandomColor => Color.FromKnownColor(Randomizer.CreateRandomizer().NextEnum<KnownColor>());
    }
}