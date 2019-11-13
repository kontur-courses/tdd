using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main(string[] args)
        {
            var sizes = Generator.GetRandomSizesList(20, 20, 20, 20, 100, new Random());
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = layouter.PutNextRectangles(sizes);
            RectanglesVisualizer.SaveNewRectanglesLayout(rectangles, "Images","Image.png");
        }
    }
}
