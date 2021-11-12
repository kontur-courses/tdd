using NUnit.Framework;
using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TagsCloudTest
{
    public class CircularCloudTest
    {
        private ICloudLayouter cloud;

        [Test]
        public void PutNextRectangle_RectangleWithCenterInCloudCenter_WhenPutFirstRectangle()
        {
            var center = new Point(5, 5);
            var cloud = new CircularCloudLayouter(center);
            this.cloud = cloud;

            var rectangle = cloud.PutNextRectangle(new Size(3,1));

            rectangle.Location.Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_ArgumentException_WhenPutRectangleWithNegativeSize()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            Action action = () => cloud.PutNextRectangle(new Size(-2, 0));

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(1000)]
        [TestCase(100)]
        [TestCase(10)]
        [TestCase(2)]
        public void PutNextRectangle_RectanglesDoNotIntersect_WhenPutRectanglesOfTheSameSize(int rectanglesCount)
        {
            var center = new Point(3, 8);
            var cloud = new CircularCloudLayouter(center);
            var rectanglesSize = new Size(3, 2);
            var rectangles = new List<Rectangle>();
            this.cloud = cloud;

            for (var i = 0; i < rectanglesCount; i++)
                rectangles.Add(cloud.PutNextRectangle(rectanglesSize));

            foreach (var r1 in rectangles)
                foreach(var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;
                    r1.IntersectsWith(r2).Should().BeFalse();
                }
        }

        [TestCase(1000)]
        [TestCase(100)]
        [TestCase(20)]
        [TestCase(2)]
        public void PutNextRectangle_RectanglesDoNotIntersect_WhenPutRectanglesOfRandomSize(int rectanglesCount)
        {
            var rnd = new Random();
            var center = new Point(3, 8);
            var cloud = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            this.cloud = cloud;

            for (var i = 0; i < rectanglesCount; i++)
            {
                var width = rnd.Next(1, 1000);
                var height = rnd.Next(1, 1000);
                rectangles.Add(cloud.PutNextRectangle(new Size(width,height)));
            }

            foreach (var r1 in rectangles)
                foreach (var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;
                    r1.IntersectsWith(r2).Should().BeFalse();
                }
        }

        [TestCase(2)]
        [TestCase(200)]
        [TestCase(2000)]
        public void PutNextRectangle_RectanglesAreCloseToCenter_WhenPutRectanglesOfTheSameSize(int rectanglesCount)
        {
            var center = new Point(1, 3);
            var cloud = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(2, 2);
            var rectangles = new List<Rectangle>();
            this.cloud = cloud;

            for (var i = 0; i < rectanglesCount; i++)
                rectangles.Add(cloud.PutNextRectangle(sizeRectangle));


            foreach (var r1 in rectangles) 
            {
                if (r1.Location == center)
                    continue;
                var newR1 = ShiftRectangleToGoalByDelta(r1, center);
                var hasIntersect = false;
                foreach (var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;

                    if(newR1.IntersectsWith(r2))
                    {
                        hasIntersect = true;
                        break;
                    }    
                }
                hasIntersect.Should().BeTrue();
            }
        }

        [TestCase(100)]
        [TestCase(2)]
        [TestCase(10)]
        public void PutNextRectangle_RectanglesAreCloseToCenter_WhenPutRectanglesOfRandomSize(int rectanglesCount)
        {
            var rnd = new Random();
            var center = new Point(1, 3);
            var cloud = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            this.cloud = cloud;

            for (var i = 0; i < rectanglesCount; i++)
            {
                var width = rnd.Next(1, 1000);
                var height = rnd.Next(1, 1000);
                rectangles.Add(cloud.PutNextRectangle(new Size(width, height)));
            }

            foreach (var r1 in rectangles)
            {
                if (r1.Location == center)
                    continue;
                var newR1 = ShiftRectangleToGoalByDelta(r1, center);
                var hasIntersect = false;
                foreach (var r2 in rectangles)
                {
                    if (r1 == r2)
                        continue;

                    if (newR1.IntersectsWith(r2))
                    {
                        hasIntersect = true;
                        break;
                    }
                }
                hasIntersect.Should().BeTrue();
            }
        }

        [TearDown]
        public void DerivedTearDown() 
        {
            var resultStatus = TestContext.CurrentContext.Result.Outcome.Status;
            if(resultStatus == NUnit.Framework.Interfaces.TestStatus.Failed && cloud!=null)
            {
                var path = @$"..\..\..\CloudImages\{TestContext.CurrentContext.Test.Name}.png";

                var xLocations = cloud.Rectangles.Select(rect => rect.X).OrderBy(x => x).ToList();
                var width = xLocations.Last() - xLocations.First();
                width = width < 100 ? 100 : 1000;

                var imageSize = new Size(width, width);

                var visualizator = new CloudVisualizator(imageSize, cloud.Center);
                visualizator.DrawRectangles(new Pen(new SolidBrush(Color.Red)), cloud.Rectangles);
                visualizator.SaveImage(path);

                TestContext.WriteLine($"Tag cloud visualization saved to file {Path.GetFullPath(path)}");
            }
        }

        private Rectangle ShiftRectangleToGoalByDelta(Rectangle rectangle, Point goal)
        {
            var vector = new Point(goal.X - rectangle.X, goal.Y - rectangle.Y);
            var newX = rectangle.X + Math.Sign(vector.X);
            var newY = rectangle.Y + Math.Sign(vector.Y);
            rectangle.Location = new Point(newX, newY);
            return rectangle;
        }
    }
}