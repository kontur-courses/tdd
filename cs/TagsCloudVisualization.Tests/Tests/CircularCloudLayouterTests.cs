using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private readonly string projectDirectory 
            = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        private List<Rectangle> rectangles;
        private CircularCloudLayouter cloudLayouter;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(500, 500);
            rectangles = new List<Rectangle>();
            cloudLayouter = new CircularCloudLayouter();
        }
        
        [Test]
        public void GenerateCloud_SuccessPath_ShouldAddRectangle()
        {
            var rectangleSize = new Size(100, 100);

            cloudLayouter.PutNextRectangle(center, rectangles, rectangleSize);
            
            Assert.AreEqual(1, rectangles.Count);
        }
        
        [Test]
        public void PutNextRectangle_WithFirstRectangle_ShouldAddInCenter()
        {
            var rectangleSize = new Size(100, 100);

            cloudLayouter.PutNextRectangle(center, rectangles, rectangleSize);
            
            Assert.AreEqual(center.X - rectangleSize.Width / 2, rectangles[^1].X);
            Assert.AreEqual(center.Y - rectangleSize.Height / 2, rectangles[^1].Y);
        }
        
        [Test]
        public void GenerateCloud_SuccessPath_ShouldReturnRectangles()
        {
            var amount = 100;
            var listSize = TagCloudHelper.GenerateRandomListSize(amount);
            
            rectangles = cloudLayouter.GenerateCloud(center, listSize);
            
            Assert.AreEqual(amount, rectangles.Count);
        }

        [Test]
        public void IsRectanglesIntersect_AllRectanglesNotIntersect_ShouldReturnTrue()
        {
            rectangles = cloudLayouter
                .GenerateCloud(center, TagCloudHelper.GenerateRandomListSize(50));

            for (var i = 1; i < rectangles.Count; i++)
                for (var j = 0; j < i; j++)
                    Assert.AreEqual(true, !rectangles[i].IntersectsWith(rectangles[j]));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                var path = string.Concat(projectDirectory, $"\\FailureImages\\{TestContext.CurrentContext.Test.Name}.png");
                var bitmap = TagCloudHelper.DrawTagCloud(rectangles, 1000, 1000);
                
                bitmap.Save(path);
                
                Console.Error.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }
    }
}