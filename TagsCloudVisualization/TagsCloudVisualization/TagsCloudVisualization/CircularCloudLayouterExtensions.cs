using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	public static class CircularCloudLayouterExtensions
	{
		public static void AddRectangleNTimes(this CircularCloudLayouter cloud, int number)
		{
			var rnd = new Random();
			for (var i = 0; i < number; i++)
				cloud.PutNextRectangle(new Size(rnd.Next(10, 30), rnd.Next(4, 12)));
		}
	}
}
