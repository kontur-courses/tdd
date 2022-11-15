using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Distributions;

namespace TagsCloudVisualization
{
    public partial class CircularCloudLayouter
    {
        public int RectangleCount => Rectangles.Count;
        public List<Rectangle> Rectangles { get; }

        private readonly Point center;
        private readonly List<Point> distributionPoints;
        private IDistribution distribution;

        public CircularCloudLayouter(Point center, IDistribution distribution)
        {
            this.center = center;
            this.distribution = distribution;
            
            Rectangles = new List<Rectangle>();
            distributionPoints = new List<Point>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(GetNextRectanglePosition(rectangleSize), rectangleSize);
            
            Rectangles.Add(rectangle);

            return rectangle;
        }

        public void GenerateRandomCloud(int amountRectangles)
        {
            var rnd = new Random();
            for (var i = 0; i < amountRectangles; i++)
                PutNextRectangle(new Size(rnd.Next(60, 120), rnd.Next(25, 60)));
        }
        
        public void DrawCircularCloud(int imageWidth, int imageHeight, bool needDrawDistribution = false, 
            string path = null)
        {
            var imageCreator = new ImageCreator(imageWidth, imageHeight);

            if (needDrawDistribution)
                imageCreator.DrawSpiral(distributionPoints);
            imageCreator.DrawRectangles(Rectangles);
            imageCreator.SaveImage(path);
        }
    }
}