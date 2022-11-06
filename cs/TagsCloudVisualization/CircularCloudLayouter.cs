using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public partial class CircularCloudLayouter
    {
        public int RectangleCount => rectangles.Count;
        public List<Rectangle> Rectangles => rectangles;

        private double angle = 0;
        private readonly Point center;
        private readonly List<Point> spiralPoints;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            spiralPoints = new List<Point>();
        }

        public void PutNextRectangle(Size rectangleSize)
        {
            rectangles.Add(new Rectangle(GetNextRectanglePosition(rectangleSize), rectangleSize));
        }

        public void GenerateRandomCloud(int amountRectangles)
        {
            var rnd = new Random();
            for (var i = 0; i < amountRectangles; i++)
                PutNextRectangle(new Size(rnd.Next(60, 120), rnd.Next(25, 60)));
        }
        
        public void DrawCircularCloud(int imageWidth, int imageHeight, bool needDrawSpiral = false ,string path = null)
        {
            var imageCreator = new ImageCreator(imageWidth, imageHeight);

            if (needDrawSpiral)
                imageCreator.DrawSpiral(spiralPoints);
            imageCreator.DrawRectangles(rectangles);
            imageCreator.SaveImage(path);
        }
    }
}