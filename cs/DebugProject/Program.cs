using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using TagsCloudVisualization;

namespace DebugProject
{
    class Program
    {
        static void Main()
        {
            var painter = new PainterOfRectangles(new Size(1000, 1000));
            var centrePoint = new Point(500, 500);
            var spiral = new ArchimedesSpiral(centrePoint);
            var circularCloudLayouter = new CircularCloudLayouter(centrePoint, spiral);
            var rectangles = new List<Rectangle>();
            var saver = new SaverImage("CircularCloudLayouter1.png");

            var generatorRectangle = new GeneratorOfRectangles();

            for (int i = 0; i < 2000; i++)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(generatorRectangle.GetSize(10, 10)));
            }

            painter.CreateImage(rectangles, saver);

            var openImageProcess = new Process();
            openImageProcess.StartInfo = new ProcessStartInfo("CircularCloudLayouter1.png")
            {
                UseShellExecute = true
            };
            openImageProcess.Start();
        }
    }
}