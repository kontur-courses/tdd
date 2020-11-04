using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class BitmapSaver
    {
        public static void Save(Bitmap bitmap, string filename)
        {
            var projectDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            bitmap.Save(Path.Combine(projectDirectory.FullName, "pictures", filename));
        }
    }
}