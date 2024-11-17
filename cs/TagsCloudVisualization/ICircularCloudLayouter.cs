using System.Drawing;

namespace TagsCloudVisualization;

public interface ICircularCloudLayouter
{
	Rectangle PutNextRectangle(Size rectangleSize, ICollection<Rectangle> rectangles);
}