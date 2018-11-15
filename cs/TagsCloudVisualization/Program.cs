using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(50, 100));
            var rectangleLayout = new[]
            {
                layouter.PutNextRectangle(new Size(100, 100)),
                layouter.PutNextRectangle(new Size(100, 100)),
                layouter.PutNextRectangle(new Size(100, 20))
            };
            var image = new RectangleLayoutVisualizer().Vizualize(new Size(700, 700), 
                rectangleLayout, new Point(50, 100));
            new ImageSaver().Save(image, "picture.png");
        }
    }
}
