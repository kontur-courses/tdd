using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization
{
    [TestFixture]
    public class TagsCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layouter;
        private Size rectangleSize;


        [SetUp]
        public void SetUp()
        {
            center = new Point(7, 11);
            layouter = new CircularCloudLayouter(center);
            rectangleSize = new Size(6, 3);
        }
        
        [Test]
        public void GetTagsCloud_ShouldNotNull()
        {
            var actual = layouter.Cloud;
            actual.Should().NotBeNull();
        }

        [Test]
        public void GetTagsCloudCenter_ShouldReturnRightPoint()
        {
            var actual = layouter.Cloud.Center;
            actual.Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangle()
        {
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            rectangle.Should().NotBeNull();
        }

        [Test]
        public void PutNextRectangle_WithValidSize_ShouldReturnRectangleWithThisSize()
        {
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            rectangle.Size.Should().BeEquivalentTo(rectangleSize);
        }

        [Test]
        public void PutNextRectangle_CloudShouldContainsThisRectangle()
        {
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            var actual = layouter.Cloud.Rectangles.Contains(rectangle);
            actual.Should().BeTrue();
        }

        [Test]
        public void PutNextRectangles_AfterPutNRectangles_ShouldNotIntersection()
        {
            var count = new Random().Next(50, 100);
            var rectangles = layouter.Cloud.Rectangles;

            PutNextRectangleCountTime(count, rectangleSize);

            rectangles.All(x => !rectangles.Any(y => y != x && y.IntersectsWith(x))).Should().BeTrue();
        }

        [Test]
        [Timeout(200)]
        public void PutNextRectangle_Put1000BigRectangles_ShouldNotThrowException()
        {
            var size = new Size(100, 50);
            PutNextRectangleCountTime(1000, size);
        }

        [Test]
        public void PutNextRectangle_TwoRectanglesWithEqualsSize_ShouldBeNotEquals()
        {
            var first = layouter.PutNextRectangle(rectangleSize);
            var second = layouter.PutNextRectangle(rectangleSize);

            first.Should().NotBeEquivalentTo(second);
        }

        [Test]
        public void PutNextRectangle_AfterPutNRectangles_CloudShouldContainsNRectangles()
        {
            var n = new Random().Next(50, 100);
            PutNextRectangleCountTime(n, rectangleSize);
           layouter.Cloud.Rectangles.Count.Should().Be(n);
        }

        [Test]
        public void PutNextRectangle_DistanceOfAdjacentRectanglesShouldNotExceedN()
        {
            var n = 8;
            var rectangles = layouter.Cloud.Rectangles;
            
            PutNextRectangleCountTime(15, rectangleSize);

            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                (rectangles[i].Location.X - rectangles[i + 1].Location.X).Should().BeLessOrEqualTo(n);
            }
        }

        [Test]
        public void PutRectangle_AfterPutNRectangles_TheyShouldBeTightlySpaced()
        {
            for (var i = 0; i < 14; i++)
            {
                var rectangle = layouter.PutNextRectangle(rectangleSize);
                Console.WriteLine(rectangle.Location);
            }

            var rectangles = layouter.Cloud.Rectangles;

//            rectangles.All(x => x.)
        }

        private void PutNextRectangleCountTime(int count, Size rectanglesSize)
        {
            for (var i = 0; i < count; i++)
            {
                var r = layouter.PutNextRectangle(rectanglesSize);
                Console.WriteLine(r.Location);
            }
        }
    }
}