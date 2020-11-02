using System.Drawing;

namespace TagsCloudVisualisation
{
    public interface ICircularCloudLayouter
    {
        public Point CloudCenter { get; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}