using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TagsCloudVisualization.Curves;

namespace TagsCloudVisualization
{
	public class CloudWordsForm
	{
		static void Main(string[] args)
		{
			var center = new Point(500, 400);
			var spiral = new Spiral(factorStep: 0.5, degreeStep: Math.PI / 18, center: center);
			var cloud = new CircularCloudLayouter(center, spiral);
			var rectangles = GetData(cloud, 200);
			var vizualizer = new RectangleTagsCloudVisualizer(center.X * 2, center.Y * 2);
			var picture = vizualizer.GetPicture(rectangles);
			picture.Save("CloudTags.png");
		}

		public static Rectangle[] GetData(CircularCloudLayouter cloud, int number)
		{
			var rnd = new Random();
			for (var i = 0; i < number; i++)
				cloud.PutNextRectangle(new Size(rnd.Next(20, 25), rnd.Next(10, 15)));

			return cloud.GetRectangles();
		}
	}
}