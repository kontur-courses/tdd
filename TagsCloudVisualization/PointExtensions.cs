using System;

namespace TagsCloudVisualization
{
	static class PointExtensions
	{
		public static double GetDistanceTo(this Point thisPoint, Point thatPoint)
		{
			var dx = thatPoint.X - thisPoint.X;
			var dy = thatPoint.Y - thisPoint.Y;
			return Math.Sqrt(dx * dx + dy * dy);
		}
	}
}