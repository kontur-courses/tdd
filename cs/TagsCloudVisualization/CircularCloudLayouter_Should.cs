using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private readonly Point center = new Point(100, 50);

        [SetUp]
        public void CreateCircularCloudLayouter()
        {
            layouter = new CircularCloudLayouter(center);
        }


        [Test]
        public void Constructor_ThrowArgumentException_OnNegativeCoordinate()
        {
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(-50, 50)));
        }

        
        [Test]
        public void PutNextRectangle_ShouldPlace_FirstRectangleToCenter()
        {
            var newRectangle = layouter.PutNextRectangle(new Size(20, 10));
            var location = newRectangle.Location;
            var expectedLocation = new Point(90, 45);
            location.Should().Be(expectedLocation);
        }

        
        [Test]
        public void PutNextRectangle_ShouldPlaceTwoRectangles_WithoutInsersection()
        {
            var newRectangle = layouter.PutNextRectangle(new Size(20, 10));
            var newRectangle2 = layouter.PutNextRectangle(new Size(10, 5));            
            newRectangle.IntersectsWith(newRectangle2).Should().BeFalse();
        }
        
        [Test]
        public void PutNextRectangle_ShouldPlaceTwoRectangles_Near()
        {
            var newRectangle = layouter.PutNextRectangle(new Size(20, 10));
            var newRectangle2 = layouter.PutNextRectangle(new Size(10, 5));
            newRectangle.Bottom.Should().Be(newRectangle2.Y);            
        }
        
        [Test]
        public void PutNextRectangle_ShouldPlaceFourRectangles_NearCenterRectangle()
        {
            var newRectangle = layouter.PutNextRectangle(new Size(20, 10));
            var newRectangle2 = layouter.PutNextRectangle(new Size(10, 5));
            var newRectangle3 = layouter.PutNextRectangle(new Size(10, 5));
            var newRectangle4 = layouter.PutNextRectangle(new Size(10, 5));
            var newRectangle5 = layouter.PutNextRectangle(new Size(10, 5));
            newRectangle2.Y.Should().BeLessOrEqualTo(newRectangle.Bottom + 6);
            newRectangle3.Y.Should().BeLessOrEqualTo(newRectangle.Bottom + 6);
            newRectangle4.Y.Should().BeLessOrEqualTo(newRectangle.Bottom + 6);
            newRectangle5.Y.Should().BeLessOrEqualTo(newRectangle.Bottom + 6);            
        }
        
        private List<Size> GetRandomSizes()
        {            
            var sizes = new List<Size>();
            Random rnd = new Random();
            for (var i = 0; i < 30; i++)
            {
                var value1 = rnd.Next(5, 70);
                var value2 = rnd.Next(5, 70);                
                sizes.Add(new Size(value1, value2));
            }
            return sizes;
        }
        
        
        [Test]        
        public void PutNextRectangle_RectanglesShouldNotIntersect()
        {
            var sizes = GetRandomSizes();

            foreach (var size in sizes)
                layouter.PutNextRectangle(size);

            var rectangles = layouter.rectanglesList;
            for (var i = 0; i < rectangles.Count; i++)
            {
                for (var j = 0; j < rectangles.Count; j++)
                {
                    if (i == j)
                        continue;            
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }       
    }
}
