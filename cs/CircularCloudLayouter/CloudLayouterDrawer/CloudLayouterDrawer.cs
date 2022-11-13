using System.Drawing;

// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class CloudLayouterDrawer : ICloudLayouterDrawer
    {
        private readonly Pen pen;

        public CloudLayouterDrawer(Color color)
        {
            pen = new Pen(color);
        }

        public void Draw(Graphics graphics, Rectangle[] rectangles)
        {
            graphics.Clear(Color.White);
            graphics.DrawRectangles(pen, rectangles);
        }
    }
}