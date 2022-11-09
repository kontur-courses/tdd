using System.Drawing;

namespace TagsCloudVisualization
{
	public class ArchimedeanSpiral : ISpiral
	{
		public double SpiralStepInPixels { get; }

		public double AngleStepInRadians { get; }

		public double CurrentAngleInRadians { get; private set; }

		public ArchimedeanSpiral(Point center, double spiralStepInPixels, double angleStepInDegrees)
		{
			this.center = center;
			SpiralStepInPixels = spiralStepInPixels;
			AngleStepInRadians = angleStepInDegrees * Math.PI / 180;
			CurrentAngleInRadians = 0;
		}

		public Point GetNextPoint()
		{
			var polarRadius = SpiralStepInPixels * CurrentAngleInRadians;
			var x = polarRadius * Math.Cos(CurrentAngleInRadians);
			var y = polarRadius * Math.Sin(CurrentAngleInRadians);
			
			var localPosition = new Point(Convert.ToInt32(x), Convert.ToInt32(y));

			CurrentAngleInRadians += AngleStepInRadians;

			return localPosition.Plus(center);
		}

		private readonly Point center;
	}
}
