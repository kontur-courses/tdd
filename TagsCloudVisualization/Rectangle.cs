namespace TagsCloudVisualization
{
	struct Rectangle
	{
		public Rectangle(double x, double y, double width, double height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public double X { get; }
		public double Y { get; }
		public double Width { get; }
		public double Height { get; }

		public override string ToString()
		{
			return $"Rectangle {nameof(X)}={X}, {nameof(Y)}={Y}, {nameof(Width)}={Width}, {nameof(Height)}={Height}";
		}
	}
}