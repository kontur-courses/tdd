using System;
using System.Drawing;
using System.Threading;

namespace TagsCloudVisualization
{
    internal class Visualizer
    {
        private readonly CircularCloudLayouter _layout;
        private Bitmap _bitmap;
        private int _minX, _maxX, _minY, _maxY, _yHeight;

        public Visualizer(CircularCloudLayouter layout)
        {
            _layout = layout;
            CalculateImageBorders(_layout);
            CreateBitmapForDrawing();
        }

        public void DrawTagsCloud(string path)
        {
            var g = Graphics.FromImage(_bitmap);
            DrawAllRectangles(g);

            _bitmap.Save(path);
        }

        private void DrawAllRectangles(Graphics g)
        {
            foreach (var rectangle in _layout.Rectangles)
            {
                var xShift = -_minX;
                var yShift = -_yHeight;
                var x = rectangle.X + xShift;
                var y = rectangle.Y + yShift - rectangle.Height;

                g.FillRectangle(
                    TakeRandomColor(),
                    x, y,
                    rectangle.Size.Width,
                    rectangle.Size.Height);
            }
        }

        private static Brush TakeRandomColor()
        {
            var rnd = new Random();
            var color = Color.FromArgb(rnd.Next());
            var brush = new SolidBrush(color);
            Thread.Sleep(50);

            return brush;
        }

        private void CalculateImageBorders(CircularCloudLayouter layout)
        {
            foreach (var rectangle in layout.Rectangles)
            {
                if (rectangle.X <= _minX)
                    _minX = rectangle.X;
                if (rectangle.X + rectangle.Width >= _maxX)
                    _maxX = rectangle.X + rectangle.Width;
                if (rectangle.Y - rectangle.Height <= _minY)
                    _minY = rectangle.Y - rectangle.Height;
                if (rectangle.Y >= _maxY)
                    _maxY = rectangle.Y;
                if (rectangle.Y - rectangle.Height <= _yHeight)
                    _yHeight = rectangle.Y - rectangle.Height;
            }
        }

        private void CreateBitmapForDrawing()
        {
            var width = Math.Abs(_minX) + Math.Abs(_maxX) + 1;
            var height = Math.Abs(_minY) + Math.Abs(_maxY) + 1;
            _bitmap = new Bitmap(width, height);
        }
    }
}