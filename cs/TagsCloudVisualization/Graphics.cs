using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
	internal class Graphics
	{
		private readonly Random Random = new Random();

		public void SaveMap(IReadOnlyList<Rectangle> allRectangles, string nameOfImage)
		{
			if (allRectangles.Count == 0)
				return;

			var maxX = allRectangles.Max(f => f.X + f.Width);
			var maxY = allRectangles.Max(f => f.Y + f.Height);
			var minX = allRectangles.Min(f => f.X);
			var minY = allRectangles.Min(f => f.Y);

			var sizeX = maxX - minX;
			var sizeY = maxY - minY;

			var locationOfCenter = allRectangles[0].Location + new Size(allRectangles[0].Width - 5, allRectangles[0].Height - 5) - new Size(minX, minY);
			var center = new Rectangle(locationOfCenter, new Size(10, 10));

			using (var map = new Bitmap(sizeX, sizeY))
			using (var graphics = System.Drawing.Graphics.FromImage(map))
			{
				for (var index = 0; index < allRectangles.Count; index++)
				{
					var rectangle = allRectangles[index];
					var color = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
					using (var brush = new SolidBrush(color))
					{
						var location = rectangle.Location - new Size(minX, minY);
						graphics.FillRectangle(brush, new Rectangle(location, rectangle.Size));
					}
				}
				using (var brush = new SolidBrush(Color.Black))
				{
					graphics.FillEllipse(brush, center);
				}
				map.Save($"{nameOfImage}.png", ImageFormat.Png);
			}
		}
	}
}