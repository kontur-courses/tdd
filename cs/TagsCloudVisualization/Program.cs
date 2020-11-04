using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.MakeLayouter(5000, 50, 60,
                20, 25);
            layouter.MakeImageTagsCircularCloud("circularCloud.jpg", ImageFormat.Jpeg);
        }
    }
}