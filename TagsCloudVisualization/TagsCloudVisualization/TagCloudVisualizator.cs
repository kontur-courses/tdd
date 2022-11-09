using System.Drawing;

namespace TagsCloudVisualization
{
	public class TagCloudVisualizator
	{
		public static Image GetSpiralImage()
		{
			var width = 500;
			var height = 500;

			var picture = new Bitmap(width, height);
			var pen = new SolidBrush(Color.DarkRed);

			var spiral = new ArchimedeanSpiral(new Point(width / 2, height / 2), 10, 5);

			using var graphics = Graphics.FromImage(picture);
			graphics.FillRectangle(new SolidBrush(Color.Cornsilk), new Rectangle(0, 0, width, height));

			for (var i = 0; i < 500; i++)
			{
				var point = spiral.GetNextPoint();
				if (point.X >= width || point.Y >= height) break;

				graphics.FillEllipse(pen, new Rectangle(point, new Size(5, 5)));
			}

			return picture;
		}

		public static Image GetTagCloudImage(List<Rectangle> rectangles, Size imageSize)
		{
			var image = new Bitmap(imageSize.Width, imageSize.Height);

			using var graphics = Graphics.FromImage(image);
			graphics.FillRectangle(new SolidBrush(Color.Cornsilk), new Rectangle(0, 0, imageSize.Width, imageSize.Height));

			graphics.FillRectangles(new SolidBrush(Color.Chartreuse), rectangles.ToArray());
			graphics.DrawRectangles(new Pen(Color.DarkRed), rectangles.ToArray());

			return image;
		}
	}
}
