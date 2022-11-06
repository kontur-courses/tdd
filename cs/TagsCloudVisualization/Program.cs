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

            var painter = new Painter(new Size(500, 500), pen1, pen2);
            painter.CreatePicture(100, ImagesFolder + "100.png");
            painter = new Painter(new Size(1000, 1000), pen1, pen2);
            painter.CreatePicture(300, ImagesFolder + "300.png");
        }
    }
}
