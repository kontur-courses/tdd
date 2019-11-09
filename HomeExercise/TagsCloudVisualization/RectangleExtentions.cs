﻿using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	public static class RectangleExtensions
	{
		public static int GetBottom(this Rectangle rectangle) => rectangle.Y - rectangle.Height;
	}
}