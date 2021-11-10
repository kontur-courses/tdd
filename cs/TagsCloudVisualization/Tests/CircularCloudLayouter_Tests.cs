using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter cloudLayouter;

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(new PointF(), new Spiral(0.5f, 0.01f));
        }

        [TestCase(0, 1, TestName = "Width is zero")]
        [TestCase(-1, 1, TestName = "Width is negative")]
        [TestCase(1, 0, TestName = "Height is zero")]
        [TestCase(1, -1, TestName = "Height is negative")]
        public void PutNextRectangle_ShouldThrow_IfIncorrectSize(int width, int height)
        {
            Action act = () => cloudLayouter.PutNextRectangle(new Size(width, height));

            act.Should().Throw<ArgumentException>()
                .WithMessage("Size parameters should be positive");
        }

        [Test]
        public void PutNextRectangle_ShouldPutWithoutIntersecting()
        {
            var puttedTags = new List<RectangleF>();
            for (var i = 0; i < 20; i++)
            {
                puttedTags.Add(cloudLayouter.PutNextRectangle(new Size(50, 15)));
            }

            puttedTags
                .Where(r => CloudIntersectWith(r, puttedTags))
                .Should()
                .BeEmpty();
        }

        private Boolean CloudIntersectWith(RectangleF r, IEnumerable<RectangleF> cloud)
        {
            foreach (var tag in cloud.Where(t => t != r))
                if (tag.IntersectsWith(r))
                    return true;
            return false;
        }


        [TestCase(0, 0, 2, 2, TestName = "Even width and height in zero center")]
        [TestCase(0, 0, 9, 9, TestName = "Odd width and height in zero center")]
        [TestCase(0, 1, 2, 5, TestName = "Different width and height")]
        [TestCase(3, 3, 1, 1, TestName = "Width and height greater than center coordinates")]
        [TestCase(3, 6, 9, 1, TestName = "Different parameters and different center coordinates")]
        [TestCase(-4, -3, 2, 6, TestName = "Negative center coordinates")]
        public void PutNextRectangle_ReturnFirstTagInCenter_IfAddOneTag(int xCloudPosition, int yCloudPosition,
            int width,
            int height)
        {
            cloudLayouter = new CircularCloudLayouter(new Point(xCloudPosition, yCloudPosition), new Spiral(1f, 0.2f));

            var tag = cloudLayouter.PutNextRectangle(new Size(width, height));

            var xCenter = (tag.Left + tag.Right) / 2;
            var yCenter = (tag.Top + tag.Bottom) / 2;
            xCenter.Should().Be(xCloudPosition);
            yCenter.Should().Be(yCloudPosition);
        }


        [TestCase(2, 2, 50)]
        public void PutNextRectangle_ShouldPutEnoughTight(int width, int height, int count)
        {
            var rectangles = new List<RectangleF>();
            for (var i = 0; i < count; i++)
                rectangles.Add(cloudLayouter.PutNextRectangle(new Size(width, height)));

            var density = GetDensity(rectangles);
            Console.WriteLine(density);
        }

        private double GetDensity(IEnumerable<RectangleF> rectangles)
        {
            var rectangleFs = rectangles as RectangleF[] ?? rectangles.ToArray();
            var union = rectangleFs.Aggregate(RectangleF.Union);
            var unionRectsArea = union.Height * union.Width;
            var sumOfAreas = rectangleFs.Sum(rectangle => rectangle.Height * rectangle.Width);
            return sumOfAreas / unionRectsArea;
        }
        
    }
}