using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{

    internal class TagCloud
    {
        private List<TextRectangle> rectangles;
        private Size srcSize;
        private List<Point> emptyPoints;

        public TagCloud()
        {
            emptyPoints = new List<Point>();
            rectangles = new List<TextRectangle>();
        }

        public List<TextRectangle> GetRectangles() => rectangles;
        public Size GetScreenSize() => srcSize;

        public void CreateTagCloud(CircularCloudLayouter circularCloudLayouter)
        {
            var arithmeticSpiral = new ArithmeticSpiral(new Point(0, 0));
            var nextSizeRectangle = circularCloudLayouter.GetNextRectangleOptions().GetEnumerator();
            nextSizeRectangle.MoveNext();
            bool? filledEmptySpaces = false;
            while (true)
                if (TryFillRectangle(arithmeticSpiral, nextSizeRectangle, ref filledEmptySpaces))
                    break;
            var maxSize = (int)(emptyPoints.MaxBy(x => x.X).X * 2.5);
            srcSize = new Size(maxSize, maxSize);
        }

        private bool TryFillRectangle(ArithmeticSpiral arithmeticSpiral,
            IEnumerator<Tuple<string, Size, Font>> nextSizeRectangle, ref bool? nextIteration)
        {
            var point = arithmeticSpiral.GetPoint();
            if (nextIteration == null)
                return true;
            nextIteration = FillEmptySpaces(nextIteration, nextSizeRectangle);
            if (!rectangles.Select(x => x.rectangle.Contains(point)).Contains(true))
                emptyPoints.Add(point);
            if (!Contains(rectangles, point, nextSizeRectangle.Current.Item2))
                nextIteration = AddRectangle(point, nextSizeRectangle);
            return false;
        }

        private bool? FillEmptySpaces(bool? filledEmptySpaced, IEnumerator<Tuple<string, Size, Font>> nextSizeRectangle)
        {
            if (filledEmptySpaced.Value && emptyPoints.Any())
            {
                for (var i = 0; i < emptyPoints.Count; i++)
                    if (!Contains(rectangles, emptyPoints[i], nextSizeRectangle.Current.Item2))
                        AddRectangle(emptyPoints[i], nextSizeRectangle);
                filledEmptySpaced = false;
            }

            return filledEmptySpaced;
        }

        private bool? AddRectangle(Point point, IEnumerator<Tuple<string, Size, Font>> nextSizeRectangle)
        {
            var rectangle = new Rectangle(point - nextSizeRectangle.Current.Item2 / 2, nextSizeRectangle.Current.Item2);
            var textRectangle = new TextRectangle(rectangle, nextSizeRectangle.Current.Item1,
                nextSizeRectangle.Current.Item3);
            if (!nextSizeRectangle.MoveNext())
                return null;
            rectangles.Add(textRectangle);
            for (var i = 0; i < emptyPoints.Count; i++)
                if (textRectangle.rectangle.Contains(emptyPoints[i]))
                    emptyPoints.Remove(emptyPoints[i]);
            return true;
        }

        private static bool Contains(List<TextRectangle> rectangles, Point point,
            Size size)
        {
            return rectangles
                .Select(x =>
                    x.rectangle.IntersectsWith(new Rectangle(point - size / 2, size)))
                .Contains(true);
        }
    }
}
