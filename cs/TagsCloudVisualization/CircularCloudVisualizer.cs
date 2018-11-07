using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    class CircularCloudVisualizer
    {
        public static void DrawTags(IEnumerable<Rectangle> rectangles, DirectoryInfo folder, string filename, 
            int imageWidth, int imageHeight)
        {
            var bmp = new Bitmap(imageWidth, imageHeight);
            var pen = new Pen(Color.White, 1);

            var halfImageWidth = imageWidth / 2;
            var halfImageHeight = imageHeight / 2;

            using (var g = Graphics.FromImage(bmp))
            {
                foreach (var r in rectangles)
                {
                    var x = r.X + halfImageWidth;
                    var y = r.Y + halfImageHeight;

                    g.DrawRectangle(pen, x, y, r.Width, r.Height);
                }
            }

            bmp.Save(Path.Combine(folder.FullName, filename + ".jpeg"), ImageFormat.Jpeg);
        }
    }
}
