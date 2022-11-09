using System.Drawing;
using TagsCloudVisualization;

namespace CircularCloudLayoutTests
{
    public class SetUpBuilder
    {
        private int layoutRadius;
        private Point center;
        private CircularCloudLayout layout;
        private List<Rectangle> placedRectangles;
        public SetUpBuilder WithCenter(Point center)
        {
            this.center = center;
            layoutRadius = center.X < center.Y ? center.X : center.Y;
            layout = new CircularCloudLayout(center);
            placedRectangles = new();
            return this;
        }
        public SetUpBuilder WithCustomSizes()
        {
            SizeListBulder.GetCustomSizes().ForEach(x =>
            {
                if (layout.PutNextRectangle(x, out Rectangle rect))
                    placedRectangles.Add(rect);
            });
            return this;
        }

        public Point GetCenter() => center;
        public int GetRadius() => layoutRadius;
        public CircularCloudLayout GetLayout() => layout;
        public List<Rectangle> GetRectangles() => placedRectangles;
    }
}
