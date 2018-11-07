using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(100, 100));
            layouter.PutNextRectangle(new Size(10, 7));
        }
    }
}