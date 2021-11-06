using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagCloud.Saving
{
    public class BitmapToDesktopSaver : IBitmapToDesktopSaver
    {
        private readonly string fileNamePrefix;
        private readonly ImageFormat imgFormat = ImageFormat.Png;
        private readonly string saveDirectory;

        public BitmapToDesktopSaver()
        {
            saveDirectory = GetSaveDirectory();
            fileNamePrefix = GetFileNamePrefix();
        }

        public void Save(Bitmap bitmap, bool openAfterSave)
        {
            var fullFileName = GetFileFullName();

            bitmap.Save(fullFileName, imgFormat);

            if (openAfterSave)
                OpenImage(fullFileName);
        }

        private string GetSaveDirectory()
        {
            var desktopPathSuffix = Path.Combine("desktop", "layouts");
            var currentUserDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(currentUserDir, desktopPathSuffix);
        }

        private static string GetFileNamePrefix()
        {
            return "TagCloud_";
        }

        private string GetFileFullName()
        {
            var currentTime = DateTime.Now.ToString("hh-mm-ss-fff");
            var fileName = string.Join("", fileNamePrefix, currentTime, ".png");
            return Path.Combine(saveDirectory, fileName);
        }

        private static void OpenImage(string fullFileName)
        {
            Process.Start(fullFileName);
        }
    }
}