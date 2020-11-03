using System.Drawing;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudVisualisationTests.Infrastructure
{
    public static class TestingHelpers
    {
        public static string BaseOutputFilePath =>
            EnsureDirectory(Path.Combine(EnsureDirectory(TestContext.CurrentContext.WorkDirectory), "tests-output"));

        public static string GetOutputDirectory(string directoryName) =>
            EnsureDirectory(Path.Combine(BaseOutputFilePath, directoryName));

        public static void ClearDirectory(string fullPath)
        {
            if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, true);
            Directory.CreateDirectory(fullPath);
        }

        private static string EnsureDirectory(string fullPath)
        {
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            return fullPath;
        }

        public static Color RandomColor => Color.FromKnownColor(Randomizer.CreateRandomizer().NextEnum<KnownColor>());
    }
}