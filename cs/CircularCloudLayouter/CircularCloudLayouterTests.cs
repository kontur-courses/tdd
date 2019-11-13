using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization

{
    [TestFixture]
    class ArchitectureOfTagsCloudShould
    {
        private CircularCloudLayouter basicLayouter;

        [SetUp]
        public void SetUp()
        {
            basicLayouter = new CircularCloudLayouter(new Point(100, 100));
        }

        [TestCase(0, 0)]
        public void Center_ShouldHaveCorrectCenterCoordinates_When_Create(int x, int y)
        {
            var circularLayouter = new CircularCloudLayouter(new Point(x, y));
            circularLayouter.Center.Should().Be(new Point(x, y));

        }

        [Test]
        public void Layouter_Should_HaveRectanglesList_WhenCreated()
        {
            basicLayouter.Rectangles.Should().NotBeNull();
        }

        [TestCase(0, 10, TestName = "WhenWidthIsZero")]
        [TestCase(10, 0, TestName = "WhenHeightIsZero")]
        [TestCase(0, 0, TestName = "WhenWidthAndHeightIsZero")]
        [TestCase(-1, 10, TestName = "WhenWidthIsNegative")]
        [TestCase(10, -1, TestName = "WhenHeightIsNegative")]
        [TestCase(-2, -1, TestName = "WhenWidthAndHeightIsNegative")]
        public void PutRectangle_ShouldThrowArgumentException_WhenPutRectangleWithIncorrectDimension(int width, int height)
        {
            Action action = () => basicLayouter.PutNextRectangle(new Size(width, height));
            action.Should().Throw<ArgumentException>().WithMessage("Width and Height should be greater than zero");
        }

        [TestCase(20, 20, 90, 90)]
        [TestCase(13, 13, 94, 94)]
        public void CenterOfTheFirstRectangle_Should_BeCorrect_WhenItPut(int width, int height, int x, int y)
        {
            basicLayouter.PutNextRectangle(new Size(width, height)).Location.Should().Be(new Point(x, y));
        }

        [Test]
        public void AllRectangles_Should_BeInRectanglesList()
        {
            Random random = new Random();
            for (int i = 0; i < 50; i++)
            {
                basicLayouter.PutNextRectangle(new Size(random.Next(1, 50), random.Next(1, 50)));
            }

            basicLayouter.Rectangles.Count.Should().Be(50);
        }

        [Test]
        [TestCaseSource(typeof(LayouterSource), "TestCases")]
        public void Cloud_Should_BeDenseAndRound(CircularCloudLayouter layouter)
        {
            var maxRadius = layouter.Rectangles
                .Max(rec => GetSegment(new Point(rec.X + rec.Width / 2, rec.Y + rec.Height / 2), layouter.Center));
            var area = (double)layouter.Rectangles.Select(rec => rec.Height * rec.Width).Sum();
            (area / (maxRadius * maxRadius * Math.PI)).Should().BeGreaterOrEqualTo(0.6);
        }


        [Test]
        [TestCaseSource(typeof(LayouterSource), "TestCases")]
        public void Rectangles_ShouldNot_Intersect(CircularCloudLayouter layouter)
        {
            layouter.Rectangles
                .Any(first =>
                    layouter.Rectangles.Any(second => second != first && first.IntersectsWith(second))).Should().BeFalse();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var layouter = TestContext.CurrentContext.Test.Arguments.FirstOrDefault() as CircularCloudLayouter;
                var path = Path.GetTempPath();
                SaveCloudWhenTestIsFallAndGetName(layouter, TestContext.CurrentContext,path);
                Console.WriteLine("Tag cloud visualization saved in directory:");
                Console.WriteLine(string.Format("{0}{1}.png",path,TestContext.CurrentContext.Test.Name));
            }
        }

        public static class LayouterSource
        {
            private static readonly CircularCloudLayouter layouterWithSameSizeRectangles =
                GetSameSizeRectnagles();
            private static readonly CircularCloudLayouter layouterWithDifferentSizeRectangles =
                GetDifferentSizeRectangles();

            public static CircularCloudLayouter GetSameSizeRectnagles()
            {
                var layouter = new CircularCloudLayouter(new Point(100, 100));  
                for (int i = 0; i < 50; i++)
                    layouter.PutNextRectangle(new Size(5, 5));
                return layouter;
            }

            public static CircularCloudLayouter GetDifferentSizeRectangles()
            {
                var layouter = new CircularCloudLayouter(new Point(100, 100));
                Random random = new Random();
                for (int i = 0; i < 50; i++)
                    layouter.PutNextRectangle(new Size(random.Next(1, 10), random.Next(1, 10)));
                return layouter;
            }

            private static TestCaseData DiffrenetSizeData =
                    new TestCaseData(layouterWithDifferentSizeRectangles).SetName("DiffetentSizeRectanglesTest");
            private static TestCaseData SameSizeData =
                    new TestCaseData(layouterWithSameSizeRectangles).SetName("SameSizeRectanglesTest");
            private static TestCaseData[] TestCases = { DiffrenetSizeData, SameSizeData };
        }

        private double GetSegment(Point start, Point end)
        {
            return Math.Sqrt((start.X - end.X) * (start.X - end.X) + (start.Y - end.Y) * (start.Y - end.Y));
        }

        private void SaveCloudWhenTestIsFallAndGetName(CircularCloudLayouter layouter
                , TestContext context,string path)
        {
            var cloud = new CloudVisualization(1000, 1000);
            var result = cloud.DrawRectangles(layouter.Rectangles);
            var name = string.Format("{0}.png", context.Test.Name);
            result.Save(Path.Combine(path, string.Format("{0}", name)));
        }
    }
}
