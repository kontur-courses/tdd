using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                DrawRectanglesSet(new CircularCloudLayouter(new Point(1000, 1000)), $"tag-cloud-{i+1}.png");
            }
        }

        private static void DrawRectanglesSet(CircularCloudLayouter layouter, string outputFileName)
        {
            var rectangles = new List<Rectangle>();
            var random = new Random();

            for (var i = 0; i < 50; i++)
            {
                var randomSize = new Size(random.Next(75, 100), random.Next(25, 40));
                var newRectangle = layouter.PutNextRectangle(randomSize);
                rectangles.Add(newRectangle);
            }

            var bitmap = new Bitmap(layouter.Size.Width, layouter.Size.Height);
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
}
