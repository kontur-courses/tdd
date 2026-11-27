using System.Drawing;

namespace TagsCloudVisualization.Geometry
{
	public static class PointExtensions
	{
		public static double DistanceTo(this Point p, Point other)
		{
			return Math.Sqrt((p.X - other.X) * (p.X - other.X) + (p.Y - other.Y) * (p.Y - other.Y));
		}
	}
}
