using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
	public class EntryPoint
	{
		static void Main(string[] args)
		{
			var layouter = new CircularCloudLayouter(new Point(250, 250));
			var rectangles = new List<Rectangle>();
			var rnd = new Random();

			for (var i = 0; i < 100; i++)
			{
				var scale = rnd.Next(1, 5);
				rectangles.Add(layouter.PutNextRectangle(new Size(30 * scale, 10 * scale)));
			}

			var image = TagCloudVisualizator.GetTagCloudImage(rectangles, new Size(500, 500));

			var imageSaver = new ImageSaver(Environment.CurrentDirectory + "\\", ImageFormat.Png);

			imageSaver.Save(image, "tagsCloud");

			Console.WriteLine($"Image save in folder: {imageSaver.Directory}");
		}
	}
}