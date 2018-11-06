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
                var layouter = new CircularCloudLayouter(new Point(1000, 1000));
                var rectangles = CircularCloudLayouterDrawer.GenerateRectanglesSet(layouter, 50, 75, 100, 25, 40);
                CircularCloudLayouterDrawer.DrawRectanglesSet(layouter.Size, $"tag-cloud-{i + 1}.png", rectangles);
            }
        }
        
    }
}
