using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualization
    {
        private readonly CircularCloudLayouter layouter;
        private Bitmap bitmap;
        private readonly int sideSize;

        public CircularCloudVisualization(CircularCloudLayouter layouter, int sideSize = 5)
        {
            this.layouter = layouter;
            this.sideSize = sideSize;
        }

        private void DrawRectanglesOnImage()
        {
            var layoutSize = layouter.GetLayoutSize();
            bitmap = new Bitmap(layoutSize.Width, layoutSize.Height);
            using var graphics = Graphics.FromImage(bitmap);
            using var pen = new Pen(Color.Red, sideSize);
            graphics.DrawRectangles(pen, layouter.ToArray());
        }

        public string SaveImage(string name)
        {
            if (!name.EndsWith(".png"))
                name += ".png";
            DrawRectanglesOnImage();
            var path = GetPathToImageName(name);
            bitmap.Save(path, ImageFormat.Png);
            bitmap.Dispose();
            return path;
        }

        private string GetPathToImageName(string name, int nestingInRootDirectory = 3)
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (var i = 0; i < nestingInRootDirectory; i++)
                directoryInfo = directoryInfo.Parent;
            return Path.Combine(directoryInfo.FullName, "LayoutImage", name);
        }
    }
}
