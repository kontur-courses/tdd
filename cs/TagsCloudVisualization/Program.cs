using System;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class Program
    {
        public void Main()
        {
            VisualizationSomeImage("smallPicture.png", 20, 400, 600);
            VisualizationSomeImage("middlePicture.png", 60, 800, 400);
            VisualizationSomeImage("largePicture.png", 300, 1500, 200);
        }

        private static void VisualizationSomeImage(string name, int sizeOfRectangle, int size, int count)
        {
            var cloudLayouter = new CircularCloudLayouter(new Point(size, size));
            var random = new Random();

            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(random.Next(sizeOfRectangle / 3, sizeOfRectangle),
                    random.Next(sizeOfRectangle / 3, sizeOfRectangle)));

            var bitMap = cloudLayouter.Visualization();
            bitMap.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name));

            bitMap = Visualization.VisualizationFil(cloudLayouter.Centreings(), size * 2, size * 2);
            bitMap.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Centering{name}"));
        }
    }
}