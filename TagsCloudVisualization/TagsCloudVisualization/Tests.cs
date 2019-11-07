using NUnit.Framework;
using System;
using FluentAssertions;
using GeometryObjects;

namespace test
{
    [TestFixture]
    class CircularCloudLayouterShould
    {
        [Test]
        public void Create_IfCorrectCentrePoint()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(2, 3));
            Assert.IsTrue(circularCloudLayouter is CircularCloudLayouter);
        }

        [Test]
        public void ThrowArgumentExceptionInCreating_IfIncorrectCentrePoint()
        {
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(-1, 3)));
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(1, -3)));
        }

        [Test]
        public void PutAndReturnOneRectangle_IfAllPlaneIsFree()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            rectangle.Size.Should().BeEquivalentTo(new Size(10, 10));
        }

        [Test]
        public void PutOneRectangleInCenter_IfAllPlaneIsFree()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(7, 10));
            rectangle.LeftBottomVertex.Should().BeEquivalentTo(new Point(47, 45));
        }

        [Test]
        public void ShiftFirstRectangleToRight_IfDoesNotFitInCenter()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(20, 50));
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(46, 10));
            rectangle.LeftBottomVertex.Should().BeEquivalentTo(new Point(0, 45));
        }

        [Test]
        public void ShiftFirstRectangleToUp_IfDoesNotFitInCenter()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 20));
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(10, 46));
            rectangle.LeftBottomVertex.Should().BeEquivalentTo(new Point(45, 0));
        }

        [Test]
        public void PutTwoRectangles_OnFreePlane()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));
            var rectangle1 = circularCloudLayouter.PutNextRectangle(new Size(10, 20));
            var rectangle2 = circularCloudLayouter.PutNextRectangle(new Size(13, 6));
            rectangle1.Should().NotBe(null);
            rectangle2.Should().NotBe(null);
        }

        [Test]
        public void PutTwoRectangles_AndTheyAreNotIntersected()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));
            var rectangle1 = circularCloudLayouter.PutNextRectangle(new Size(10, 20));
            var rectangle2 = circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            Assert.False(Rectangle.AreRectanglesIntersected(rectangle1, rectangle2));
        }

        [Test]
        public void PutThreeRectangles_AndTheyAreNotIntersected()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));
            var rectangle1 = circularCloudLayouter.PutNextRectangle(new Size(10, 20));
            var rectangle2 = circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            var rectangle3 = circularCloudLayouter.PutNextRectangle(new Size(20, 7));
            Assert.False(Rectangle.AreRectanglesIntersected(rectangle1, rectangle2));
            Assert.False(Rectangle.AreRectanglesIntersected(rectangle1, rectangle3));
            Assert.False(Rectangle.AreRectanglesIntersected(rectangle2, rectangle3));
        }

        [Test]
        public void PutManyRectangles_AndTheyAreNotIntersected()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(100, 100));
            var rectanglesCount = 3;
            var randomRange = 200;
            var random = new Random(randomRange);
            var sizes = new Size[rectanglesCount];
            for (var i = 0; i < rectanglesCount; i++)
                sizes[i] = new Size(1 + random.Next(randomRange), 1 + random.Next(randomRange));
            var rectangles = new Rectangle[rectanglesCount];
            for (var i = 0; i < rectanglesCount; i++)
                rectangles[i] =  circularCloudLayouter.PutNextRectangle(sizes[i]);
            Assert.AreEqual(rectanglesCount, circularCloudLayouter.RectangleCount);
            Assert.IsTrue(!IsIntersectedAnyPairOfRectangles(rectangles));
        }

        private bool IsIntersectedAnyPairOfRectangles(Rectangle[] rectanglesList)
        {
            var n = rectanglesList.Length;
            for (var i = 0; i < n - 1; i++)
            {
                for (var j = i + 1; j < n; j++)
                {
                    if (Rectangle.AreRectanglesIntersected(rectanglesList[i], rectanglesList[j]))
                        return true;
                }
            }
            return false;
        }
    }
}