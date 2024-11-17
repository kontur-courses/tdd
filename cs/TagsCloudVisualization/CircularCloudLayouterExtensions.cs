using System.Drawing;

namespace TagsCloudVisualization;

public static class CircularCloudLayouterExtensions
{
	public static Size GetBoundaryBox(this IList<Rectangle> rectangles)
	{
		var minX = rectangles.Min(r => r.Left);
		var minY = rectangles.Min(r => r.Top);
		var maxX = rectangles.Max(r => r.Right);
		var maxY = rectangles.Max(r => r.Bottom);

		return new Size(maxX - minX, maxY - minY);
	}

	public static bool IsIntersecting(this IEnumerable<Rectangle> rectangles, Rectangle rectangle)
		=> rectangles.Any(existingRectangle => existingRectangle.IntersectsWith(rectangle));
}