using System.Drawing;
using TagCloud_TestDataGenerator;

namespace TagCloud
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var vs = new LayoutVisualizator();

            var layouter = new CircularCloudLayouter(new Point(0, 0));

            foreach (var size in DataGenerator.GetNextSize())
                layouter.PutNextRectangle(size);

            vs.VisualizeCloud(layouter.GetRectangles());
            vs.SaveToDesktop();
        }
    }
}