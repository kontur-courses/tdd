using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class LayoutVisualization
    {
        private const int scale = 10;

        public Bitmap Visualize(CircularCloudLayouter layouter)
        {
            var bitmap = new Bitmap(2 * scale * layouter.Width, 2 * scale * layouter.Height);
            var graphics = Graphics.FromImage(bitmap);
            DrawMarking(graphics, bitmap.Width, bitmap.Height);
            var pen = new Pen(Color.Black);
            var i = 1;
            foreach (var rectangle in layouter.Rectangles)
            {
                var newLocation = new Point(scale * rectangle.X + bitmap.Width / 2,
                    scale * rectangle.Y + bitmap.Height / 2);
                var newSize = new Size(rectangle.Width * scale, rectangle.Height * scale);
                var newRect = new Rectangle(newLocation, newSize);
                graphics.DrawRectangle(pen, newRect);
                DrawCounter(graphics, i, newRect);
                i++;
            }

            return bitmap;
        }

        private void DrawCounter(Graphics graphics, int count, Rectangle rectangle)
        {
            using (var font = new Font("Arial", Math.Min(rectangle.Height, 10),
                FontStyle.Regular, GraphicsUnit.Point))
            {
                var stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                graphics.DrawString(count.ToString(), font, Brushes.Black, rectangle, stringFormat);
            }
        }

        private static void DrawMarking(Graphics graphics, int width, int height)
        {
            graphics.FillRectangle(Brushes.White, 0, 0, width, height);
            var pen = new Pen(Color.Gray);
            graphics.DrawRectangle(pen, new Rectangle(new Point(0, 0),
                new Size(width - 1, height - 1)));
            graphics.DrawLine(pen, new Point(0, height / 2),
                new Point(width, height / 2));
            graphics.DrawLine(pen, new Point(width / 2, 0),
                new Point(width / 2, height));
        }
    }
}