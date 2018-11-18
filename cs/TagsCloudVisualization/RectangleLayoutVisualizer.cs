using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectangleLayoutVisualizer
    {
        public Bitmap Vizualize(Size size, Rectangle[] layout, Point layoutCenter)
        {
            var imageCenter = new Point(size.Width / 2, size.Height / 2);
            ShiftAboutImageCenter(layout, layoutCenter, imageCenter);

            var image = new Bitmap(size.Width, size.Height);

            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.White);
                if (layout.Length > 0)
                    graphics.DrawRectangles(new Pen(Brushes.Black), layout);
            }

            return image;
        }

        private void ShiftAboutImageCenter(Rectangle[] layout, Point layoutCenter,
            Point imageCenter)
        {
            var offset = new Size(imageCenter.X - layoutCenter.X,
                imageCenter.Y - layoutCenter.Y);

            for (int i = 0; i < layout.Length; i++)
                layout[i].Location = Point.Add(layout[i].Location, offset);
        }
    }
}
