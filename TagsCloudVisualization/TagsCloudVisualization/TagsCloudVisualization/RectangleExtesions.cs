﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
	public static class RectangleExtesions
	{
		public static bool IntersectsWith(this Rectangle rectangle, List<Rectangle> rectangles) =>
			rectangles.Any(r => r.IntersectsWith(rectangle));
	}
}