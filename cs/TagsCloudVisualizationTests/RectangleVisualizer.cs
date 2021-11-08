using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualizationTests
{
    public class RectangleVisualizer : IVisualizer
    {
        public IRectangleStyle Style { get; set; } = new RedPenStyle();

        private readonly List<Rectangle> rectangles;

        public RectangleVisualizer(IEnumerable<Rectangle> rectangles)
        {
            this.rectangles = rectangles.ToList();
        }

        public void Draw(Graphics graphics)
        {
            var offset = PointHelper.GetTopLeftAge(rectangles.Select(rectangle => rectangle.Location));
            rectangles
                .Select(rectangle =>
                    new Rectangle(new Point(rectangle.Left - offset.X, rectangle.Top - offset.Y), rectangle.Size))
                .ToList()
                .ForEach(rectangle => Style.Draw(graphics, rectangle));
        }

        public Size GetBitmapSize()
        {
            var topLeft = PointHelper.GetTopLeftAge(rectangles.Select(rectangle => rectangle.Location));
            var bottomRight = PointHelper.GetBottomRightAge(rectangles
                .Select(rectangle => new Point(rectangle.Right, rectangle.Bottom)));

            return new Size(
                bottomRight.X - topLeft.X + 1,
                bottomRight.Y - topLeft.Y + 1);
        }
    }
}