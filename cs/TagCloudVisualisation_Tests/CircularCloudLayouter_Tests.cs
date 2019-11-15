using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloudVisualization;

namespace TagCloudVisualization_Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private static Point center;
        private static CircularCloudLayouter cloudLayouter;
        [SetUp]
        public void Setup()
        {
            center = new Point(0, 0);
            cloudLayouter = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void TearDown()
        {
            var result = TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed) ? "Failed" : "Successful";
            var workingDirectory = Environment.CurrentDirectory;
            var testFullName = TestContext.CurrentContext.Test.Name;
            var savePath = workingDirectory + "\\" + result + "Test" + testFullName + ".bmp";
            Console.WriteLine("Tag cloud visualization saved to file " + savePath);
            TagDrawer.Draw(savePath, cloudLayouter);
        }

        [Test]
        public void SizeOfCloud_ReturnsSameSize_OnPositiveSize()
        {
            var size = new Size(10, 5);

            cloudLayouter.PutNextRectangle(size);
            var currentSize = cloudLayouter.SizeOfCloud;

            currentSize.Should().Be(size);
        }

        [TestCase(10, 10)]
        [TestCase(50, 50)]
        public void LayouterRectangles_NotIntersect_WhileAdding(int widthMax, int heightMax)
        {
            var sizes = GetSizesList(50, 50);

            var rectangles = cloudLayouter.Rectangles;

            sizes.ForEach(s => cloudLayouter.PutNextRectangle(s));
            var isAnyIntersects = rectangles.Any(r1 => rectangles.Any(r2 => (r1 != r2 && r1.IntersectsWith(r2))));

            isAnyIntersects.Should().BeFalse();
        }

        [Test]
        public void Layouter_ContainsTheSameSizes_OnAddingRectanglesList()
        {

            var sizes = GetSizesList(50, 50);

            sizes.ForEach(s => cloudLayouter.PutNextRectangle(s));
            var rectangles = cloudLayouter.Rectangles.Select(rect => new Size(rect.Width, rect.Height));

            rectangles.Should().BeEquivalentTo(sizes);
        }


        [TestCase(5, 5)]
        [TestCase(20, 20)]
        public void PutNextRectangle_ReturnSameSizeRectangle_OnPositiveSizes(int width, int height)
        {
            var size = new Size(width, height);

            var rectangleSize = cloudLayouter.PutNextRectangle(size).Size;

            rectangleSize.Should().Be(size);
        }

        private static IEnumerable<Size> TestCases
        {
            get
            {
                yield return new Size(0, 10);
                yield return new Size(10, 0);
                yield return Size.Empty;
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PutNextRectangle_ThrowsException_OnIncorrectSize(Size size)
        {
            Action rectAdding = () => cloudLayouter.PutNextRectangle(size);

            rectAdding.Should().Throw<ArgumentException>();
        }
        [Test]
        public void PutNextRectangle_OptimalRectanglesLocation_OnManyRectangles()
        {
            var accuracy = 40;
            var sizes =  GetSizesList(50, 50);


            sizes.ForEach(s => cloudLayouter.PutNextRectangle(s));
            var radius = Math.Max(LengthToPint(center, cloudLayouter.RightUpperPointOfCloud),
                    LengthToPint(center, cloudLayouter.LeftDownPointOfCloud));
            var sumOfRectanglesSquares = cloudLayouter.Rectangles.Select(r => r.Width * r.Height).Sum();
            var squareOfCircle = Math.PI * radius * radius;
            var isOptimal =  sumOfRectanglesSquares / squareOfCircle * 100 < accuracy;

            isOptimal.Should().BeTrue();
        }

        public double LengthToPint(Point first, Point second)
        {
            return Math.Sqrt((first.X - second.X) * (first.X - second.X) +
                             (first.Y - second.Y) * (first.Y - second.Y));
        }

        private static List<Size> GetSizesList(int widthMax, int heightMax)
        {
            var sizes = new List<Size>();
            for (var width = 5; width < widthMax; width += 2)
            {
                for (var height = 5; height < heightMax; height += 2)
                {
                    sizes.Add(new Size(width, height));
                }
            }

            return sizes;
        }

    }
}
