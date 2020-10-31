using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class Painter
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public Painter(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
        }

        public void PaintRectangle(Rectangle rectangle)
        {
            var rand = new Random(Environment.TickCount);
            if (rectangle.Left >= 30 && rectangle.Top >= 30 && rectangle.Right <= bitmap.Width - 30 &&
                rectangle.Bottom <= bitmap.Height - 30)
                graphics.DrawRectangle(
                    new Pen(Color.FromArgb(rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255)), 2),
                    rectangle);
            graphics.Save();
        }

        public void SavePicture(string fileName)
        {
            bitmap.Save(fileName, ImageFormat.Jpeg);
        }
    }
}