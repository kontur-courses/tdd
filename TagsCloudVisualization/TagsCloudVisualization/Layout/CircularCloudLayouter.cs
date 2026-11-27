using System.Drawing;

namespace TagsCloudVisualization.Layout
{
	public class CircularCloudLayouter
	{
		private readonly Point center;
		private readonly ISpiral spiral;
		private readonly IRectangleCompactor compactor;
		private readonly List<Rectangle> rectangles = new();
		public IReadOnlyList<Rectangle> Rectangles {  get { return rectangles; } }

		public CircularCloudLayouter(Point center, ISpiral? spiral = null, IRectangleCompactor? compactor = null)
		{
			this.center = center;
			this.spiral = spiral ?? new ArchimedeanSpiral(center);
			this.compactor = compactor ?? new RectangleCompactor();
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			throw new NotImplementedException();
		}
	}
}