using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Point center;
        public readonly List<Rectangle> Items;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Items = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newItem = new Rectangle(default, rectangleSize);
            Items.Add(newItem);
            ReallocRectangles();
            return newItem;
        }

        private void ReallocRectangles()
        {
        }

        public void SaveToFile(string filename)
        {
            var left = int.MaxValue;
            var right = int.MinValue;
            var top = int.MaxValue;
            var bottom = int.MinValue;
            foreach (var r in Items)
            {
                left = Math.Min(left, r.Left);
                right = Math.Max(right, r.Right);
                top = Math.Min(top, r.Top);
                bottom = Math.Max(bottom, r.Bottom);
            }

            var bmp = new Bitmap(right - left, bottom - top);
            var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.RosyBrown);
            Brush br = new SolidBrush(Color.Green);
            foreach (var item in Items)
                gr.FillRectangle(br, item.X - left, item.Y - top, item.Width, item.Height);
            bmp.Save(filename);
        }
    }
}
