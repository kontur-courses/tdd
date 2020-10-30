using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using System.Linq;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_shuld
    {
        private CircularCloudLayouter cloud;
        private Point center;

        [SetUp]
        public void Setup()
        {
            center = new Point(0, 0);
            cloud = new CircularCloudLayouter(center);
        }

        [Test]
        public void PutNextRectangle_OnCenter_AfterFirstPut()
        {
            var rectangleSize = new Size(100, 50);
            var expectedRectangle = new Rectangle(center - new Size(50, 25), rectangleSize);
            cloud.PutNextRectangle(rectangleSize).Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_NoIntersects_AfterPutting()
        {
            var rectangleSize = new Size(10, 50);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
            {
                rectangles.Add(cloud.PutNextRectangle(rectangleSize));
            }

            foreach (var rectangle in rectangles)
            {
                foreach (var secondRectangle in rectangles.Where(x => x != rectangle))
                {
                    rectangle.IntersectsWith(secondRectangle).Should().BeFalse();
                }
            }
        }
    }
}