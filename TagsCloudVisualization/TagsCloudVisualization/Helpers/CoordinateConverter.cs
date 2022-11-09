using System.Drawing;

namespace TagsCloudVisualization
{
	public static class CoordinatesConverter
	{
		public static Point ToCartesian(double polarRadius, double polarAngle)
		{
			var x = polarRadius * Math.Cos(polarAngle);
			var y = polarRadius * Math.Sin(polarAngle);
			
			return new Point(Convert.ToInt32(x), Convert.ToInt32(y));
		}

		public static (double polarRadius, double polarAngle) ToPolar(Point cartesianCoordinates)
		{
			var polarAngle = Math.Atan2(cartesianCoordinates.Y, cartesianCoordinates.X);
			var polarRadius = Math.Sqrt(Math.Pow(cartesianCoordinates.X, 2) + Math.Pow(cartesianCoordinates.Y, 2));

			return (polarRadius, polarAngle);
		}
	}
}
