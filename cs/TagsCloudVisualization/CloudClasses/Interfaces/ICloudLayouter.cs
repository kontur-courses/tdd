using System.Drawing;

namespace TagsCloudVisualization.CloudClasses.Interfaces
{
    public interface ICloudLayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);

        public List<Rectangle> Rectangles { get; }
    }
}
