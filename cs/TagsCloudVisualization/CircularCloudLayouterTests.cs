using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            rectangles = new List<Rectangle>();
            layouter = new CircularCloudLayouter(new Point(25, 25));
        }
        
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                Drawer.DrawRectangles(layouter.Center, rectangles);
                
                var fileName = $"{TestContext.CurrentContext.Test.FullName}.png";
                var path = Path.Combine(Path.GetFullPath(@"..\..\..\"), fileName);
                Drawer.SaveImage(path);
                
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [Test]
        public void PutNextRectangle_LocationOfFirstRectangleIsPointEmpty()
        {
            rectangles.Add(
                layouter.PutNextRectangle(new Size(2, 2)));

            rectangles.Should().ContainSingle(rectangle =>
                rectangle.Location == Point.Empty);
        }

        [Test]
        public void PutNextRectangle_AllRectanglesAreNotIntersect()
        {
            PutNextRectangleTest();
        }

        [TestCase(-5, -5, TestName = "Center at (-5, -5)")]
        [TestCase(400, 400, TestName = "Center at (400, 400)")]
        [TestCase(0, 0, TestName = "Center at (0, 0)")]
        public void PutNextRectangle_CorrectWorkWithDifferentCenters(int xCenter, int yCenter)
        {
            layouter = new CircularCloudLayouter(new Point(xCenter, yCenter));
            PutNextRectangleTest();
        }

        private void PutNextRectangleTest(int count = 100)
        {
            var random = new Random();
            
            for (int i = 0; i < count; i++)
            {
                var size = new Size(random.Next(5, 80), random.Next(5, 80));
                rectangles.Add(layouter.PutNextRectangle(size));
            }
            
            AreRectangleItersect(rectangles).Should().Be(false);
        }

        private static bool AreRectangleItersect(List<Rectangle> rects)
        {
            for (int i = 0; i < rects.Count - 1; i++)
                if (rects.Skip(i + 1).Any(rectangle =>
                    rectangle.IntersectsWith(rects[i])))
                {
                    return true;
                }

            return false;
        }
    }
}