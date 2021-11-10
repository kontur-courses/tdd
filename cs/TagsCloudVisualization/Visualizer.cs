using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private ILayouter layouter;
        private int bitmapWidth;
        private int bitmapHeight;
        private Random random;

        public Visualizer(ILayouter layouter, Random rnd, int bitmapWidth, int bitmapHeight)
        {
            this.layouter = layouter;
            this.bitmapWidth = bitmapWidth;
            this.bitmapHeight = bitmapHeight;
            this.random = rnd;
        }

        public byte[] CreateImageFromRectangles(Size[] rectanglesSizes)
        {
            using (var bmp = new Bitmap(bitmapWidth, bitmapHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    foreach (var size in rectanglesSizes)
                    {
                        var brush = new SolidBrush(CreateRandomColor());
                        var rectangle = layouter.PutNextRectangle(size);
                        g.FillRectangle(brush, rectangle);
                    }
                }

                var memStream = new MemoryStream();
                bmp.Save(memStream, ImageFormat.Jpeg);
                return memStream.ToArray();
            }
        }

        public byte[] CreateImageFromJPGS(string pathToFolder)
        {
            var pictures = Directory.GetFiles(pathToFolder);
            pictures = pictures.OrderBy(x => random.Next()).ToArray();
            using (var bmp = new Bitmap(bitmapWidth, bitmapHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    foreach (var picture in pictures)
                    {
                        AddImageToGraphics(picture, g);
                    }
                }

                var memStream = new MemoryStream();
                bmp.Save(memStream, ImageFormat.Jpeg);
                return memStream.ToArray();
            }
        }

        private void AddImageToGraphics(string pathToPicture, Graphics g)
        {
            var bitmap = new Bitmap(pathToPicture);
            var size = bitmap.Size;
            var rectangle = layouter.PutNextRectangle(size);
            g.DrawImage(bitmap, rectangle);
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