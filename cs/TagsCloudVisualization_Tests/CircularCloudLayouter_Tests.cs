using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter layouter;
        private Size minSize;
        private Size maxSize;
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(300, 300));
            minSize = new Size(5, 5);
            maxSize = new Size(20, 20);
        }
        
        [Test]
        public void PutNextRectangle_ShouldReturnRectangleWithSameSize_WhenSomeSizesAdded()
        {
            foreach (var size in SizesGenerator.GenerateSizesList(5, minSize, maxSize))
                layouter.PutNextRectangle(size).Size
                    .Should().BeEquivalentTo(size);
        }
        
        [Test]
        public void Layout_ShouldContainsAllRectangles_WhenSomeSizesAdded()
        {
            var sizes = SizesGenerator.GenerateSizesList(5, minSize, maxSize);
            FillLayoutWithSomeRectangles(layouter, sizes);

            layouter.Rectangles.Select(rect => rect.Size)
                .Should().BeEquivalentTo(sizes);
        }
        
        [Test]
        public void LayoutRectangles_ShouldNotIntersectEachOther_WhenSomeRectanglesAdded()
        {
            var sizes = SizesGenerator.GenerateSizesList(5, minSize, maxSize);
            FillLayoutWithSomeRectangles(layouter, sizes);

            foreach (var rectangle in layouter.Rectangles)
            {
                foreach (var otherRectangle in layouter.Rectangles)
                {
                    if (rectangle != otherRectangle)
                        rectangle.IntersectsWith(otherRectangle)
                            .Should().BeFalse();
                }
            }
        }
        
        [Test]
        public void LayoutShape_ShouldBeCloseToCircle_WhenManySizesAdded()
        {
            var sizes = SizesGenerator.GenerateSizesList(20, minSize, maxSize);
            FillLayoutWithSomeRectangles(layouter, sizes);
            
            var occupiedArea = 0;
            foreach (var rectangle in layouter.Rectangles)
                occupiedArea += rectangle.Width * rectangle.Height;

            var allowedDistance = GetMaxAllowedDistance(occupiedArea, 0.26);

            foreach (var rectangle in layouter.Rectangles)
            {
                GetMaxDistanceToRectangle(layouter.Center, rectangle)
                    .Should().BeLessOrEqualTo(allowedDistance, 
                        $"rectangle must be in a circle with a radius {allowedDistance}");
            }
        }

        private static double GetMaxDistanceToRectangle(Point center, Rectangle rectangle)
        {
            var cornerX = GetNumberWithBiggerDistanceFromGivenNumber(center.X, 
                rectangle.X, rectangle.X + rectangle.Width);
            var cornerY = GetNumberWithBiggerDistanceFromGivenNumber(center.Y, 
                rectangle.Y, rectangle.Y + rectangle.Height);
            return GetDistance(center, new Point(cornerX, cornerY));
        }

        //Очень не нравится имя данного метода, но короче я сейчас придумать не могу
        // TODO: поправить имя!!
        private static int GetNumberWithBiggerDistanceFromGivenNumber(int givenNumber, 
            int firstNumber, int secondNumber)
        {
            return Math.Abs(givenNumber - firstNumber) >= Math.Abs(givenNumber - secondNumber)
                ? firstNumber
                : secondNumber;
        } 

        private static double GetMaxAllowedDistance(int occupiedArea, double occupiedAreaRatio)
            => GetCircleRadiusFromArea(occupiedArea / occupiedAreaRatio);
        
        private static double GetCircleRadiusFromArea(double circleArea) 
            => Math.Sqrt(circleArea / Math.PI);
        
        private static double GetDistance(Point first, Point second)
            => Math.Sqrt(Math.Pow(first.X - second.X, 2)
                         + Math.Pow(first.Y - second.Y, 2));
        
        private static void FillLayoutWithSomeRectangles(CircularCloudLayouter layouter,
            List<Size> rectangleSizes)
        {
            foreach (var size in rectangleSizes)
                layouter.PutNextRectangle(size);
        }
    }
}
