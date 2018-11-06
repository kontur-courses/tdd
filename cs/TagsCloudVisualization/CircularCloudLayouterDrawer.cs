using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class CircularCloudLayouterDrawer
    {
        public static void DrawRectanglesSet(Size size, string outputFileName,
            List<Rectangle> rectangles)
        {
            using (var bitmap = new Bitmap(size.Width, size.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    foreach (var rectangle in rectangles)
                    {
                        graphics.DrawRectangle(new Pen(Color.Red), rectangle);
                    }
                    bitmap.Save(outputFileName);
                }
            }
        }

        public static List<Rectangle> GenerateRectanglesSet(CircularCloudLayouter layouter, int count,
            int widthBottomBound, int widthTopBound, int heightBottomBound, int heightTopBound)
        {
            var rectangles = new List<Rectangle>();
            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                var randomSize = new Size(random.Next(widthBottomBound, widthTopBound),
                    random.Next(heightBottomBound, heightTopBound));
                var newRectangle = layouter.PutNextRectangle(randomSize);
                rectangles.Add(newRectangle);
            }

            return rectangles;
        }
    }
}
