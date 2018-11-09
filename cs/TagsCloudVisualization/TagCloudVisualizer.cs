using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    public class TagCloudVisualizer
    {
        public static void Visualize(IEnumerable<Rectangle> layout, DirectoryInfo folder, string filename, 
            int imageWidth, int imageHeight)
        {
            var pen = new Pen(Color.White, 1);

            var halfImageWidth = imageWidth / 2;
            var halfImageHeight = imageHeight / 2;

            using (var bmp = new Bitmap(imageWidth, imageHeight))
            using (var g = Graphics.FromImage(bmp))
            {
                foreach (var tag in layout)
                {
                    var x = tag.X + halfImageWidth;
                    var y = tag.Y + halfImageHeight;

                    g.DrawRectangle(pen, x, y, tag.Width, tag.Height);
                }

                bmp.Save(Path.Combine(folder.FullName, filename + ".jpeg"), ImageFormat.Jpeg);
            }
        }
    }
}
