using System.Drawing;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		private readonly Point center;
		private Rectangle boundingBox;
		private Rectangle nextBoundingBox;
		private BoxSide side = BoxSide.Down;
		private Point startPointer;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			boundingBox = new Rectangle(center.X, center.Y, 0, 0);
			nextBoundingBox = new Rectangle(0, 0, 0, 0);
			startPointer = new Point(boundingBox.X - boundingBox.Width, boundingBox.Y);
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			var x = 0;
			var y = 0;
			switch (side)
			{
				case BoxSide.Down:
					x = startPointer.X - rectangleSize.Width;
					y = boundingBox.Y - rectangleSize.Height;
					startPointer = new Point(x, y);
					if (nextBoundingBox.Width == 0)
						CreateNewBoundingBox(rectangleSize, x, y);
					ExtensionOfNewBoundBoxForDownSide(rectangleSize, x, y);
					if (x < boundingBox.X)
					{
						side = BoxSide.Left;
						startPointer.Y = y + rectangleSize.Height;
						startPointer.X = boundingBox.X;
					}
					break;

				case BoxSide.Left:
					x = startPointer.X - rectangleSize.Width;
					y = startPointer.Y;
					startPointer.Y = y + rectangleSize.Height;
					ExtensionOfNewBoundBoxForLeftSide(rectangleSize, x, y);
					if (y + rectangleSize.Height > boundingBox.Y + boundingBox.Height)
					{
						side = BoxSide.Up;
						startPointer.X = boundingBox.X;
						startPointer.Y = boundingBox.Y + boundingBox.Height;
					}
					break;

				case BoxSide.Up:
					x = startPointer.X;
					y = startPointer.Y;
					startPointer.X = x + rectangleSize.Width;
					ExtensionOfNewBoundBoxForUpSide(rectangleSize, x, y);
					if (x + rectangleSize.Width > boundingBox.X + boundingBox.Width)
					{
						side = BoxSide.Right;
						startPointer.X = boundingBox.X + boundingBox.Width;
						startPointer.Y = boundingBox.Y + boundingBox.Height;
					}
					break;

				case BoxSide.Right:
					x = startPointer.X;
					y = startPointer.Y - rectangleSize.Height;
					startPointer.Y = y;
					rectangleSize = ExtensionForNewBoundBoxForRightSide(rectangleSize, x, y);
					if (y < boundingBox.Y)
					{
						side = BoxSide.Down;
						boundingBox = new Rectangle(nextBoundingBox.X, nextBoundingBox.Y, nextBoundingBox.Width, nextBoundingBox.Height);
						nextBoundingBox = new Rectangle(0, 0, 0, 0);
						startPointer.X = boundingBox.X + boundingBox.Width;
						startPointer.Y = boundingBox.Y;
					}
					break;
			}
			return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
		}

		private Size ExtensionForNewBoundBoxForRightSide(Size rectangleSize, int x, int y)
		{
			if (IsWidthOfRectOutsideNewBoundBox(ref rectangleSize, x))
			{
				var delta = x + rectangleSize.Width - nextBoundingBox.X - nextBoundingBox.Width;
				nextBoundingBox.Width += delta;
			}
			if (y < nextBoundingBox.Y)
			{
				nextBoundingBox.Height += nextBoundingBox.Y - y;
				nextBoundingBox.Y = y;
			}

			return rectangleSize;
		}

		private bool IsWidthOfRectOutsideNewBoundBox(ref Size rectangleSize, int x)
		{
			return x + rectangleSize.Width > nextBoundingBox.X + nextBoundingBox.Width;
		}

		private void ExtensionOfNewBoundBoxForUpSide(Size rectangleSize, int x, int y)
		{
			if (IsWidthOfRectOutsideNewBoundBox(ref rectangleSize, x))
				nextBoundingBox.Width += x + rectangleSize.Width - nextBoundingBox.X - nextBoundingBox.Width;
			if (y + rectangleSize.Height > nextBoundingBox.Y + nextBoundingBox.Height)
				nextBoundingBox.Height += y + rectangleSize.Height - nextBoundingBox.Y - nextBoundingBox.Height;
		}

		private void ExtensionOfNewBoundBoxForLeftSide(Size rectangleSize, int x, int y)
		{
			if (y + rectangleSize.Height > nextBoundingBox.Y + nextBoundingBox.Height)
				nextBoundingBox.Height += rectangleSize.Height;
			if (x < nextBoundingBox.X)
			{
				nextBoundingBox.Width += nextBoundingBox.X - x;
				nextBoundingBox.X = x;
			}
		}

		private void ExtensionOfNewBoundBoxForDownSide(Size rectangleSize, int x, int y)
		{
			if (x < nextBoundingBox.X)
			{
				nextBoundingBox.X = x;
				nextBoundingBox.Width += rectangleSize.Width;
			}
			if (y < nextBoundingBox.Y)
			{
				nextBoundingBox.Y = y;
				nextBoundingBox.Height += nextBoundingBox.Y - y;
			}
		}

		private void CreateNewBoundingBox(Size rectangleSize, int x, int y)
		{
			nextBoundingBox.X = x;
			nextBoundingBox.Y = y;
			nextBoundingBox.Width = rectangleSize.Width;
			nextBoundingBox.Height = rectangleSize.Height;
		}
	}
}