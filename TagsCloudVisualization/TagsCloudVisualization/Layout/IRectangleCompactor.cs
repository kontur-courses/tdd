using System.Drawing;

namespace TagsCloudVisualization.Layout
{
	public interface IRectangleCompactor
	{
		Rectangle Compact(Rectangle candidate, IReadOnlyList<Rectangle> placed, Point center);
	}
}
