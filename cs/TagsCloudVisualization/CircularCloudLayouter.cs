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
        private bool isUpdated;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Items = new List<LayoutItem>();
            isUpdated = false;
        }

        public void PutNextRectangle(Size rectangleSize)
        {
            var newItem = new LayoutItem(new Rectangle(default, rectangleSize));
            Items.Add(newItem);
            isUpdated = false;
        }

        public void PutRectangles(IEnumerable<Size> sizes)
        {
            Items.AddRange(sizes.Select(size => new LayoutItem(new Rectangle(default, size))));
            isUpdated = false;
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
                    var closestRectOnRay = GetClosestPlaceWithoutIntersects(angle, size, i);
                    var farthestVertexDist = closestRectOnRay.GetDistanceOfFathestFromCenterVertex();
                    if (farthestVertexDist < minVertexDist)
                    {
                        minVertexDist = farthestVertexDist;
                        bestRect = closestRectOnRay;
                    }
                }

                Items[i].Rectangle = bestRect;
            }

            isUpdated = true;
        }

        private Rectangle GetClosestPlaceWithoutIntersects(double rayAngle, Size size, int intersectCheckingRectanglesCount)
        {
            var checkingItems = Items.Take(intersectCheckingRectanglesCount);

            var farthestIntersectionPointDistance = checkingItems
                .Max(it => it.Rectangle.GetDistanceIfIntersectsByRay(rayAngle));

            var r = Utils.LengthOfRayFromCenterOfRectangle(size, rayAngle);
            const int step = 2;
            var dist = farthestIntersectionPointDistance + r;
            Rectangle tryRect;
            do
            {
                dist += step;
                var location = new Point().FromPolar(rayAngle, dist);
                location.Offset(-size.Width / 2, -size.Height / 2);
                tryRect = new Rectangle(location, size);
            } while (checkingItems.Any(it => tryRect.IntersectsWith(it.Rectangle)));

            return tryRect;
        }

        public IEnumerable<Rectangle> GetRectangles()
        {
            if (!isUpdated)
                ReallocRectangles();

            return Items.Select(it => it.Rectangle);
        }
    }
}
