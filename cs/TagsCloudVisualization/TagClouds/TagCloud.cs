using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualization.TagClouds
{
    public class TagCloud : IEnumerable<Rectangle>
    {
        public int Count => Rectangles.Count;

        protected List<Rectangle> Rectangles = new List<Rectangle>();
        private readonly ILayouter layouter;
        private Point leftUpBound = new Point(int.MaxValue, int.MaxValue);
        private Point rightDownBound = new Point(int.MinValue, int.MinValue);

        public Point LeftUpBound => new Point(leftUpBound.X, leftUpBound.Y);
        public Point RightDownBound => new Point(rightDownBound.X, rightDownBound.Y);

        public TagCloud(ILayouter layouter)
        {
            this.layouter = layouter;
        }

        public virtual Rectangle PutNextRectangle(Size size)
        {
            var rectangle = layouter.PutNextRectangle(size);
            leftUpBound.X = Math.Min(leftUpBound.X, rectangle.X);
            leftUpBound.Y = Math.Min(leftUpBound.Y, rectangle.Y);
            rightDownBound.X = Math.Max(rightDownBound.X, rectangle.X + rectangle.Width);
            rightDownBound.Y = Math.Max(rightDownBound.Y, rectangle.Y + rectangle.Height);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        public IEnumerator<Rectangle> GetEnumerator()
        {
            return ((IEnumerable<Rectangle>) Rectangles).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
