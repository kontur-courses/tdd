using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly Spiral spiral = new Spiral();
        private readonly Surface surface = new Surface();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rect = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            while (surface.RectangleIntersectsWithOther(rect))
            {
                rect.Location = spiral.GetNextPoint();
            }

            if (rect.Location != Point.Empty)
                rect = GetShiftedToCenterRectangle(rect);
            
            surface.AddRectangle(rect);
            spiral.Reset();
            return rect;
        }

        private Rectangle GetShiftedToCenterRectangle(Rectangle rect)
        {
            while (true)
            {
                var movedRect = DoStepToCenter(rect);
                if (surface.IsRectangleIntersect(movedRect))
                    return rect;
                rect = movedRect;
            }
        }

        private static Rectangle DoStepToCenter(Rectangle rect)
        {
            var rectQuarters = Surface.FindQuartersForRectangle(rect);
            foreach (var quarter in rectQuarters)
            {
                var (dx, dy) = GetDeltaForQuarter(quarter);
                rect.Offset(dx, dy);
            }

            return rect;
        }

        private static (int dx, int dy) GetDeltaForQuarter(Surface.Quarters quarter)
        {
            return quarter switch
            {
                Surface.Quarters.First => (-1, 1),
                Surface.Quarters.Second => (1, 1),
                Surface.Quarters.Third => (1, -1),
                Surface.Quarters.Fourth => (-1, -1),
                _ => throw new ArgumentException()
            };
        }
    }
}