using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        public List<Point> Points { get; set; }
        public List<Point> FreePoints { get; set; }
        public Point Center { get; private set; }
        private double segmentLength;
        private double helixPitch;
        private int pointsQuantity;
        private double lastX, lastY;

        public Spiral(Point center, double segmentLength = 50, int pointsQuantity = 200, double helixPitch = 20)
        {
            Points = new List<Point>();
            FreePoints = new List<Point>();
            Center = center;
            this.segmentLength = segmentLength;
            this.pointsQuantity = pointsQuantity;
            this.helixPitch = helixPitch;

            double x = 0, y = 0;
            AddPoint(Center.X, Center.Y);
            y += segmentLength;
            AddPoint(Center.X + x, Center.Y + y);
            lastX = x;
            lastY = y;
        }

        public void AddPoint(double x, double y)
        {
            var addedPoint = new Point((int)x, (int)y);
            Points.Add(addedPoint);
            FreePoints.Add(addedPoint);
        }

        public void AddMorePointsInSpiral(int addQuantity)
        {
            var cycleLimit = Points.Count + addQuantity;

            for (int i = Points.Count; i < cycleLimit; ++i)
            {
                double r = Math.Sqrt(lastX * lastX + lastY * lastY);
                double tx = helixPitch * lastX + r * lastY;
                double ty = helixPitch * lastY - r * lastX;
                double tLen = Math.Sqrt(tx * tx + ty * ty);
                double k = segmentLength / tLen;
                lastX -= tx * k;
                lastY -= ty * k;
                AddPoint(Center.X + lastX, Center.Y + lastY);
            }
        }


    }
}