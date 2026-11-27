using System.Drawing;

namespace TagsCloudVisualization.Visualization
{
	public interface ICloudRenderer
	{
		Bitmap Render(IReadOnlyList<Rectangle> rectangles, Size canvasSize, Color color);
	}
}
