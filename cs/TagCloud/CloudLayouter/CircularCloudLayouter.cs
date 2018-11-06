using System.Drawing;

namespace TagCloud
{
    public class CircularCloudLayouter : CloudLayouter
    {
        private readonly IPointsSequence pointsSequence;

        public CircularCloudLayouter(Point center) : base(center)
        {
            pointsSequence = new SpiralPointsSequence(1);
        }

        protected override Rectangle GetNextRectangle(Size size)
        {
            while (true)
            {
                var center = pointsSequence.GetNextPoint().WithTranslation(Center);
                var rectangle = new Rectangle(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
                if (IsInsideSurface(rectangle)) continue;
                pointsSequence.Reset();
                return rectangle;
            }
        }
    }
}