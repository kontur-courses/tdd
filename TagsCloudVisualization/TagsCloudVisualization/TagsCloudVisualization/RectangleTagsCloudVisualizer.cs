using System.Drawing;
namespace TagsCloudVisualization
{
	public static class RectangleTagsCloudVisualizer
	{
		public static Bitmap GetPicture(Rectangle[] rectangles, Color color)
		{
			var bitmap = new Bitmap(1000, 800);
			var pen = new Pen(color, 1);

			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.Transparent);
				graphics.DrawRectangles(pen, rectangles);
			}

			return bitmap;
		}
	}
}