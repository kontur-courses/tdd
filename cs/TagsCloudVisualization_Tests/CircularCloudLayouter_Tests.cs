using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private IRectangleLayouter layouter;
        private Size minSize;
        private Size maxSize;
        
        [SetUp]
        public void SetUp()
        {
            // Закомментируйте одну из строк ниже
            layouter = new CircularCloudLayouter(new Point(1000, 1000));
            // layouter = new BadLayouter(new Point(1000, 1000));
            minSize = new Size(5, 5);
            maxSize = new Size(20, 20);
        }

        [TearDown]
        public void DrawLayoutAfterTestFailed()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var picture = new Picture(new Size(2000, 2000));
            foreach (var rectangle in layouter.Rectangles)
            {
                
                picture.FillRectangle(rectangle, Color.Lime);
            }
            picture.Save(outputFileName:"failed_layout");
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleWithSameSize_WhenSomeSizesAdded()
        {
            foreach (var size in SizesGenerator.GenerateSizes(5, minSize, maxSize, seed:128))
                layouter.PutNextRectangle(size).Size
                    .Should().BeEquivalentTo(size);
        }
        
        [Test]
        public void Layout_ShouldContainsAllRectangles_WhenSomeSizesAdded()
        {
            var sizes = SizesGenerator.GenerateSizes(5, minSize, maxSize, seed:128);
            FillLayoutWithSomeRectangles(layouter, sizes);

            layouter.Rectangles.Select(rect => rect.Size)
                .Should().BeEquivalentTo(sizes);
        }
        
        [Test]
        public void LayoutRectangles_ShouldNotIntersectEachOther_WhenSomeRectanglesAdded()
        {
            var sizes = SizesGenerator.GenerateSizes(25, minSize, maxSize, seed:128);
            FillLayoutWithSomeRectangles(layouter, sizes);

            foreach (var rectangle in layouter.Rectangles)
            {
                foreach (var otherRectangle in layouter.Rectangles.Where(otherRectangle => rectangle != otherRectangle))
                {
                    rectangle.IntersectsWith(otherRectangle)
                        .Should().BeFalse();
                }
            }
        }
        
        [Test]
        public void LayoutShape_ShouldBeCloseToCircle_WhenManySizesAdded()
        {
            var sizes = SizesGenerator.GenerateSizes(600, minSize, maxSize, seed:128);
            FillLayoutWithSomeRectangles(layouter, sizes);
            
            var occupiedArea = layouter.Rectangles
                .Sum(rectangle => rectangle.Width * rectangle.Height);

            var allowedDistance = GetMaxAllowedDistance(occupiedArea, 0.65);

            foreach (var rectangle in layouter.Rectangles)
            {
                GetMaxDistanceToRectangle(layouter.Center, rectangle)
                    .Should().BeLessOrEqualTo(allowedDistance, 
                        $"rectangle must be in a circle with a radius {allowedDistance}");
            }
        }

        private static double GetMaxDistanceToRectangle(Point center, Rectangle rectangle)
        {
            var cornerX = GetNumberWithBiggerDistanceFromGiven(center.X, 
                rectangle.X, rectangle.X + rectangle.Width);
            var cornerY = GetNumberWithBiggerDistanceFromGiven(center.Y, 
                rectangle.Y, rectangle.Y + rectangle.Height);
            return GetDistance(center, new Point(cornerX, cornerY));
        }
        
        private static int GetNumberWithBiggerDistanceFromGiven(int givenNumber, 
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
        
        private static void FillLayoutWithSomeRectangles(IRectangleLayouter layouter,
            IEnumerable<Size> rectangleSizes)
        {
            foreach (var size in rectangleSizes)
                layouter.PutNextRectangle(size);
        }
    }
}
