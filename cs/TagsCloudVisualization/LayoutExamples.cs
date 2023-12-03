using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class LayoutExamples
    {
        public static void Main()
        {
            GenerateRectanglesWithRandomSizes(50, 20, 200);
            GenerateManySmallSameSizedRectangles();
            GenerateVerybigThenSmallRectangles();
        }

        public static void GenerateRectanglesWithRandomSizes(int amountRectangles, int minSizeParam, int maxSizeParam)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point());
            var random = new Random();

            for (var i = 0; i < amountRectangles; i++)
            {
                var rectWidth = random.Next(minSizeParam, maxSizeParam);
                var rectHeight = random.Next(minSizeParam, maxSizeParam);
                circularCloudLayouter.PutNextRectangle(new Size(rectWidth, rectHeight));
            }

            circularCloudLayouter.CreateImageOfLayout("Random rectangles");
        }

        public static void GenerateManySmallSameSizedRectangles()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point());

            for (var i = 0; i < 200; i++)
            {
                circularCloudLayouter.PutNextRectangle(new Size(30, 30));
            }
            circularCloudLayouter.CreateImageOfLayout("Many small rectangles");
        }

        // Worst scenario
        public static void GenerateVerybigThenSmallRectangles()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point());

            for (var i = 0; i < 5; i++)
            {
                circularCloudLayouter.PutNextRectangle(new Size(150, 300));
                for (var j = 0; j < 5; j++)
                    circularCloudLayouter.PutNextRectangle(new Size(20, 20));
            }
            circularCloudLayouter.CreateImageOfLayout("Small rectangles after very big");
        }
    }
}
