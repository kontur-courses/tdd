using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Distributions;

namespace TagsCloudVisualization.Tests.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private readonly string projectDirectory 
            = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        
        private CircularCloudLayouter cloudLayouter;
        private Point center;
        private IDistribution distribution;
        
        [SetUp]
        public void Init()
        {
            center = new Point(500, 500);
            distribution = new Spiral(center);
            cloudLayouter = new CircularCloudLayouter(center, distribution);
        }
        
        [Test]
        public void PutNextRectangle_SuccessPath_ShouldAddRectangle()
        {
            var rectangleSize = new Size(100, 100);

            cloudLayouter.PutNextRectangle(rectangleSize);
            
            Assert.AreEqual(1, cloudLayouter.RectangleCount);
        }
        
        [Test]
        public void PutNextRectangle_WithFirstRectangle_ShouldAddInCenter()
        {
            var rectangleSize = new Size(100, 100);

            cloudLayouter.PutNextRectangle(rectangleSize);
            
            Assert.AreEqual(center.X - rectangleSize.Width / 2, cloudLayouter.Rectangles[^1].X);
            Assert.AreEqual(center.Y - rectangleSize.Height / 2, cloudLayouter.Rectangles[^1].Y);
        }
        
        [Test]
        public void GenerateRandomCloud_SuccessPath_ShouldReturnAmountRectanglesAdded()
        {
            var amount = 100;
            
            cloudLayouter.GenerateRandomCloud(amount);
            
            Assert.AreEqual(amount, cloudLayouter.RectangleCount);
        }

        [Test]
        public void IsRectanglesIntersect_WithIntersectingRectangles_ShouldReturnTrue()
        {
            var size = new Size(100, 100);
            var firstRectangle = new Rectangle(new Point(100, 100), size);
            var secondRectangle = new Rectangle(new Point(150, 150), size);

            var result = RectangleAddons.IsRectanglesIntersect(firstRectangle, secondRectangle);
            
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void IsRectanglesIntersect_WithSameRectanglePosition_ShouldReturnTrue()
        {
            var rectangle = new Rectangle(new Point(100, 100), new Size(100, 100));

            var result = RectangleAddons.IsRectanglesIntersect(rectangle, rectangle);
            
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void IsRectanglesIntersect_WithNotIntersectingRectangles_ShouldReturnFalse()
        {
            var size = new Size(100, 100);
            var firstRectangle = new Rectangle(new Point(100, 100), size);
            var secondRectangle = new Rectangle(new Point(300, 150), size);

            var result = RectangleAddons.IsRectanglesIntersect(firstRectangle, secondRectangle);
            
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsRectanglesIntersect_AllRectanglesNotIntersect_ShouldReturnTrue()
        {
            cloudLayouter.GenerateRandomCloud(50);

            for (var i = 0; i < cloudLayouter.RectangleCount; i++)
                for (var j = 0; j < i; j++)
                {
                    Assert.AreEqual(true, !RectangleAddons
                        .IsRectanglesIntersect(cloudLayouter.Rectangles[i], cloudLayouter.Rectangles[j]));
                }
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure && cloudLayouter.RectangleCount != 0)
            {
                var path = string.Concat(projectDirectory, $"\\FailureImages\\{TestContext.CurrentContext.Test.Name}.png");
                cloudLayouter.DrawCircularCloud(1000, 1000, false, path);
                Console.Error.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }
    }
}