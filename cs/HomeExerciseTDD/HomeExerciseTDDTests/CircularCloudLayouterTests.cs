using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using HomeExerciseTDD;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TestProject1
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layouter;
        private double radius;
        private List<Rectangle> rectanglesInCloud;

        [SetUp]
        public void Init()
        {
            
            radius = double.MinValue;
            rectanglesInCloud = new List<Rectangle>();
            var random = new Random();
            var x = random.Next(0, 40);
            var y = random.Next(0, 40);
            center = new Point(x,y);
            layouter = new CircularCloudLayouter(center);
        }
        
        [Test]
        public void PutNextRectangle_AlmostCircleFormAndArea_WhenPutManyRectangle()
        {
            var totalRectanglesArea = 0.0;
            var controlCircleArea = 0.0;

            for (var i = 0; i < 100; i++)
            {
                var newRectangle = layouter.PutNextRectangle(GetRandomSize(17,22));
                rectanglesInCloud.Add(newRectangle);
                totalRectanglesArea += newRectangle.Width * newRectangle.Height;
                var distance = GetMaxDistance(center, newRectangle);
                radius = distance > radius ? distance : radius;
            }
            controlCircleArea = Math.PI * Math.Pow(radius, 2);
            var fillSpacePercent = (totalRectanglesArea / controlCircleArea)*100;
            
            fillSpacePercent.Should().BeGreaterThan(60);
        }
        
        [Test]
        public void PutNextRectangle_ReturnNotNull_WhenPutSize()
        {
            var newRectangle = layouter.PutNextRectangle(GetRandomSize(4,8));
            rectanglesInCloud.Add(newRectangle);
            
            newRectangle.Should().NotBe(null);
        }
        
        [Test]
        public void PutNextRectangle_ReturnRectangleInCenter_WhenAddedFirstRectangle()
        {
            var firstRectangle = layouter.PutNextRectangle(GetRandomSize(19, 25));
            rectanglesInCloud.Add(firstRectangle);
            
            firstRectangle.Location.Should().BeEquivalentTo(center);
        }
        
        [Test]
        public void PutNextRectangle_ReturnArgumentException_WhenInvalidSize()
        {
            var invalidSize = new Size(-2,0);
            
            Action act = () =>layouter.PutNextRectangle(invalidSize);

            act.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void PutNextRectangle_RectanglesNotIntersect_WhenAddedManyRectangle()
        {
            var isIntersect = false;
            
            for (var i = 0; i < 1000; i++)
            {
                var newRectangle = layouter.PutNextRectangle(GetRandomSize(20,23));
                foreach (var rectangle in rectanglesInCloud)
                {
                    isIntersect = newRectangle.IntersectsWith(rectangle);
                }
                rectanglesInCloud.Add(newRectangle);
            }

            isIntersect.Should().Be(false);
        }

        [TearDown]
        public void TearDown()
        {
            var testDir = TestContext.CurrentContext.TestDirectory;
            var testName = TestContext.CurrentContext.Test.Name;

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                foreach (var rectangle in rectanglesInCloud)
                {
                    var distance = GetMaxDistance(center, rectangle);
                    radius = distance > radius ? distance : radius;
                }

                var imageSize = (int)(radius + 20) * 2;
                var format = ImageFormat.Png;
                layouter.DrawCircularCloud(imageSize, imageSize, $"{testName}.png", format);
                TestContext.Error.WriteLine($"Tag cloud visualization saved to file {testDir}\\{testName}.png");
            }
        }

        private double GetDistantBetweenPoints(Point first, Point second)
        {
            return Math.Sqrt(Math.Pow(second.X-first.X,2)+Math.Pow(second.Y-first.Y,2));
        }

        private double GetMaxDistance(Point center, Rectangle rectangle)
        {
            var maxDistant = double.MinValue;
            var rectanglePoints = new []
            {
                new Point(rectangle.Left, rectangle.Top),
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Right, rectangle.Bottom)
            };

            foreach (var point in rectanglePoints)
            {
                var distance = GetDistantBetweenPoints(point, center);
                maxDistant = distance > maxDistant ? distance : maxDistant;
            }
            return maxDistant;
        }

        private Size GetRandomSize(int LeftBorder, int RightBorder)
        {
            var randomazer = new Random();
            var width = randomazer.Next(LeftBorder, RightBorder);
            var height = randomazer.Next(LeftBorder, RightBorder);
            return new Size(width,height);
        }
    }
}