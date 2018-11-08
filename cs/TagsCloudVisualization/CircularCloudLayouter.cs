using System.Drawing;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		private readonly Point center;
		private Rectangle bountyBox;
		private Rectangle nextBountyBox;
		private Point prev;
		private BoxSide side = BoxSide.Down;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			bountyBox = new Rectangle(center.X, center.Y, 0, 0);
			nextBountyBox = new Rectangle(0, 0, 0, 0);
			prev = new Point(bountyBox.X - bountyBox.Width, bountyBox.Y);
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			var x = 0;
			var y = 0;
			switch (side)
			{
				case BoxSide.Down:
					x = prev.X - rectangleSize.Width;
					y = bountyBox.Y - rectangleSize.Height;
					prev = new Point(x, y);
					if (nextBountyBox.Width == 0)
					{
						nextBountyBox.X = x;
						nextBountyBox.Y = y;
						nextBountyBox.Width = rectangleSize.Width;
						nextBountyBox.Height = rectangleSize.Height;
					}
					if (x < nextBountyBox.X)
					{
						nextBountyBox.X = x;
						nextBountyBox.Width += rectangleSize.Width;
					}
					if (y < nextBountyBox.Y)
					{
						nextBountyBox.Y = y;
						nextBountyBox.Height += rectangleSize.Height - nextBountyBox.Height;
					}

					if (x < bountyBox.X)
					{
						side = BoxSide.Left;
						prev.Y = y + rectangleSize.Height;
						prev.X = bountyBox.X;
					}
					break;
				case BoxSide.Left:
					x = prev.X - rectangleSize.Width;
					y = prev.Y;
					prev.Y = y + rectangleSize.Height;
					if (y + rectangleSize.Height > nextBountyBox.Y)
						nextBountyBox.Height += rectangleSize.Height;
					if (x < nextBountyBox.X)
					{
						nextBountyBox.Width += nextBountyBox.X - x;
						nextBountyBox.X = x;
					}

					if (y + rectangleSize.Height >= bountyBox.Y + bountyBox.Height)
					{
						side = BoxSide.Up;
						prev.X = bountyBox.X;
						prev.Y = bountyBox.Y + bountyBox.Height;
					}
					break;
				case BoxSide.Up:
					x = prev.X;
					y = prev.Y;
					prev.X = x + rectangleSize.Width;

					if (x + rectangleSize.Width > nextBountyBox.X + nextBountyBox.Width)
						nextBountyBox.Width += x + rectangleSize.Width - nextBountyBox.X - nextBountyBox.Width;
					if (y + rectangleSize.Height > nextBountyBox.Y + nextBountyBox.Height)
						nextBountyBox.Height += y + rectangleSize.Height - nextBountyBox.Y - nextBountyBox.Height;
					if (x + rectangleSize.Width > bountyBox.X + bountyBox.Width)
					{
						side = BoxSide.Rigth;
						prev.X = bountyBox.X + bountyBox.Width;
						prev.Y = bountyBox.Y + bountyBox.Height;
					}
					break;
				case BoxSide.Rigth:
					x = prev.X;
					y = prev.Y - rectangleSize.Height;
					prev.Y = y;
					if (x + rectangleSize.Width > nextBountyBox.X + nextBountyBox.Width)
						nextBountyBox.Width += x + rectangleSize.Width - nextBountyBox.X - nextBountyBox.Width;
					if (y < nextBountyBox.Y)
						nextBountyBox.Y = y;
					if (y < bountyBox.Y)
					{
						side = BoxSide.Down;
						bountyBox = new Rectangle(nextBountyBox.X, nextBountyBox.Y, nextBountyBox.Width, nextBountyBox.Height);
						nextBountyBox = new Rectangle(0, 0, 0, 0);
						prev.X = bountyBox.X + bountyBox.Width;
						prev.Y = bountyBox.Y;
					}
					break;
			}

			return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
		}
	}
}