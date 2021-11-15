using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace TagsCloudVisualization
{
    class Program
    {
        private static void CreateCloudImage(string name, Point cloudCenter, int rectangleCount)
        {
            


            var directoryPath = @"..\..\..\..";

            var cloud = new CircularCloudLayouter(cloudCenter);
            var rnd = new Random();

            var visualizator = new CloudVisualizer(new Size(1000, 1000), cloudCenter);
            var pen = new Pen(new SolidBrush(Color.Red));

            for (var i = 0; i < rectangleCount; i++)
            {
                var rectangle = cloud.PutNextRectangle(new Size(rnd.Next(50, 100), rnd.Next(50, 100)));
                visualizator.DrawRectangle(pen, rectangle);
            }

            visualizator.GetImage().Save(@$"{Path.GetFullPath(directoryPath)}\{name}.png");
        }

        static void Main(string[] args)
        {
            CreateCloudImage("first", new Point(), 25);
            CreateCloudImage("second", new Point(25, 50), 50);
        }

    }
}
