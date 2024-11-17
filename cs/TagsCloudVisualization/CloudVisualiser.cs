using System.Drawing;
#pragma warning disable CA1416

namespace TagsCloudVisualization;

public static class CloudVisualiser
{
	public static void DrawAndSaveCloud(IEnumerable<Rectangle> rectangles, string path, string fileName, int imageWidth, int imageHeight, Color backgroundColor)
	{
		var filePath = Path.Combine(path, fileName + ".png");
		using var bitmap = new Bitmap(imageWidth, imageHeight);
		using var graphics = Graphics.FromImage(bitmap);
		graphics.Clear(backgroundColor);

		var random = new Random();
		foreach (var rectangle in rectangles)
		{
			var color = Color.FromArgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
			using var brush = new SolidBrush(color);
			graphics.FillRectangle(brush, rectangle);
			graphics.DrawRectangle(Pens.Black, rectangle);
		}

		try
		{
			bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Не удалось сохранить файл {filePath}. Ошибка: {e.Message} ");
		}
	}
}

#pragma warning restore CA1416