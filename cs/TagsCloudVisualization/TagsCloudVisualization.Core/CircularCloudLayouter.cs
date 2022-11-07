using System.Drawing;

namespace TagsCloudVisualization.Core
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        public List<Rectangle> Rectangles { get; }

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("");

            Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new Exception();
        }
    }
}