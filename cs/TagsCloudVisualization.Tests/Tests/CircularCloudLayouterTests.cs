using System;
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
        
        private CircularCloudLayouter cloudLayouter;
        
        [SetUp]
        public void Init()
        {
            cloudLayouter = new CircularCloudLayouter(new Point(500, 500));
        }
        
        [Test]
        public void PutNextRectangle_SuccessPath_ShouldAddRectangle()
        {
            var rectangleSize = new Size(100, 100);

            cloudLayouter.PutNextRectangle(rectangleSize);
            
            Assert.AreEqual(1, cloudLayouter.RectangleCount);
        }
        
        [Test]
        public void GenerateRandomCloud_SuccessPath_ShouldAddRectangles()
        {
            var amount = 100;
            
            cloudLayouter.GenerateRandomCloud(amount);
            
            Assert.AreEqual(amount, cloudLayouter.RectangleCount);
        }
        
        [Test]
        public void IsRectanglesIntersect_WithIntersectingRectangles_ShouldReturnTrue()
        {
            var firstRectangle = new Rectangle(new Point(100, 100), new Size(100, 100));
            var secondRectangle = new Rectangle(new Point(150, 150), new Size(100, 100));

            var result = CircularCloudLayouter.IsRectanglesIntersect(firstRectangle, secondRectangle);
            
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void IsRectanglesIntersect_WithSameRectanglePosition_ShouldReturnTrue()
        {
            var firstRectangle = new Rectangle(new Point(100, 100), new Size(100, 100));
            var secondRectangle = new Rectangle(new Point(100, 100), new Size(100, 100));

            var result = CircularCloudLayouter.IsRectanglesIntersect(firstRectangle, secondRectangle);
            
            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void IsRectanglesIntersect_WithNotIntersectingRectangles_ShouldReturnFalse()
        {
            var firstRectangle = new Rectangle(new Point(100, 100), new Size(100, 100));
            var secondRectangle = new Rectangle(new Point(300, 150), new Size(100, 100));

            var result = CircularCloudLayouter.IsRectanglesIntersect(firstRectangle, secondRectangle);
            
            Assert.AreEqual(false, result);
        }

        [TearDown]
        public void CleanUp()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure && cloudLayouter.RectangleCount != 0)
            {
                var path = string.Concat(projectDirectory, $"\\Images\\{TestContext.CurrentContext.Test.Name}.png");
                cloudLayouter.DrawCircularCloud(1000, 1000, path);
                Console.Error.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }
    }
}