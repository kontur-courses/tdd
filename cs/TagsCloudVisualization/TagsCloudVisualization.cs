using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class TagsCloudVisualization
    {
        public static void Visualizate(CircularCloudLayouter cloud, string path)
        {
            var random = new Random();
            var size = GetSizeCloud();
            var bitmap = new Bitmap(2 * size.Width, 2 * size.Height);
            var graphics = Graphics.FromImage(bitmap);
            foreach(var rectangle in cloud.rectangles)
            {
                var randomColor = Color.FromArgb(255, random.Next(255), random.Next(255), random.Next(255));
                var pen = new Pen(randomColor);
                var brush = new SolidBrush(randomColor);
                graphics.DrawRectangle(pen, MoveRectangle(rectangle));
                graphics.FillRectangle(brush, MoveRectangle(rectangle));
            }
            bitmap.Save(path);

            Size GetSizeCloud()
            {
                var top = 0;
                var bottom = 0;
                var right = 0;
                var left = 0;
                bool isFirst = true;
                foreach(var rectangle in cloud.rectangles)
                {
                    if(isFirst)
                    {
                        isFirst = false;
                        top = rectangle.Top;
                        bottom = rectangle.Bottom;
                        right = rectangle.Right;
                        left = rectangle.Left;
                    }
                    if(rectangle.Top < top)
                        top = rectangle.Top;
                    if(rectangle.Bottom > bottom)
                        bottom = rectangle.Bottom;
                    if(rectangle.Left < left)
                        left = rectangle.Left;
                    if(rectangle.Right > right)
                        right = rectangle.Right;
                }
                return new Size(right - left, bottom - top);
            }
            
            Rectangle MoveRectangle(Rectangle r) => 
                new Rectangle(r.X + size.Width - cloud.center.X, r.Y + size.Height - cloud.center.Y, r.Width, r.Height);
        }
    }
}
