using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Visualization
{
    public class Drawer : IDrawer
    {
        public void DrawCanvasBoundary(Graphics g, Size imgSize)
        {
            var boundary = new Rectangle(Point.Empty,
                new Size(imgSize.Width - 1, imgSize.Height - 1));

            g.DrawRectangle(Pens.Red, boundary);
        }

        public void DrawAxis(Graphics g, Size imgSize, Point cloudCenter)
        {
            var cloudCenterOnImg = new Point(cloudCenter.X + imgSize.Width / 2,
                cloudCenter.Y + imgSize.Height / 2);

            g.DrawLine(Pens.Green, cloudCenterOnImg, new Point(cloudCenterOnImg.X, 0));
            g.DrawLine(Pens.Green, cloudCenterOnImg, new Point(cloudCenterOnImg.X, imgSize.Height));

            g.DrawLine(Pens.Green, cloudCenterOnImg, new Point(0, cloudCenterOnImg.Y));
            g.DrawLine(Pens.Green, cloudCenterOnImg, new Point(imgSize.Width, cloudCenterOnImg.Y));
        }

        public void DrawCloudBoundary(Graphics g, Size imgSize, Point cloudCenter, int cloudCircleRadius)
        {
            var cloudCenterOnImg = new Point(cloudCenter.X + imgSize.Width / 2,
                cloudCenter.Y + imgSize.Height / 2);

            var location = new Point(cloudCenterOnImg.X - cloudCircleRadius,
                cloudCenterOnImg.Y - cloudCircleRadius);

            var size = new Size(cloudCircleRadius * 2, cloudCircleRadius * 2);

            g.DrawEllipse(Pens.DodgerBlue, new Rectangle(location, size));
        }

        public void DrawRectangles(Graphics g, Point cloudCenter, List<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
                g.DrawRectangle(Pens.Black, rectangle);
        }
    }
}