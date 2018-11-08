using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Visualisator
    {
        private Bitmap bitmap;
        private Graphics graphics;
        private Pen pen;
        private Point center;

        public Visualisator(Size bitmapSize)
        {
            bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            graphics = Graphics.FromImage(bitmap);
            pen = new Pen(Color.Black);
            center = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);
        }

        public void ShowCurrentConfig(CircularCloudLayouter cloud, string imageName)
        {
            foreach (var rect in cloud.rectangles)
            {
                var replacedRect = new Rectangle(rect.Location.X + center.X,
                    rect.Location.Y + center.Y, rect.Size.Width, rect.Height);
                graphics.DrawRectangle(pen, replacedRect);
            }
            bitmap.Save(@"D:/newFile.bmp");
        }

        public void MakeImage(string imageName, int rectangleCount)
        {
            var cloud = new CircularCloudLayouter(center, 0.5);
            var rectangles = new List<Rectangle>();
            var random = new Random();
            for (var i = 0; i < rectangleCount; i++)
            {
                var width = (int)(random.NextDouble() * 50);
                var height = (int)(random.NextDouble() * 20);
                rectangles.Add(cloud.PutNextRectangle(new Size(width, height)));
            }
            graphics.DrawRectangles(pen, rectangles.ToArray());
            bitmap.Save(imageName);
        }
    }
}
