using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class PositionSearchResult
    {
        public double MinDistance { get; private set; }
        public Segment MinSegment { get; private set; }
        public Point ClosestRectCoordinates { get; private set; }
        public PositionSearchResult(double minDistance, Segment minSegment, Point closestRectCoordinates)
        {
            MinDistance = minDistance;
            MinSegment = minSegment;
            ClosestRectCoordinates = closestRectCoordinates;
        }
        public PositionSearchResult Update(double minDistance, Segment minSegment, Point closestRectCoordinates)
        {
            if (minDistance < MinDistance)
            {
                return new PositionSearchResult(minDistance, minSegment, closestRectCoordinates);
            }
            return this;
        }
    }
}
