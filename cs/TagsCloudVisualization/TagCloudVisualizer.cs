using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization
{
    class TagCloudVisualizer
    { 
        private readonly Brush backgroundBrush;
        private readonly Brush shapeBrush;
        private readonly Pen shapeRectangle;
        private readonly IEnumerable<Rectangle> _rectangles;
        private BitmapSaver _bitmapSaver;
        public TagCloudVisualizer(IEnumerable<Rectangle> rectangles)
        {
            _bitmapSaver = new BitmapSaver();
            _rectangles = rectangles;
            backgroundBrush = Brushes.AliceBlue;
            shapeBrush = Brushes.BlueViolet;
            shapeRectangle = new Pen(Color.Aqua);
        }

        public void CreateImageWithRectangles(Size sizeOfImage, string directoryPath = @".")
        {
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException("The specified directory does not exist");

            var bitmap = DrawRectangles(new Bitmap(sizeOfImage.Width, sizeOfImage.Height));
            _bitmapSaver.Save(bitmap, directoryPath);

            bitmap.Dispose();
        }

        private Bitmap DrawRectangles(Bitmap bitmap)
        {
            var graphics = Graphics.FromImage(bitmap);

            graphics.FillRectangle(backgroundBrush, new Rectangle(new Point(0, 0), bitmap.Size));
            foreach (var rect in _rectangles)
            {
                graphics.FillRectangle(shapeBrush, rect);
                graphics.DrawRectangle(shapeRectangle, rect);
            }

            graphics.Dispose();
            return bitmap;
        }
    }
}
