using System;
using System.Drawing;

namespace ProjectCircularCloudLayouter
{
    public class GenerateRectangles
    {
        public static void MakeLayouter(CircularCloudLayouter layouter, int countRectangles,
            int minRectangleWidth, int maxRectangleWidth,
            int minRectangleHeight, int maxRectangleHeight)
        {
            var random = new Random();
            for (var i = 0; i < countRectangles; i++)
            {
                layouter.PutNextRectangle(new Size(random.Next(minRectangleWidth, maxRectangleWidth),
                    random.Next(minRectangleHeight, maxRectangleHeight)));
            }
        }
    }
}