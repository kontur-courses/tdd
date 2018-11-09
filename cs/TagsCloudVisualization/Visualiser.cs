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
    class Visualiser
    {
        private readonly Size bitmapSize;
        private readonly Pen pen;
        private readonly Point center;

        public Visualiser(Size bitmapSize)
        {
            this.bitmapSize = bitmapSize;
            pen = new Pen(Color.Black);
            center = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);
        }

        public void ShowCurrentConfig(CircularCloudLayouter cloud, string imageName)
        {
            Bitmap bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            foreach (var rect in cloud.rectangles)
            {
                var replacedRect = new Rectangle(rect.Location.X + center.X,
                    rect.Location.Y + center.Y, rect.Size.Width, rect.Height);
                graphics.DrawRectangle(pen, replacedRect);
            }
            bitmap.Save(imageName);
        }

        private Size GetNewRectangleSize(Random random)
        {
            var minWidth = 10;
            var minHeight = 5;
            var rectangleSize = new Size();
            rectangleSize.Width = minWidth + (int) (random.NextDouble() * 50);
            rectangleSize.Height = minHeight + (int)(random.NextDouble() * 20);
            return rectangleSize;
        }

        public void MakeExampleImage(string imageName, int rectangleCount, Size maxRectangleSize)
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0), 0.5);
            var random = new Random();
            for (var i = 0; i < rectangleCount; i++)
            {
                var currentRectangleSize = GetNewRectangleSize(random);
                cloud.PutNextRectangle(currentRectangleSize);
            }
            ShowCurrentConfig(cloud, imageName);
        }
    }
}
