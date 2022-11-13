using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TagCloud;

namespace TagCloudUnitTests
{
    public static class ErrorTestImageSaver
    {
        private const string FileNameDateFormat = "yy-MM-dd hh-mm-ss";

        private static readonly DirectoryInfo TestsLogDirectory;

        static ErrorTestImageSaver()
        {
            TestsLogDirectory = GetTestsLogDirectory();
        }

        public static void SaveBitmap(Bitmap bitmap, out string fullPath)
        {
            var subTestDirectory = GetSubTestDirectory();

            var fileName = DateTime.Now.ToString(FileNameDateFormat) + ".png";

            fullPath = Path.Combine(subTestDirectory.FullName, fileName);

            ImageSaver.SaveBitmap(bitmap, fullPath);
        }

        private static DirectoryInfo GetTestsLogDirectory()
        {
            var currentDirectory = Environment.CurrentDirectory;

            var directoryToSaveImage = Path.Combine(currentDirectory, "TestsLog");

            if (!Directory.Exists(directoryToSaveImage))
                return Directory.CreateDirectory(directoryToSaveImage);

            return new DirectoryInfo(directoryToSaveImage);
        }

        private static DirectoryInfo GetSubTestDirectory()
        {
            var currentTestName = TestContext.CurrentContext.Test.Name;

            return TestsLogDirectory.CreateSubdirectory(currentTestName);
        }
    }
}
