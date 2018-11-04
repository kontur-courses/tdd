using System.Drawing;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        private readonly Point center;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size size)
        {
            return Rectangle.Empty;
        }
    }
}
