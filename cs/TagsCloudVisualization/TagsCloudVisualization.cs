using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class TagsCloudVisualization
    {
        public static void Visualizate(CircularCloudLayouter cloud, string path)
        {
            var random = new Random();
            var size = cloud.Size;
            var bitmap = new Bitmap(2 * size.Width, 2 * size.Height);
            var vectorShift = new Point(size.Width - cloud.center.X, size.Height - cloud.center.Y);
            var graphics = Graphics.FromImage(bitmap);
            foreach(var rectangle in cloud.rectangles)
            {
                var randomColor = Color.FromArgb(255, random.Next(255), random.Next(255), random.Next(255));
                var pen = new Pen(randomColor);
                var brush = new SolidBrush(randomColor);
                graphics.DrawRectangle(pen, ShiftRectangle(rectangle));
                graphics.FillRectangle(brush, ShiftRectangle(rectangle));
            }
            bitmap.Save(path);
            
            Rectangle ShiftRectangle(Rectangle r) => 
                new Rectangle(r.X + vectorShift.X, r.Y + vectorShift.Y, r.Width, r.Height);
        }
    }
}
