using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main(string[] args)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
            for (var i = 0; i < 50; i++)
                circularCloudLayouter.PutNextRectangle(new Size(50 + i, 50 - i));
            var cloudImageCreator = new TagCloudImageCreator(circularCloudLayouter);
            cloudImageCreator.Save();
        }
    }
}
