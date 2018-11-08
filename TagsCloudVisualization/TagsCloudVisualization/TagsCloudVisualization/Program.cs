using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class CloudWordsForm
	{
		private static void Main(string[] args)
		{
			var parameters = CloudParameters.Parse(args);

			if (!CloudParameters.IsCorrect(parameters))
				return;

			var cloud = new CircularCloudLayouter(parameters.curve);
			var rectangles = GetData(cloud, parameters.count, parameters.maxLengthRect);
			var picture = RectangleTagsCloudVisualizer.GetPicture(rectangles, Color.Aqua);
			picture.Save("CloudTags.png");
		}

		public static Rectangle[] GetData(CircularCloudLayouter cloud, int number, int maxLength)
		{
			var rnd = new Random();
			for (var i = 0; i < number; i++)
				cloud.PutNextRectangle(new Size(rnd.Next(5, maxLength), rnd.Next(5, 30)));

			return cloud.GetRectangles();
		}
	}
}