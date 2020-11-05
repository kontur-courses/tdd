using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class BitmapSaver
    {
        public const string directoryToSave = "pictures";

        public static void Save(Bitmap bitmap, string filename)
        {
            var projectDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            bitmap.Save(Path.Combine(projectDirectory.FullName, directoryToSave, filename));
        }

        public static string GetRelativePath(string filename) =>
            Path.Combine(directoryToSave, filename);
    }
}