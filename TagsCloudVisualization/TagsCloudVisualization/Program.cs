using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var bitmap1 = GetCloudPictureWith100RandomRectangles();
            bitmap1.Save("cloud1.jpg", ImageFormat.Jpeg);
            var bitmap2 = GetCloudPictureWith10HorizontalRectangles();
            bitmap2.Save("cloud2.jpg", ImageFormat.Jpeg);
            var bitmap3 = GetCloudPictureWith100RandomDecreasingRectangles();
            bitmap3.Save("cloud3.jpg", ImageFormat.Jpeg);
        }

        private static Bitmap GetCloudPictureWith100RandomRectangles()
        {
            var random = new Random();
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            for (var i=0; i<100; i++)
            {
                var width = random.Next(100, 200);
                var height = random.Next(100, 200);
                cloud.PutNextRectangle(new Size(width, height));
            }

            return TagsCloudVisualizer.GetCloudVisualization(cloud);
        }

        private static Bitmap GetCloudPictureWith100RandomDecreasingRectangles()
        {
            var random = new Random();
            var cloud = new CircularCloudLayouter(new Point(-100, 200));
            for (var i = 0; i < 100; i++)
            {
                var width = random.Next(300, 400);
                var height = random.Next(300, 400);
                cloud.PutNextRectangle(new Size(width-3*i, height-3*i));
            }

            return TagsCloudVisualizer.GetCloudVisualization(cloud);
        }

        private static Bitmap GetCloudPictureWith10HorizontalRectangles()
        {
            var random = new Random();
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            for (var i = 0; i < 100; i++)
            {
                var width = random.Next(100, 600);
                var height = random.Next(50, 100);
                cloud.PutNextRectangle(new Size(width, height));
            }

            return TagsCloudVisualizer.GetCloudVisualization(cloud);
        }
    }
}
