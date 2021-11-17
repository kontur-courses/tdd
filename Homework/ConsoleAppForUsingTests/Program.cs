using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Visualization;

namespace ConsoleAppForTests
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(400, 300));
            var visualizator = new CircularCloudVisualizator(800, 600, Color.Black);

            var rectangles = new List<RectangleF>();
            
            Console.Write("Input rectangles count: ");

            var rectanglesCount = int.Parse(Console.ReadLine());

            for (var i = 0; i < rectanglesCount; ++i)
            {
                rectangles.Add(layouter.PutNextRectangle(new SizeF(30, 50)));
            }

            var strs = GetRandomStrings(rectanglesCount);

            visualizator.PutWordsInRectangles(strs, rectangles);

            visualizator.SaveImage();
        }

        public static List<String> GetRandomStrings(int stringsCount)
        {
            var random = new Random();
            var strs = new List<string>();

            var startSymbol = '0';
            var endSymbol = 'Z';
            
            for (var i = 0; i < stringsCount; ++i)
            {
                var builder = new StringBuilder();
                for (var j = 0; j < 5; ++j)
                    // Random symbol from '0' to 'Z' in ASCII
                    builder.Append((char) random.Next(startSymbol, endSymbol));
                strs.Add(builder.ToString());
            }

            return strs;
        }
    }
}