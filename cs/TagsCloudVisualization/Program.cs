using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(300, 300));
            cloudLayouter.GenerateRandomCloud(30);
            cloudLayouter.DrawCircularCloud(600, 600);
        }
    }
}