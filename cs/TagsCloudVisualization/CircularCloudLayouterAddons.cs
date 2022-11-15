using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Distributions;

namespace TagsCloudVisualization
{
    public partial class CircularCloudLayouter
    {
        private Point GetNextRectanglePosition(Size rectangleSize)
        {
            var shiftX = -rectangleSize.Width / 2;
            var shiftY = -rectangleSize.Height / 2;
            
            if (Rectangles.Count == 0)
                return new Point(center.X + shiftX, center.Y + shiftY);

            var potentialPoint = GetPotentialPoint();

            while (Rectangles.Any(rectangle =>
                       RectangleAddons.IsRectanglesIntersect(new Rectangle(potentialPoint, rectangleSize), rectangle)))
                potentialPoint = GetPotentialPoint();

            return potentialPoint;
        }
        
        private Point GetPotentialPoint()
        {
            var point = distribution.GetNextPoint();
            
            distributionPoints.Add(point);
                
            return point;
        }

        public void Clear()
        {
            Rectangles.Clear();
            distributionPoints.Clear();
            distribution = new Spiral(center);
        }
    }
}