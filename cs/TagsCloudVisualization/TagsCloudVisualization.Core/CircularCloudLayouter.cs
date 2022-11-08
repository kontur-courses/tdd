using System.Drawing;

namespace TagsCloudVisualization.Core
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        public List<Rectangle> Rectangles { get; }

        private readonly ArchimedeanSpiral _spiral;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("X or Y is negative!");

            Center = center;
            Rectangles = new List<Rectangle>();
            _spiral = new ArchimedeanSpiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("X or Y is negative!");

            throw new NotImplementedException();
        }
    }
}