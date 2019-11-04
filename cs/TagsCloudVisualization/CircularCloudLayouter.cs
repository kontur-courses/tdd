using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class LayoutItem
    {
        public string Title;
        public Rectangle Rectangle;

        public LayoutItem() { }
        public LayoutItem(string title, Rectangle rectangle)
        {
            Title = title;
            Rectangle = rectangle;
        }
    }

    public class CircularCloudLayouter
    {
        private readonly Point center;
        public readonly List<LayoutItem> Items;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Items = new List<LayoutItem>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newItem = new LayoutItem(Items.Count.ToString(), new Rectangle(default, rectangleSize));
            Items.Add(newItem);
            ReallocRectangles();
            return newItem.Rectangle;
        }

        private void ReallocRectangles()
        {
        }

        public void SaveToFile(string filename)
        {
            if (Items.Count == 0)
                throw new ArgumentException("There are no items.");

            var left = int.MaxValue;
            var right = int.MinValue;
            var top = int.MaxValue;
            var bottom = int.MinValue;
            foreach (var item in Items)
            {
                left = Math.Min(left, item.Rectangle.Left);
                right = Math.Max(right, item.Rectangle.Right);
                top = Math.Min(top, item.Rectangle.Top);
                bottom = Math.Max(bottom, item.Rectangle.Bottom);
            }

            var bmp = new Bitmap(right - left, bottom - top);
            var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.RosyBrown);
            Brush br = new SolidBrush(Color.Green);
            Pen pen = new Pen(Color.Black);
            Brush textBrush = new SolidBrush(Color.Black);
            foreach (var item in Items)
            {
                gr.FillRectangle(br, item.Rectangle.X - left, item.Rectangle.Y - top, item.Rectangle.Width, item.Rectangle.Height);
                gr.DrawRectangle(pen, item.Rectangle.X - left, item.Rectangle.Y - top, item.Rectangle.Width, item.Rectangle.Height);
                gr.DrawString(item.Title, new Font("Tahoma", item.Rectangle.Height, GraphicsUnit.Pixel), textBrush, item.Rectangle.X - left, item.Rectangle.Y - top - 1);
            }
            bmp.Save(filename);
        }
    }
}
