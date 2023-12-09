using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ICircularCloudLayouter
    {
        public Point Center { get; }

        public Rectangle PutNextRectangle(Size rectangleSize, ICollection<Rectangle> existingRectangles);
    }
}
