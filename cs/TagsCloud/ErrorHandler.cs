using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace TagsCloudVisualization.Tests
{
    public class ErrorHandler : IImageFromTestSaver
    {
        private const string DateTimeFormat = "MM/dd/yy hh-mm-ss";
        private const string DirectoryName = "Error_Test_Images";

        public bool TrySaveImageToFile(string testName, Image image, out string path)
        {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            var fileName = DateTime.Now.ToString(DateTimeFormat) + $" {testName}.png";
            path = Path.Combine(DirectoryName, fileName);
            
            try
            {
                image.Save(path, ImageFormat.Png);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}