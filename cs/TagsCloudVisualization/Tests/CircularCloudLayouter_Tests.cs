using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        public CircularCloudLayouter cloud;
        [SetUp]
        public void SetUp()
        {
            cloud = new CircularCloudLayouter(new Point(50, 50));
        }
        
        [Test]
        public void CircularCloudLayouterCtor_ValidParameter_ShouldNotThrowException()
        {
            Action act = () => new CircularCloudLayouter(new Point(0, 0));
            act.Should().NotThrow();
        }

        [Test]
        public void PutNextRectangle_ValidSize_ShouldNotThrowException()
        {
            Action act = () => cloud.PutNextRectangle(new Size(50, 50));
            act.Should().NotThrow();
        }

        [Test]
        public void PutNextRectangle_FirstRectangle_ShouldBeInCenter()
        {
            var rectangle = cloud.PutNextRectangle(new Size(100, 50));
            rectangle.Location.Should().Be(new Point(0, 25));
        }

        [Test]
        public void CircularCloudLayouter_RecatanglesBeforePutting_ShouldBeEmpty()
        {
            cloud.Recatangles.Should().BeEmpty();
        }

        [Test]
        public void CircularCloudLayouter_RectanglesAfterPutting_ShouldNotBeEmpty()
        {
            cloud.PutNextRectangle(new Size(100, 50));
            cloud.Recatangles.Should().NotBeEmpty();
        }

        [Test]
        public void CircularCloudLayouter_RectanglesAfterSomePutting_ShouldContainsSomeElements()
        {
            cloud.PutNextRectangle(new Size(100, 50));
            cloud.PutNextRectangle(new Size(10, 5));
            cloud.PutNextRectangle(new Size(120, 30));
            cloud.PutNextRectangle(new Size(170, 5));
            cloud.PutNextRectangle(new Size(10, 40));
            cloud.Recatangles.Count.Should().Be(5);
        }
    }
}