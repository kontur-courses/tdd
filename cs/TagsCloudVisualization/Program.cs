using System;
using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework.Constraints;
using TagsCloudVisualization.CloudLayouts;
using TagsCloudVisualization.CloudVisualizers;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var cloudLayoutFirst = new CircularCloudLayout(Point.Empty);
            var cloudLayoutSecond = new CircularCloudLayout(new Point(100, 100));
            var cloudLayoutThird = new CircularCloudLayout(new Point(-100, -100));
            GenerateRectangles(cloudLayoutFirst);
            GenerateRectangles(cloudLayoutSecond);
            GenerateRectangles(cloudLayoutThird);
            
            var imageSize = new Size(800, 600);
            
            var cloudVisualizer = new CloudVisualizer(Pens.Aqua, cloudLayoutFirst);
            var image = cloudVisualizer.GenerateImage(imageSize);
            image.Save(@"first.png", ImageFormat.Png);
            
            cloudVisualizer.Cloud = cloudLayoutSecond;
            image = cloudVisualizer.GenerateImage(imageSize);
            image.Save(@"second.png", ImageFormat.Png);
            
            cloudVisualizer.Cloud = cloudLayoutThird;
            image = cloudVisualizer.GenerateImage(imageSize);
            image.Save(@"third.png", ImageFormat.Png);
        }

        private static void GenerateRectangles(ICloudLayout cloud)
        {
            var rand = new Random();
            var rectCount = rand.Next(10, 100);
            for (int i = 0; i < rectCount; i++)
            {
                 cloud.PutNextRectangle(new Size(rand.Next(50, 100), rand.Next(10, 40)));
            }
        }
    }
}