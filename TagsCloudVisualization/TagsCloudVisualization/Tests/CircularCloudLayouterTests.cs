using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
     public class CircularCloudLayouterTests
     {
         private List<Rectangle> rectangles;
        [SetUp]
        public void SetUp()
        {
            rectangles = new List<Rectangle>();
        }
        
        [TestCaseSource(nameof(cloudCenters))] 
        public void Constructor_DoesNotThrow_WithСorrectСenter(Point center) 
        { 
            Action action = () => new CircularCloudLayouter(center); 
            action.Should().NotThrow(); 
        }
        
        [TestCaseSource(nameof(cloudCenters))]
        public void PutNextRectangle_LocateFirstRectangle_OnSpecifiedByXCenter(Point center)
        {
            var circularCloudLayouter = new CircularCloudLayouter(center); 
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(31 , 42));
            rectangle.X.Should().Be(center.X - 31 / 2);
        }
        
        [TestCaseSource(nameof(cloudCenters))]
        public void PutNextRectangle_LocateFirstRectangle_OnSpecifiedByYCenter(Point center)
        {
            var circularCloudLayouter = new CircularCloudLayouter(center); 
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(31 , 42));
            rectangle.Y.Should().Be(center.Y - 42 / 2);
        }
        
        [TestCase(2, TestName = "TwoRectangles")]
        [TestCase(10, TestName = "TenRectangles")]
        [TestCase(20, TestName = "TwentyRectangles")]
        [TestCase(100, TestName = "HundredRectangles")]
        public void PutNextRectangle_RectanglesMustNotIntersect(int countRectangles)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            rectangles.AddRange(Enumerable.Range(10, countRectangles)
                .Select(i => circularCloudLayouter.PutNextRectangle(new Size(i * 3, i))));
            
            CircularCloudLayouter.HasOverlappingRectangles(rectangles).Should().BeFalse();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure) return;
            var path = Path.Combine(Environment.CurrentDirectory, "TagCloudTests");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var circularCloudDrawing = new CircularCloudDrawing(new Size(2000, 2000));
            foreach (var rectangle in rectangles) 
                circularCloudDrawing.DrawRectangle(rectangle);
            var testName = TestContext.CurrentContext.Test.FullName;
            path =  Path.Combine(path, $"{testName}.png");
            circularCloudDrawing.SaveImage(path);
        }

        private static IEnumerable<TestCaseData> cloudCenters = Enumerable 
        .Range(-1, 3) 
        .SelectMany(i => Enumerable 
            .Range(-1, 3) 
            .Select(j => new TestCaseData(new Point(i, j)).SetName("{m}: " + $"X = {i}, Y = {j}"))); 
    }
}