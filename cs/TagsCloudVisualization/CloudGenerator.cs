using System.Drawing;

namespace TagsCloudVisualization;

public static class CloudGenerator
{
	public static List<Rectangle> GenerateRandomCloud(ICircularCloudLayouter layouter, int count, int minWidth,
		int maxWidth)
	{
		var cloud = new List<Rectangle>();
		var rnd = new Random();
		for (var i = 0; i < count; i++)
		{
			var width = rnd.Next(minWidth, maxWidth);
			var height = rnd.Next(minWidth, maxWidth);
			layouter.PutNextRectangle(new Size(width, height), cloud);
		}

		return cloud;
	}
}