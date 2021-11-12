using System.Diagnostics;
using System.Drawing;
using TagsCloudVisualization;

namespace DebugProject
{
    class Program
    {
        static void Main()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
            var painterOfRectangles = new PainterOfRectangles(new Size(1000, 1000));
            var generator = new GeneratorOfRectangles();

            for (int i = 0; i < 2000; i++)
            {
                circularCloudLayouter.PutNextRectangle(generator.GetRectangleSize(10, 10));
            }

            painterOfRectangles.CreateImage(circularCloudLayouter.Rectangles, "CircularCloudLayouter1.png");

            var p = new Process();
            p.StartInfo = new ProcessStartInfo("CircularCloudLayouter1.png")
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}