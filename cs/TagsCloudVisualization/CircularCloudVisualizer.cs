using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        private Size bitmapSize;
        private List<Rectangle> rectangles;
        private const int defaultBitmapSide = 500;

        public CircularCloudVisualizer(List<Rectangle> rectangles)
        {
            this.rectangles = rectangles;
        }

        public void DetermineBitmapSizes()
        {
            var mostDistantRectangle = rectangles
                .OrderByDescending(rect => rect.GetCircumcircleRadius())
                .First();
            var circleRadius = mostDistantRectangle.GetCircumcircleRadius();
            var bitmapSide = Math.Max(circleRadius * 2, defaultBitmapSide);
            bitmapSize = new Size(bitmapSide, bitmapSide);           
        }

        public Bitmap DrawRectangles()
        {
            DetermineBitmapSizes();
            var canvas = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            if (rectangles.Count == 0)
                return canvas;
            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.Clear(Color.White);
                foreach (var rectangle in rectangles)
                {
                    var shiftedRectangle = ShiftRectangleToCenter(rectangle);
                    graphics.DrawRectangle(new Pen(Color.Black, 5), shiftedRectangle);
                }
            }
            return canvas;
        }

        public Rectangle ShiftRectangleToCenter(Rectangle rect)
        {
            var layoutCenter = new Point(bitmapSize.Width/2, bitmapSize.Height/2);
            return new Rectangle(new Point(rect.X + layoutCenter.X, rect.Y + layoutCenter.Y), rect.Size);
        }

    }
}
