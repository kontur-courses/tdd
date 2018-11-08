using System.Collections.Generic;
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
		public readonly List<Rectangle> AllRectangles;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			boundingBox = new Rectangle(center.X, center.Y, 0, 0);
			nextBoundingBox = new Rectangle(0, 0, 0, 0);
			startPointer = new Point(boundingBox.X - boundingBox.Width, boundingBox.Y);
			AllRectangles = new List<Rectangle>();
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
					ExtendOfNewBoundBoxForDownSide(rectangleSize, x, y);
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
					ExtendOfNewBoundBoxForLeftSide(rectangleSize, x, y);
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
					ExtendOfNewBoundBoxForUpSide(rectangleSize, x, y);
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
					ExtendForNewBoundBoxForRightSide(rectangleSize, x, y);
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

			var rectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
			AllRectangles.Add(rectangle);
			return rectangle;
		}

		private void ExtendForNewBoundBoxForRightSide(Size rectangleSize, int x, int y)
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
		}

		private bool IsWidthOfRectOutsideNewBoundBox(ref Size rectangleSize, int x)
		{
			return x + rectangleSize.Width > nextBoundingBox.X + nextBoundingBox.Width;
		}

		private void ExtendOfNewBoundBoxForUpSide(Size rectangleSize, int x, int y)
		{
			if (IsWidthOfRectOutsideNewBoundBox(ref rectangleSize, x))
			{
				var deltaX = x + rectangleSize.Width - nextBoundingBox.X - nextBoundingBox.Width;
				nextBoundingBox.Width += deltaX;
			}
			if (IsHeightOfRectOutsideNewBoundBox(ref rectangleSize, y))
			{
				var deltaY = y + rectangleSize.Height - nextBoundingBox.Y - nextBoundingBox.Height;
				nextBoundingBox.Height += deltaY;
			}
		}

		private bool IsHeightOfRectOutsideNewBoundBox(ref Size rectangleSize, int y)
		{
			return y + rectangleSize.Height > nextBoundingBox.Y + nextBoundingBox.Height;
		}

		private void ExtendOfNewBoundBoxForLeftSide(Size rectangleSize, int x, int y)
		{
			if (IsHeightOfRectOutsideNewBoundBox(ref rectangleSize, y))
				nextBoundingBox.Height += rectangleSize.Height;
			if (x < nextBoundingBox.X)
			{
				nextBoundingBox.Width += nextBoundingBox.X - x;
				nextBoundingBox.X = x;
			}
		}

		private void ExtendOfNewBoundBoxForDownSide(Size rectangleSize, int x, int y)
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