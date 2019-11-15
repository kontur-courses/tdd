using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            DrawRectangles(100,100, 1);
            DrawRectangles(50, 50, 2);
            DrawRectangles(30, 30, 3);
        }

        private static void DrawRectangles(int widthMax ,int heightMax, int numberOfPicture)
        {
            var center = new Point(0, 0);
            var cloudLayouter = new CircularCloudLayouter(center);
            var sizes = new List<Size>();

            for (var width = 5; width < widthMax; width += 5)
            {
                for (var height = 5; height < heightMax; height += 5)
                {
                    sizes.Add(new Size(width, height));
                }
            }
            sizes.ForEach(s => cloudLayouter.PutNextRectangle(s));

            string workingDirectory = Environment.CurrentDirectory;
            TagDrawer.Draw(workingDirectory + "\\cloud"+numberOfPicture+".bmp", cloudLayouter);
        }
    }
}
