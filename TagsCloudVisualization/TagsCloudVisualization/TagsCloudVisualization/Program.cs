using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	public class CloudWordsForm
	{
		static void Main(string[] args)
		{
			var center = new Point(500, 400);
			var cloud = new CircularCloudLayouter(center);
			var rectangles = GetData(cloud, 200);
			var vizualizer = new RectangleTagsCloudVisualizer(center.X * 2, center.Y * 2);
			var picture = vizualizer.GetPicture(rectangles);
			picture.Save("CloudTags.png");
		}

		public static Rectangle[] GetData(CircularCloudLayouter cloud, int number)
		{
			var rnd = new Random();
			for (var i = 0; i < number; i++)
				cloud.PutNextRectangle(new Size(rnd.Next(20, 60), rnd.Next(10, 20)));

			return cloud.GetRectangles();
		}
	}
}