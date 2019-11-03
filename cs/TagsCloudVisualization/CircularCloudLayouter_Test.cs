using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Test
    {
        private CircularCloudLayouter circularCloudLayouter;
        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(0,0));
        }
        
        [Test]
        public void PutNextRectangle_Should_PutRectangleInTheCenter_When_FirstRectangle()
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            rectangle.Location.Should().Be(new Point(-15, -10));
        }

        [Test]
        public void PutNextRectangle_Should_PutSecondRectangleOnTheRightUpperCornerOfFirst_When_AddedSecondRectangle()
        {
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            secondRectangle.Location.Should().Be(new Point(firstRectangle.Right, firstRectangle.Top));
        }

        [Test]
        public void PutNextRectangle_Should_NotIntersect_When_TwoRectanglesAreAdded()
        {
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ShouldPutThirdRectangleOnTheLeftBottomCorner_When_AddedThirdRectangle()
        {
            circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            var thirdRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            thirdRectangle.Location.Should().Be(new Point(secondRectangle.Left, secondRectangle.Bottom));
        }
        
        [Test]
        public void PutNextRectangle_Should_NotIntersectAnyOfRectangles_When_AddedThirdRectangle()
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 3; i++)
            {
                var rect = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
                rectangles.Add(rect);
            }

            var anyIntersect = false;
            foreach (var firstRectangle in rectangles)
            {
                foreach (var secondRectangle in rectangles)
                {
                    if(firstRectangle == secondRectangle)
                        continue;
                    if (firstRectangle.IntersectsWith(secondRectangle))
                        anyIntersect = true;
                }
            }
            anyIntersect.Should().Be(false);
        }
    }
}