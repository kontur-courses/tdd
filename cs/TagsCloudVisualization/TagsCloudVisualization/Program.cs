﻿using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var rectangleSizes = RectangleSizeProvider.GetRandomSizes(888, 500, 100);

            IRectangleLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
            foreach (var size in rectangleSizes)
                layouter.PutNextRectangle(size);

            var visualizer = new LayoutVisualizer(layouter.Rectangles);
            var visualizationPath = args[0];
            visualizer.SaveAs(visualizationPath);
        }
    }
}