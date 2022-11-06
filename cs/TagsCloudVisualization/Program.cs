using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(750, 750));
            cloudLayouter.GenerateRandomCloud(100);
            cloudLayouter.DrawCircularCloud(1500, 1500);
        }
    }
}