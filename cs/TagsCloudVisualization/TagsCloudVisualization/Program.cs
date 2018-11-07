using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(100, 100));
            circularCloudLayouter.PutNextRectangle(new Size(50, 60));
            circularCloudLayouter.PutNextRectangle(new Size(70, 30));
            circularCloudLayouter.PutNextRectangle(new Size(20, 100));
            circularCloudLayouter.PutNextRectangle(new Size(80, 60));
            circularCloudLayouter.PutNextRectangle(new Size(50, 100));
            circularCloudLayouter.PutNextRectangle(new Size(110, 100));
            circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            CloudPainter.CreateNewTagCloud(circularCloudLayouter);
        }
    }
}