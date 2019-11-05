using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterConstructor_should
    {
        [TestCase(0, 10, TestName = "Center x is zero")]
        [TestCase(10, 0, TestName = "Center y is zero")]
        [TestCase(-1, 10, TestName = "Center x is negative")]
        [TestCase(10, -1, TestName = "Center y is negative")]
        public void ThrowArgumentException_When(int x, int y)
        {
            Action action = () => new CircularCloudLayouter(new Point(x, y));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void InitCenterField_When_CenterCoordinatesPositive()
        {
            var center = new Point(600, 800);
            var circularCloudLayouter = new CircularCloudLayouter(center);

            circularCloudLayouter.Center.Should().BeEquivalentTo(center);
        }
        
    }
    
    [TestFixture]
    public class CircularCloudLayouterPutNextRectangle_should
    {
        private CircularCloudLayouter circularCloudLayouter;
        
        [SetUp]
        public void Init()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(500,500));
        }
        
        [TestCase(0, 10, TestName = "Width x is zero")]
        [TestCase(10, 0, TestName = "Height y is zero")]
        [TestCase(-1, 10, TestName = "Width x is negative")]
        [TestCase(10, -1, TestName = "Height y is negative")]
        public void ThrowArgumentException_When(int width, int height)
        {
            Action action = () => circularCloudLayouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void AddRectangle_When_SizeIsPositive()
        {
            var checkList = new List<Rectangle>();
            
            checkList.Add(circularCloudLayouter.PutNextRectangle(new Size(100, 200)));

            checkList.Count.Should().Be(1);
            checkList.First().Should().BeEquivalentTo(new Size(100,200));
        }
        
        [Test]
        public void AddFirstRectangleInTheCloudCenter()
        {
            Rectangle addedRectangle;
            
            addedRectangle = circularCloudLayouter.PutNextRectangle(new Size(100, 200));

            addedRectangle.Location.Should().BeEquivalentTo(circularCloudLayouter.Center);
        }
        
        [Test]
        public void AddNextRectangle_That_DoesntIntersectWithFirst()
        {
            Rectangle firstRectangle;
            Rectangle secondRectangle;

            firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(100, 200));
            secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(50, 100));

            secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
        }
        
        [Test]
        public void AddMultipleRectangles_That_DontIntersectWithEachOther()
        {
            List<Rectangle> checkList = new List<Rectangle>();

            checkList.Add(circularCloudLayouter.PutNextRectangle(new Size(100, 200)));
            checkList.Add(circularCloudLayouter.PutNextRectangle(new Size(130, 250)));
            checkList.Add(circularCloudLayouter.PutNextRectangle(new Size(210, 160)));
            checkList.Add(circularCloudLayouter.PutNextRectangle(new Size(120, 115)));
            
            checkList.Any(r1 => checkList.Any(r2 => r1.IntersectsWith(r2) && r1 != r2)).Should().BeFalse();
        }
    }
}