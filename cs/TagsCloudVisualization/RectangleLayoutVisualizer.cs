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
            var graphics = Graphics.FromImage(image);

            graphics.Clear(Color.White);
            graphics.DrawRectangles(new Pen(Brushes.Black), layout);

            return image;
        }

        private void ShiftAboutImageCenter(Rectangle[] layout, Point layoutCenter,
            Point imageCenter)
        {
            var offset = (X: imageCenter.X - layoutCenter.X,
                Y: imageCenter.Y - layoutCenter.Y);
            for(int i=0; i<layout.Length; i++)
            {
                layout[i].X += offset.X;
                layout[i].Y += offset.Y;
            }
        }
    }
}
