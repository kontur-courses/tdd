using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private CircularCloudLayouter layouter;
        private List<Size> sizes;
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(300, 300));
            sizes = new List<Size>() {new Size(10, 5), 
                new Size(20, 10), new Size(10, 5), 
                new Size(15, 15)};

        }
        
        [Test]
        [TestCase(15, 10)]
        public void Layouter_ShouldReturnRectangleWithSameSize_WhenPuttingNextRectangle(int width, int height)
        {
            var size = new Size(width, height);
            layouter.PutNextRectangle(size).Size
                .Should().BeEquivalentTo(size);
        }
        
        [Test]
        public void Layouter_ShouldContainsAllRectangles_WhenSomeRectangleSizesAdded()
        {
            fillLayouterWithSomeRectangles(layouter, sizes);

            layouter.Rectangles.Select(rect => rect.Size)
                .Should().BeEquivalentTo(sizes);
        }
        
        [Test]
        public void LayouterRectangles_ShouldNotIntersectEachOther_WhenSomeRectanglesAdded()
        {
            fillLayouterWithSomeRectangles(layouter, sizes);

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
        public void LayouterActualCenter_ShouldBeCloseToExpectedCenter_WhenManyRectanglesAdded()
        {
            fillLayouterWithSomeRectangles(layouter, sizes);
            fillLayouterWithSomeRectangles(layouter, sizes);
            fillLayouterWithSomeRectangles(layouter, sizes);

            var borderRectangle = layouter.BorderRectangle;
            checkNumberInMiddleBetweenOthers(layouter.Center.X, borderRectangle.X,
                borderRectangle.X + borderRectangle.Width, 0.5);
            checkNumberInMiddleBetweenOthers(layouter.Center.Y, borderRectangle.Y,
                borderRectangle.Y + borderRectangle.Height, 0.5);
        }

        private void checkNumberInMiddleBetweenOthers(double targetNumber, double smallerNumber,
            double biggerNumber, double middleSize)
        {
            var segmentLength = biggerNumber - smallerNumber;
            var middleNumber = biggerNumber + smallerNumber / 2;
            var leftMiddleCorner = middleNumber - middleSize * segmentLength / 2;
            var rightMiddleCorner = middleNumber + middleSize * segmentLength / 2;
            targetNumber.Should().BeInRange(leftMiddleCorner, rightMiddleCorner);
        }

        private void fillLayouterWithSomeRectangles(CircularCloudLayouter layouter,
            List<Size> rectangleSizes)
        {
            foreach (var size in rectangleSizes)
                layouter.PutNextRectangle(size);
        }
    }
}