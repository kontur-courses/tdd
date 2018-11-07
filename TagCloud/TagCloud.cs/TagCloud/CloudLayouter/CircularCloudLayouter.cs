using System.Drawing;

namespace TagCloud
{
    public class CircularCloudLayouter : CloudLayouter
    {
        private readonly IPointsSequence pointsSequence;

        public CircularCloudLayouter(IPointsSequence pointsSequence )
        {
            this.pointsSequence = pointsSequence;
        }

        protected override Rectangle GetNextRectangle(Size size)
        {
            while (true)
            {
                var center = pointsSequence.GetNextPoint();
                var rectangle = RectangleBuilder.CreateRectangle(size, center);
                if (IsInsideSurface(rectangle))
                    continue;
                pointsSequence.Reset();
                return rectangle;
            }
        }
    }
}