using System.Drawing;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class CloudLayouterDrawer : ICloudLayouterDrawer
    {
        private readonly Pen pen;
        public CloudLayouter CloudLayouter { get; }

        public CloudLayouterDrawer(Color color, CloudLayouter cloudLayouter)
        {
            CloudLayouter = cloudLayouter;
            pen = new Pen(color);
        }

        public void Draw(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.DrawRectangles(pen, CloudLayouter.Rectangles);
        }
    }
}