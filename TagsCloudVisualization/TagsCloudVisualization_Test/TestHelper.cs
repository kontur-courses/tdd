using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization_Test
{
    class TestHelper
    {
        public static List<Size> GenerateSizes(int tagsCount)
        {
            var sises = new List<Size>();
            var rnd = new Random();
            for (int i = 0; i < tagsCount; i++)
            {
                var width = rnd.Next(14, 60);
                var height = rnd.Next(10, width);
                sises.Add(new Size(width, height));
            }
            return sises;
        }

        public static double GetDensityFactor(List<Rectangle> rectangles, Point center)
        {
            var union = rectangles.First();
            double squareSum = 0;
            foreach (var r in rectangles)
            {
                union = Rectangle.Union(union, r);
                squareSum += r.Width * r.Height;
            }
            var radius = union.GetDistancesToPoint(center).Average();
            var sphereSquare = Math.PI * radius * radius;
            return squareSum / sphereSquare;
        }

        public static List<Rectangle> CheckIntersects(List<Rectangle> rectangles)
        {
            var intersects = new List<Rectangle>();
            foreach (var first in rectangles)
                foreach (var second in rectangles)
                {
                    if (first == second)
                        continue;
                    if (first.IntersectsWith(second))
                    {
                        var squareFirst = first.Width * first.Height;
                        var itersect = first.GetIntersection(second);
                        if (first.Width * first.Height != squareFirst)
                            throw new InvalidOperationException("Был изменен объект вместо создания нового");
                        intersects.Add(itersect);
                    }
                }
            return intersects;
        }

        public static Rectangle UnionAll(List<Rectangle> rectangles)
        {
            var union = rectangles.First();
            foreach (var r in rectangles)
                union = Rectangle.Union(union, r);
            return union;
        }
    }
}
