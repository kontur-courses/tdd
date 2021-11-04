using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloud_TestDataGenerator;

namespace TagCloud
{
    class Program
    {
        static void Main(string[] args)
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
