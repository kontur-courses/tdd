using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        public Point Center { get; private set; }
        private List<Rectangle> _rectangles = new List<Rectangle>();
        private IReadOnlyList<Rectangle> Rectangles => _rectangles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new Exception();
        }

        public List<Rectangle> GetArrangedRectangles()
        {
            throw new Exception();
        }

        // Refactor to another class
        public Bitmap MakeVisualisation()
        {
            throw new Exception();
        }

        public void Save()
        {
            throw new Exception();
        }
    }
}