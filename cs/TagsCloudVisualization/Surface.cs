using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Surface
    {
        private readonly List<Rectangle> firstQuarterRectangles = new List<Rectangle>();
        private readonly List<Rectangle> secondQuarterRectangles = new List<Rectangle>();
        private readonly List<Rectangle> thirdQuarterRectangles = new List<Rectangle>();
        private readonly List<Rectangle> fourthQuarterRectangles = new List<Rectangle>();

        public void AddRectangle(Rectangle rect)
        {
            var rectQuarters = FindQuartersForRectangle(rect);

            foreach (var quarter in rectQuarters)
            {
                GetRectanglesFromQuarter(quarter).Add(rect);
            }
        }

        public bool RectangleIntersectsWithOther(Rectangle rect)
        {
            var rectQuarters = FindQuartersForRectangle(rect);

            return rectQuarters.Any(quarter =>
                GetRectanglesFromQuarter(quarter).Any(
                    rectFromQuarter => rectFromQuarter.IntersectsWith(rect)));
        }

        public static IEnumerable<Quarters> FindQuartersForRectangle(Rectangle rect)
        {
            var rectangleQuarters = new HashSet<Quarters>();

            foreach (var corner in rect.GetCorners())
            {
                var quarter = GetQuarterForPoint(corner);
                if (quarter != Quarters.Unknown)
                {
                    rectangleQuarters.Add(quarter);
                }
            }

            return rectangleQuarters;
        }

        private List<Rectangle> GetRectanglesFromQuarter(Quarters quarter)
        {
            return quarter switch
            {
                Quarters.First => firstQuarterRectangles,
                Quarters.Second => secondQuarterRectangles,
                Quarters.Third => thirdQuarterRectangles,
                Quarters.Fourth => fourthQuarterRectangles,
                _ => throw new ArgumentException()
            };
        }

        private static Quarters GetQuarterForPoint(Point point)
        {
            return (point.X, point.Y) switch
            {
                var (x, y) when x > 0 && y < 0 => Quarters.First,
                var (x, y) when x < 0 && y < 0 => Quarters.Second,
                var (x, y) when x < 0 && y > 0 => Quarters.Third,
                var (x, y) when x > 0 && y > 0 => Quarters.Fourth,
                _ => Quarters.Unknown
            };
        }

        public enum Quarters
        {
            First,
            Second,
            Third,
            Fourth,
            Unknown
        }
    }
}