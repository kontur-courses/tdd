using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(100,100));
            cloudLayouter.PutNextRectangle(new Size(2, 3));
            cloudLayouter.PutNextRectangle(new Size(4, 3));
            cloudLayouter.PutNextRectangle(new Size(5, 3));
            cloudLayouter.PutNextRectangle(new Size(3, 3));
            cloudLayouter.PutNextRectangle(new Size(2, 6));
            cloudLayouter.PutNextRectangle(new Size(1, 4));
            cloudLayouter.PutNextRectangle(new Size(3, 4));
            Visualizer.Draw(cloudLayouter.GetCloud(), new Size(200,200));
        }
    }
}