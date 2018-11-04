using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudVisualizer
    {
        private Size bitmapSize;
        private CircularCloudLayouter layouter;

        public CircularCloudVisualizer(CircularCloudLayouter layouter)
        {
            this.layouter = layouter;
        }

        public void DetermineBitmapSizes()
        {
            if (layouter.Rectangles.Count == 0)
                throw new NullReferenceException("No Rectangles");
            var mostDistantRectangle = layouter.Rectangles
                .OrderByDescending(rect => GetCircumscribedСircleRadius(rect))
                .First();
            var circleRadius = GetCircumscribedСircleRadius(mostDistantRectangle);
            var bitmapSide = Math.Max(circleRadius * 2, 500);
            bitmapSize = new Size(bitmapSide, bitmapSide);           
        }

        public int GetCircumscribedСircleRadius(Rectangle rect)
        {
            var maxRightBorder = Math.Max(rect.Left, rect.Right);
            var maxTopBorder = Math.Max(rect.Top, rect.Bottom);
            return (int)Math.Sqrt(Math.Pow(maxRightBorder, 2) + Math.Pow(maxTopBorder, 2));
        }

        public Bitmap DrawRectangles(List<Rectangle> rectangles)
        {
            DetermineBitmapSizes();
            var canvas = new Bitmap(bitmapSize.Width, bitmapSize.Height);
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
