using System.Drawing;
using System.Drawing.Drawing2D;

namespace TagsCloud.Visualization.Drawer
{
    public class GradientDrawer : Drawer
    {
        protected override void Transform(Graphics graphics, Rectangle[] rectangles)
        {
            var linGrBrush = new LinearGradientBrush(
                new Point(0, 10),
                new Point(200, 10),
                Color.Blue,
                Color.Red);

            var pen = new Pen(linGrBrush);
            graphics.DrawRectangles(pen, rectangles);
        }
    }
}