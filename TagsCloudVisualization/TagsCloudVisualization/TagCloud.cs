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
         
        private FrequencyTags tags;
        private List<TextRectangle> rectangles;
        private Size srcSize;
        private List<Point> emptyPoints ;

        public TagCloud(FrequencyTags tags = null)
        {
            emptyPoints = new List<Point>();
            this.tags = tags;
            rectangles = new List<TextRectangle>();
        }

        public List<TextRectangle> GetRectangles() => rectangles;
        public Size GetScreenSize() => srcSize;

        public void CreateTagCloud(int heightSize=1000)
        {
            srcSize = new Size(heightSize* 2, heightSize);
            var arithmeticSpiral = new ArithmeticSpiral(new Point(srcSize / 2));
            var sizeDictionary = new DivideTags((srcSize.Height + srcSize.Width) * 2, tags).sizeDictionary;
            var circularCloudLayouter = new CircularCloudLayouter(sizeDictionary, srcSize);
            var nextSizeRectangle = circularCloudLayouter.GetRectangleOptions();
            var filledEmptySpaces = false;
            while (true)
            {
                if (TryFillRectangle(arithmeticSpiral, circularCloudLayouter, ref nextSizeRectangle, ref filledEmptySpaces)) 
                    break;
            }
        }

        private bool TryFillRectangle(ArithmeticSpiral arithmeticSpiral, CircularCloudLayouter circularCloudLayouter,
            ref Tuple<string, Size, Font> nextSizeRectangle, ref bool filledEmptySpaces)
        {
            var point = arithmeticSpiral.GetPoint();
            if (nextSizeRectangle.Item2.IsEmpty)
                return true;
            filledEmptySpaces = FillEmptySpaces(filledEmptySpaces, circularCloudLayouter, ref nextSizeRectangle);
            if (!rectangles.Select(x => x.rectangle.Contains(point)).Contains(true))
                emptyPoints.Add(point);
            if (!Contains(rectangles, point, nextSizeRectangle.Item2))
                filledEmptySpaces = AddRectangle(point, circularCloudLayouter, ref nextSizeRectangle);
            return false;
        }

        private bool FillEmptySpaces(bool filledEmptySpaced, CircularCloudLayouter circularCloudLayouter,
            ref Tuple<string, Size, Font> nextSizeRectangle)
        {
            if (filledEmptySpaced && emptyPoints.Any())
            {
                for (var i = 0; i < emptyPoints.Count; i++)
                    if (!Contains(rectangles, emptyPoints[i], nextSizeRectangle.Item2))
                    {
                        if (nextSizeRectangle.Item2.IsEmpty)
                            break;
                        AddRectangle(emptyPoints[i], circularCloudLayouter, ref nextSizeRectangle);
                    }

                filledEmptySpaced = false;
            }

            return filledEmptySpaced;
        }

        private bool AddRectangle(Point point, CircularCloudLayouter circularCloudLayouter, ref Tuple<string, Size, Font> nextSizeRectangle)
        {
            bool clearused;
            var rectangle = new Rectangle(point - nextSizeRectangle.Item2 / 2, nextSizeRectangle.Item2);
            var textRectangle = new TextRectangle(rectangle, nextSizeRectangle.Item1, nextSizeRectangle.Item3);
            rectangles.Add(textRectangle);
            nextSizeRectangle = circularCloudLayouter.GetRectangleOptions();
            clearused = true;
            for (var i = 0; i < emptyPoints.Count; i++)
                if (textRectangle.rectangle.Contains(emptyPoints[i]))
                    emptyPoints.Remove(emptyPoints[i]);
            return clearused;
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
