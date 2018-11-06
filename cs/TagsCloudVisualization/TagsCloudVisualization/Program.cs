using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(100, 100));
            circularCloudLayouter.PutNextRectangle(new Size(50, 60));
            circularCloudLayouter.PutNextRectangle(new Size(50, 60));
            circularCloudLayouter.PutNextRectangle(new Size(50, 60));
            circularCloudLayouter.PutNextRectangle(new Size(50, 60));
            circularCloudLayouter.PutNextRectangle(new Size(50, 60));
            circularCloudLayouter.PutNextRectangle(new Size(50, 60));
            circularCloudLayouter.PutNextRectangle(new Size(50, 60));
            CloudPainter.CreateNewTagCloud(circularCloudLayouter);
        }
    }
}