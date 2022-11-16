using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public class RectangleVisualisator
    {
        private readonly CircularCloudLayouter _layouter;
        private readonly Random _random;
        private Bitmap _bitmap;
        private Size shiftToBitmapCenter;
        
        public IReadOnlyList<Rectangle> Rectangles;

        public RectangleVisualisator(CircularCloudLayouter layouter)
        {
            _layouter = layouter;
            Rectangles = layouter.Rectangles;
            _random = new Random();
            _bitmap = GenerateBitmap();
            shiftToBitmapCenter = new Size(_bitmap.Width / 2, _bitmap.Height / 2);
        }

        private Bitmap GenerateBitmap()
        {
            var width = _layouter.Rectangles.Max(rectangle => rectangle.Right) - 
                        _layouter.Rectangles.Min(rectangle => rectangle.Left);
            
            var height = _layouter.Rectangles.Max(rectangle => rectangle.Bottom) - 
                         _layouter.Rectangles.Min(rectangle => rectangle.Top);

            return new Bitmap(width * 2, height * 2);
        }

        public void Paint()
        {
            Graphics graphics = Graphics.FromImage(_bitmap);
            graphics.Clear(Color.DimGray);
            Pen pen = new Pen(Color.Red);

            foreach (var rectangle in Rectangles)
            {
                var rectangleOnMap = CreateRectangleOnMap(rectangle);
                graphics.DrawRectangle(pen, rectangleOnMap);
                pen = new Pen(Color.FromArgb(_random.Next() % 255, _random.Next() % 255, _random.Next() % 255));
            }
            
            pen.Dispose();
            graphics.Dispose();
        }

        private Rectangle CreateRectangleOnMap(Rectangle rectangle)
        {
            return new Rectangle(rectangle.Location + shiftToBitmapCenter, rectangle.Size);
        }
        
        public void Save(string filename)
        {
            _bitmap.Save(filename, ImageFormat.Png);
            _bitmap.Dispose();
        }
    }
}