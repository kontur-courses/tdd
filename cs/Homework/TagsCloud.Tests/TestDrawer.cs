using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.Drawer;
using TagsCloud.Visualization.Extensions;

namespace TagsCloud.Tests
{
    public class TestDrawer : Drawer
    {
        protected override void Transform(Graphics graphics, Rectangle[] rectangles)
        {
            var center = rectangles.First().GetCenter();
            var pen = new Pen(Settings.Color);
            graphics.DrawRectangles(pen, rectangles);
            graphics.FillEllipse(Brushes.Red, InitRectangleFromCenter(center, 3, 3));
        }

        private Rectangle InitRectangleFromCenter(Point center, int width, int height)
        {
            return new(center.X - width / 2, center.Y - height / 2, width, height);
        }
    }
}