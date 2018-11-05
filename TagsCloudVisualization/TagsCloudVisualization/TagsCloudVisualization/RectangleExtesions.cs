using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
	public static class RectangleExtesions
	{
		public static bool IntersectsWith(this List<Rectangle> rectangles, Rectangle rect) =>
			rectangles.Any(r => r.IntersectsWith(rect));
	}
}