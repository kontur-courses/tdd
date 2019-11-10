using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TagsCloudVisualization;

namespace ExampleTagsClouds
{
    class Program
    {
        static void Main(string[] args)
        {
            EqualSizedSquaresVisualization();
            RandomSizedRectangles();
            FixedHeightRandomXFactorWidthRectangles();
            ManyRandomSizedRectangles();
        }

        private static void EqualSizedSquaresVisualization(string name = "equal-sized-squares.png", int width = 20)
        {
            var tagsCloudImage = new TagsCloudImage(1920, 1080);

            var circularCloudLayouter = new CircularCloudLayouter(new Point(100, 250));
            var rectangles = new List<Rectangle>();
            var rectangleSize = new Size(width, width);

            for (var i = 0; i < 500; ++i)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(rectangleSize));
            }
            
            tagsCloudImage.AddRectangles(rectangles);

            var exactPath = Path.GetFullPath(name);
            tagsCloudImage.GetBitmap().Save(exactPath);
            Console.WriteLine("Saved to {0}", exactPath);
        }
        
        private static void RandomSizedRectangles(string name = "random-sized-rectangles.png")
        {
            var tagsCloudImage = new TagsCloudImage(1920, 1080);

            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            var random = new Random();

            for (var i = 0; i < 500; ++i)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(
                    new Size(random.Next(5, 50), random.Next(5, 50))));
            }
            
            tagsCloudImage.AddRectangles(rectangles);

            var exactPath = Path.GetFullPath(name);
            tagsCloudImage.GetBitmap().Save(exactPath);
            Console.WriteLine("Saved to {0}", exactPath);
        }
        
        private static void ManyRandomSizedRectangles(string name = "many-random-sized-rectangles.png")
        {
            var tagsCloudImage = new TagsCloudImage(15000, 15000);

            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            var random = new Random();

            for (var i = 0; i < 50000; ++i)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(
                    new Size(random.Next(5, 50), random.Next(5, 50))));
            }
            
            tagsCloudImage.AddRectangles(rectangles);

            var exactPath = Path.GetFullPath(name);
            tagsCloudImage.GetBitmap().Save(exactPath);
            Console.WriteLine("Saved to {0}", exactPath);
        }
        
        private static void FixedHeightRandomXFactorWidthRectangles(string name = "fixed-height-rectangles.png")
        {
            var tagsCloudImage = new TagsCloudImage(1920, 1080);

            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            var random = new Random();

            for (var i = 0; i < 500; ++i)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(
                    new Size(random.Next(3, 10) * 7, 7)));
            }
            
            tagsCloudImage.AddRectangles(rectangles);

            var exactPath = Path.GetFullPath(name);
            tagsCloudImage.GetBitmap().Save(exactPath);
            Console.WriteLine("Saved to {0}", exactPath);
        }
    }
}