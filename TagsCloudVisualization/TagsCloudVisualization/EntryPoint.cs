using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class EntryPoint
{
	private static void Main(string[] args)
	{
		var width = 800;
		var height = 800;
		var layouter = new CircularCloudLayouter(new Point(width / 2, height / 2));
		var rectangles = new List<Rectangle>();
		var rnd = new Random();

		for (var i = 0; i < 100; i++)
		{
			var scale = rnd.Next(1, 5);
			rectangles.Add(layouter.PutNextRectangle(new Size(30 * scale, 10 * scale)));
		}

		var image = TagCloudVisualizator.GetRandomColorTagCloudImage(rectangles, new Size(width, height),
			new Point(0, 0));

		var imageSaver = new ImageSaver(Environment.CurrentDirectory, ImageFormat.Png);

		imageSaver.Save(image, "tagsCloud");

		Console.WriteLine($"Image save in folder: {imageSaver.Directory}");
	}
}