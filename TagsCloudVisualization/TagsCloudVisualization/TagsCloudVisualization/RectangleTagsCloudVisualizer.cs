using System;
using System.Drawing;
namespace TagsCloudVisualization
{
	public class RectangleTagsCloudVisualizer
	{
		private readonly Bitmap bitmap;

		public RectangleTagsCloudVisualizer(int width, int height)
		{
			if (width <= 0 || height <= 0)
				throw new ArgumentException("Lengths of size must be positive");
			bitmap = new Bitmap(width, height);
		}

		public Bitmap GetPicture(Rectangle[] rectangles)
		{
			var pen = new Pen(Color.Black, 1);

			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.Transparent);
				graphics.DrawRectangles(pen, rectangles);
			}

			return bitmap;
		}
	}
}