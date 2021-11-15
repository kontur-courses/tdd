using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Cloud : IRectanglesCloud
    {
        private readonly List<RectangleF> rectangles;
        public IReadOnlyList<RectangleF> Rectangles => rectangles;
        public PointF Center { get; }

        public Cloud(PointF center)
        {
            Center = center;
            rectangles = new List<RectangleF>();
        }

        public void AddRectangle(RectangleF rectangle) 
            => rectangles.Add(rectangle);

        public void DefaultVisualize(string filename)
        {
            new RectanglesVisualizator(this).DefaultVisualize(filename);
        }

        public void CustomVisualize(string filename,
            Size bitmapSize, 
            List<Color> colors,
            Color backgroundColor,
            bool fillRectangles = false,
            Func<int, List<RectangleF>> getRectanglesByColorIndex = null)
        {
            new RectanglesVisualizator(this).CustomVisualize(filename,
                bitmapSize,
                colors,
                backgroundColor,
                fillRectangles,
                getRectanglesByColorIndex);
        }
    }
}
