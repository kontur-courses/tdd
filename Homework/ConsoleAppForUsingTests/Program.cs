using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Visualization;
using TagsCloudVisualizationTests;

namespace ConsoleAppForTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(400, 300));
            var visualizator = new CircularCloudVisualizator();

            var rectangles = new List<RectangleF>();

            for (var i = 0; i < 100; ++i)
            {
                rectangles.Add(layouter.PutNextRectangle(new SizeF(30, 50)));
            }

            var random = new Random();

            var strs = new List<string>();

            for (var i = 0; i < 100; ++i)
            {
                var builder = new StringBuilder();
                for (var j = 0; j < 5; ++j)
                    builder.Append((char) random.Next(33, 126));
                strs.Add(builder.ToString());
            }

            visualizator.PutWordsInRectangles(strs, rectangles);
            
            visualizator.SaveImage();
        }
    }
}