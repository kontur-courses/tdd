using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
	internal class Graphics
	{
		private readonly Random rnd = new Random();

		public void GetMap(List<Rectangle> list, string name)
		{
			if (list.Count == 0)
				return;

			var maxX = list.Max(f => f.X + f.Width);
			var maxY = list.Max(f => f.Y + f.Height);
			var minX = list.Min(f => f.X);
			var minY = list.Min(f => f.Y);

			var sizeX = maxX - minX;
			var sizeY = maxY - minY;

			using (var map = new Bitmap(sizeX, sizeY))
			using (var graphics = System.Drawing.Graphics.FromImage(map))
			{
				for (var index = 0; index < list.Count; index++)
				{
					var rectangle = list[index];
					var color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
					using (var brush = new SolidBrush(color))
					{
						var location = rectangle.Location - new Size(minX, minY);
						graphics.FillRectangle(
							brush,
							new Rectangle(
								location,
								rectangle.Size
							)
						);
						graphics.DrawString(
							index.ToString(),
							new Font(new FontFamily("Calibri"),15 ),
							new SolidBrush(Color.Chartreuse),
							location
						);
					}
				}
				map.Save($@"C:\Users\Svetlana\Documents\Visual Studio 2015\Projects\tdd\cs\TagsCloudVisualization\{name}.png",
					ImageFormat.Png);
			}
		}
	}
}