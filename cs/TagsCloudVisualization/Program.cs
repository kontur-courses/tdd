using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        private const string ImagesFolder = "../../../result/";

        static void Main(string[] args)
        {

            var pen1 = new Pen(Brushes.DeepSkyBlue);
            var pen2 = new Pen(Brushes.Red);
            var generator = new RectangleGenerator(new Size(10, 5), new Size(30, 30));
            var painter = new Painter(new Size(500, 500), pen1, pen2, generator);
            painter.CreatePicture(100, ImagesFolder + "100.png");
            painter = new Painter(new Size(9000, 9000), pen1, pen2, generator);
            painter.CreatePicture(10000, ImagesFolder + "10000.png");
        }
    }
}
