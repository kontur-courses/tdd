using System.Drawing;
using TagsCloudVisualization.Distributions;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var center = new Point(750, 750);
            var distribution = new Spiral(center);
            var cloudLayouter = new CircularCloudLayouter(center, distribution);
            cloudLayouter.GenerateRandomCloud(100);
            cloudLayouter.DrawCircularCloud(1500, 1500, false);
        }
    }
}