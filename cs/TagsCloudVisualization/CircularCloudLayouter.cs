using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<LayoutItem> Items;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Items = new List<LayoutItem>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newItem = new LayoutItem(new Rectangle(default, rectangleSize));
            Items.Add(newItem);
            ReallocRectangles();
            return newItem.Rectangle;
        }

        public void PutRectangles(IEnumerable<Size> sizes)
        {
            Items.AddRange(sizes.Select(size => new LayoutItem(new Rectangle(default, size))));
            ReallocRectangles();
        }

        private void ReallocRectangles()
        {
            if (Items.Count == 0) return;

            Items.Sort((i1, i2) => i2.Rectangle.Square().CompareTo(i1.Rectangle.Square()));

            var biggestItem = Items[0];
            biggestItem.Rectangle.X = -biggestItem.Rectangle.Width / 2;
            biggestItem.Rectangle.Y = -biggestItem.Rectangle.Height / 2;

            var rnd = new Random();
            for (var i = 1; i < Items.Count; i++)
            {
                var size = Items[i].Rectangle.Size;
                var minVertexDist = double.MaxValue;
                Rectangle bestRect = default;

                for (var angle = rnd.NextDouble() * Math.PI / 18; angle < 1.99 * Math.PI; angle += (Math.PI / 18))
                {
                    var farthestIntersectionPointDistance = Items
                        .Take(i)
                        .Select(it => it.Rectangle.IsIntersectsByRay(angle, out double intersectionPointDistance) ? intersectionPointDistance : 0)
                        .Max();
                    var r = Utils.LengthOfRayFromCenterOfRectangle(Items[i].Rectangle, angle);
                    const int step = 2;
                    var dist = farthestIntersectionPointDistance + r;
                    Rectangle newRect;
                    do
                    {
                        dist += step;
                        var location = new Point().FromPolar(angle, dist);
                        location.Offset(-size.Width / 2, -size.Height / 2);
                        newRect = new Rectangle(location, size);
                    } while (Items.Take(i).Select(it => it.Rectangle).Any(rect => newRect.IntersectsWith(rect)));

                    var farthestVertexDist = newRect.GetDistanceOfFathestFromCenterVertex();
                    if (farthestVertexDist < minVertexDist)
                    {
                        minVertexDist = farthestVertexDist;
                        bestRect = newRect;
                    }
                }

                Items[i].Rectangle = bestRect;
            }
        }

        public IEnumerable<Rectangle> GetRectangles() => Items.Select(it => it.Rectangle);
    }
}
