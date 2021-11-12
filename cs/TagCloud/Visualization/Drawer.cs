using System.Collections.Generic;
using System.Drawing;
using TagCloud.Geometry;

namespace TagCloud.Visualization
{
    public class Drawer : IDrawer
    {
        private const int LineWidth = 2;

        private readonly List<Color> colors = new List<Color>
        {
            Color.Green,
            Color.Red,
            Color.CadetBlue,
            Color.Orange,
            Color.DeepPink,
            Color.Black,
            Color.Chartreuse
        };

        public void DrawCanvasBoundary(Graphics g, Size imgSize)
        {
            var boundary = new Rectangle(Point.Empty,
                new Size(imgSize.Width - 1, imgSize.Height - 1));

            using (var pen = new Pen(Brushes.Red, LineWidth))
            {
                g.DrawRectangle(pen, boundary);
            }
        }

        public void DrawAxis(Graphics g, Size imgSize, Point cloudCenter)
        {
            var cloudCenterOnImg = GetCloudCenterOnImg(imgSize, cloudCenter);

            using (var pen = new Pen(Brushes.Black, LineWidth))
            {
                g.DrawLine(pen, cloudCenterOnImg, new Point(cloudCenterOnImg.X, 0));
                g.DrawLine(pen, cloudCenterOnImg, new Point(cloudCenterOnImg.X, imgSize.Height));

                g.DrawLine(pen, cloudCenterOnImg, new Point(0, cloudCenterOnImg.Y));
                g.DrawLine(pen, cloudCenterOnImg, new Point(imgSize.Width, cloudCenterOnImg.Y));
            }
        }

        public void DrawCloudBoundary(Graphics g, Size imgSize, Point cloudCenter, int cloudCircleRadius)
        {
            var cloudCenterOnImg = GetCloudCenterOnImg(imgSize, cloudCenter);

            var location = new Point(cloudCenterOnImg.X - cloudCircleRadius,
                cloudCenterOnImg.Y - cloudCircleRadius);

            var size = new Size(cloudCircleRadius * 2, cloudCircleRadius * 2);

            using (var pen = new Pen(Brushes.DodgerBlue, LineWidth))
            {
                g.DrawEllipse(pen, new Rectangle(location, size));
            }
        }

        public void DrawRectangles(Graphics g, List<Rectangle> rectangles)
        {
            var i = 0;
            foreach (var rectangle in rectangles)
            {
                g.DrawRectangle(Pens.Black, rectangle);

                var nextColor = colors[i % colors.Count];
                g.FillRectangle(new SolidBrush(nextColor), rectangle);

                i++;
            }
        }

        private Point GetCloudCenterOnImg(Size imgSize, Point cloudCenter)
        {
            return cloudCenter.MovePoint(imgSize.Width / 2, imgSize.Height / 2);
        }
    }
}