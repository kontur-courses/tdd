using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    static class Program
    {
        private const string Filename =
            @"C:\Users\Night\Desktop\Languages\C#\Shpora\2.tdd\cs\TagsCloudVisualization\\image.bmp";
        private static readonly Size bitmapSize = new Size(800, 600);
        private static readonly Color backgroundColor = Color.FromArgb(0, 34, 43);

        private static Bitmap CreateBitmap(IReadOnlyList<Rectangle> rectangles, Size bitmapSize)
        {
            Bitmap bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            
            graphics.FillRectangle(new SolidBrush(backgroundColor), new Rectangle(Point.Empty, bitmapSize));

            for (int i = 0; i < rectangles.Count; i++)
            {
                var brash = i < 1 ? Brushes.Black : i % 2 == 0 ? Brushes.Aquamarine : Brushes.Salmon;
                graphics.FillRectangle(brash, rectangles[i]);
            }

            return bitmap;
        }
        
        private static void Main()
        {
            var center = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);

            var circularCloudLayouter = new CircularCloudLayouter(center);
            var random = new Random();
            var rectangles = Enumerable.Range(0, 100)
                                       .Select(i => circularCloudLayouter.PutNextRectangle(
                                                   new Size(random.Next(20, 60), random.Next(10, 30))))
                                       .ToArray();
            
            var bitmap = CreateBitmap(rectangles, bitmapSize);
            bitmap.Save(Filename);
        }
    }
}