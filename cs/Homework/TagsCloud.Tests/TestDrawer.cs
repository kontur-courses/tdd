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
            var pen = new Pen(Color.Chocolate);
            graphics.DrawRectangles(pen, rectangles);
            graphics.FillEllipse(Brushes.Red, new Rectangle(center.X, center.Y, 3, 3));
        }
    }
}