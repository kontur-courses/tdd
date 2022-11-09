using System.Drawing;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			spiral = new ArchimedeanSpiral(center, 1, 1);
			placedRectangles = new List<Rectangle>();
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			var rectangleOnSpiral = GetRectangleOnSpiral(rectangleSize);
			var shiftedRectangle = ShiftToCenter(rectangleOnSpiral);

			placedRectangles.Add(shiftedRectangle);

			return shiftedRectangle;
		}

		private Rectangle GetRectangleOnSpiral(Size rectangleSize)
		{
			Rectangle newRectangle;

			do
			{
				newRectangle = RectangleCreator.GetRectangle(spiral.GetNextPoint(), rectangleSize);
			}
			while (IsContainsIntersection(newRectangle));

			return newRectangle;
		}

		private Rectangle ShiftToCenter(Rectangle oldRectangle)
		{
			if (placedRectangles.Count == 0)
			{
				return oldRectangle;
			}

			var newRectangle = oldRectangle;
			var nextRectangle = oldRectangle;

			while (!IsContainsIntersection(nextRectangle))
			{
				newRectangle = nextRectangle;

				var (polarRadius, polarAngle) = CoordinatesConverter.ToPolar(newRectangle.Center().Minus(center));

				var nextLocation = CoordinatesConverter.ToCartesian(polarRadius - ShiftStep, polarAngle).Plus(center);

				nextRectangle = RectangleCreator.GetRectangle(nextLocation, newRectangle.Size);
			}


			return newRectangle;
		}
		
		private bool IsContainsIntersection(Rectangle newRectangle)
		{
			return placedRectangles.Any(oldRectangle => oldRectangle.IntersectsWith(newRectangle));
		}

		private readonly ISpiral spiral;
		private readonly Point center;
		private readonly List<Rectangle> placedRectangles;

		private const double ShiftStep = 1;
	}
}