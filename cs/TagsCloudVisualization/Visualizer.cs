using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private int bitmapWidth;
        private int bitmapHeight;
        private Random random;

        public Visualizer(Random rnd, int bitmapWidth, int bitmapHeight)
        {
            this.bitmapWidth = bitmapWidth;
            this.bitmapHeight = bitmapHeight;
            this.random = rnd;
        }

        public byte[] CreateImageFromRectangles(Rectangle[] rectangles)
        {
            using (var bmp = new Bitmap(bitmapWidth, bitmapHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    foreach (var rectangle in rectangles)
                    {
                        var brush = new SolidBrush(CreateRandomColor());
                        g.FillRectangle(brush, rectangle);
                    }
                }

                var memStream = new MemoryStream();
                bmp.Save(memStream, ImageFormat.Jpeg);
                return memStream.ToArray();
            }
        }

        public byte[] CreateImageFromSizes(Size[] rectanglesSizes, ILayouter layouter)
        {
            var rectangles = rectanglesSizes.Select(layouter.PutNextRectangle).ToArray();
            return CreateImageFromRectangles(rectangles);
        }

        public byte[] CreateImageFromJPGS(string pathToFolder, ILayouter layouter)
        {
            var images = Directory.GetFiles(pathToFolder);
            images = images.OrderBy(x => random.Next()).ToArray();
            using (var bmp = new Bitmap(bitmapWidth, bitmapHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    foreach (var pathToImage in images)
                    {
                        var pictureBitmap = new Bitmap(pathToImage);
                        var rectangle = layouter.PutNextRectangle(pictureBitmap.Size);
                        g.DrawImage(pictureBitmap, rectangle);
                    }
                }

                var memStream = new MemoryStream();
                bmp.Save(memStream, ImageFormat.Jpeg);
                return memStream.ToArray();
            }
        }

        private void AddImageToGraphics(string pathToPicture, Graphics g)
        {
        }

        private Color CreateRandomColor()
        {
            const int minColor = 25;
            const int maxColor = 225;

            return Color.FromArgb(
                random.Next(minColor, maxColor), 
                random.Next(minColor, maxColor),
                random.Next(minColor, maxColor));
        }
    }
}