using System.Drawing;
using System.Drawing.Imaging;

namespace ProjectCircularCloudLayouter
{
    class Program
    {
        static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            GenerateRectangles.MakeLayouter(layouter, 323, 50, 100, 
                20, 45);
            var visualization = new CircularCloudVisualisation(layouter);
            visualization.MakeImageTagsCircularCloud("circularCloud.jpg", ImageFormat.Jpeg);
        }
    }
}