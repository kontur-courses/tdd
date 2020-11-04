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

        public CircularCloudVisualization(CircularCloudLayouter layouter)
        {
            this.layouter = layouter;
        }

        public void DrawRectanglesOnImage()
        {
            var layoutSize = layouter.GetLayoutSize();
            bitmap = new Bitmap(layoutSize.Width, layoutSize.Height);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Red, 5);
            graphics.DrawRectangles(pen, layouter.ToArray());
            graphics.Dispose();
        }

        public void SaveImage(string name)
        {
            if (!name.EndsWith(".png"))
                name += ".png";
            var path = GetPathToImageName(name);
            bitmap.Save(path, ImageFormat.Png);
            bitmap.Dispose();
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
