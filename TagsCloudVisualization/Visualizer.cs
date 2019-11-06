using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Visualizer
    {
        private static Random rnd = new Random();
        public static Bitmap GetCloudVisualization(CircularCloudLayouter cloud)
        {
            var imgRectangle = GetImageRectangle(cloud);
            var bitmap = new Bitmap(imgRectangle.Width, imgRectangle.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TranslateTransform(-imgRectangle.X, -imgRectangle.Y);
                graphics.DrawRectangles(new Pen(GetRandomColor(), rnd.Next(3,5)), cloud.Rectangles.ToArray());
            }

            return bitmap;
        }

        private static Rectangle GetImageRectangle(CircularCloudLayouter cloud)
        {
            var minX = cloud.Rectangles.Min(rect => rect.Left);
            var minY = cloud.Rectangles.Min(rect => rect.Top);
            var maxX = cloud.Rectangles.Max(rect => rect.Right);
            var maxY = cloud.Rectangles.Max(rect => rect.Bottom);
            return new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
        }
        private static Color GetRandomColor()
        {
             return Color.FromArgb(rnd.Next(50,256),
                 rnd.Next(50, 256), rnd.Next(50, 256));
        }
    }
}
