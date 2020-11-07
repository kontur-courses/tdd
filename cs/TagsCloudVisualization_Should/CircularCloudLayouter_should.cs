using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Should
{
    public class CircularCloudLayouterShould
    {
        [Test]
        public void PutNextRectangle_ThrowArgumentException_SizeOfRectangleHaveNegativeValue()
        {
            var center =  new Point(100,100);
            var pointProvider = new PointProvider(center);
            var cloud = new CircularCloudLayouter(center,pointProvider);

            Action act = () => cloud.PutNextRectangle(new Size(-100, -100));

            act.ShouldThrow<ArgumentException>().WithMessage("Width or height of rectangle was negative");
        }

        [Test]
        public void PutNextRectangle_ReturnSameRectangle_OneRectangle()
        {
            var center = new Point(40,40);
            var pointProvider = new PointProvider(center);
            var expectedRectangle = new Rectangle(new Point(40, 40), new Size(30, 30));
            var cloud = new CircularCloudLayouter(center, pointProvider);

            var actual = cloud.PutNextRectangle(new Size(30, 30));

            actual.ShouldBeEquivalentTo(expectedRectangle);

        }

        [Test]
        public void Rectangles_CountIsTen_RandomTenRectangles()
        {
            var rnd = new Random();
            var center = new Point(500,500);
            var pointProvider = new PointProvider(center);
            var cloud = new CircularCloudLayouter(center,pointProvider);
            const int expectedLength = 10;

            for (var i = 0; i < 10; i++)
            {
                var size = new Size(rnd.Next(10, 200), rnd.Next(10, 200));
                cloud.PutNextRectangle(size);
            }
            var actualLength = cloud.GetListRectangles().Count;

            actualLength.Should().Be(expectedLength);
        }

        [Test]
        public void Rectangles_SameOrderLikeAdded_ThreeRectangles()
        {
            var center = new Point(500,500);
            var pointProvider = new PointProvider(center);
            var cloud = new CircularCloudLayouter(center,pointProvider);
            var expectedRectangles = new List<Rectangle>
            {
                new Rectangle(new Point(500, 500), new Size(30, 30)),
                new Rectangle(new Point(530, 493), new Size(40, 40)),
                new Rectangle(new Point(510, 530), new Size(20, 20))
            };

            cloud.PutNextRectangle(new Size(30, 30));
            cloud.PutNextRectangle(new Size(40, 40));
            cloud.PutNextRectangle(new Size(20, 20));
            var actualRectangles = cloud.GetListRectangles();

            actualRectangles.ShouldAllBeEquivalentTo(expectedRectangles);

        }
    }
}
