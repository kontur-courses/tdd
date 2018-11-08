using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class CloudVisualizer
    {
        public static Bitmap CreateImage(CircularCloudLayouter layouter)
        {
            var width = layouter.Rectangles.Max(r => r.Right) - layouter.Rectangles.Min(r => r.Left);
            var height = layouter.Rectangles.Max(r => r.Bottom) - layouter.Rectangles.Min(r => r.Top);
            var image = new Bitmap(width * 2, height * 2);
            var graphics = Graphics.FromImage(image);
            var pen = new Pen(Color.Red, 2);
            var newCenter = new Point(image.Width / 2, image.Height / 2);

            foreach (var rectangle in layouter.Rectangles)
            {
                var movedRectangle = new Rectangle(
                    rectangle.X + newCenter.X, rectangle.Y + newCenter.Y, rectangle.Width, rectangle.Height);
                graphics.DrawRectangle(pen, movedRectangle);
            }

            image.Save(@"D:\image.bmp");
            return image;
        }
    }
}
