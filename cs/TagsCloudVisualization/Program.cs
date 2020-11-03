using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using TagsCloudVisualization.PointsGenerators;
using TagsCloudVisualization.TagCloud;

namespace TagsCloudVisualization
{
    class Program
    {
        private static void Main()
        {
            var canvasSize = new Size(500, 500);
            var centerPoint = new Point(canvasSize.Width / 2, canvasSize.Height / 2);
            var pointGenerator = new ArchimedesSpiral(centerPoint, canvasSize);
            
            var cloudLayouter = new CircularCloudLayouter(pointGenerator);
            var rectanglesToAdd = GetSortedRectanglesToAdd(canvasSize);

            foreach (var rectangleSize in rectanglesToAdd) 
                cloudLayouter.PutNextRectangle(rectangleSize);

            TagCloudVisualizer.PrintTagCloud(cloudLayouter, ImageFormat.Png);
        }

        private static IEnumerable<Size> GetSortedRectanglesToAdd(Size canvasSize)
        {
            var rand = new Random();
            var rectangleSizes = new List<Size>();
            
            for (var i = 0; i < 120; i++)
            {
                var width = rand.Next(1, canvasSize.Width / 8);
                var height = rand.Next(1, canvasSize.Height / 8);
                rectangleSizes.Add(new Size(width, height));
            }

            return rectangleSizes.OrderByDescending(rect => rect.Width * rect.Height);
        }
    }
}