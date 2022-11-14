using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    public class UnitTest1
    {
        private CircularCloudLayouter? circularCloudLayouter;
        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));
        }

        [TestCase(0,1, TestName = "{m}_zero_width")]
        [TestCase(1, 0, TestName = "{m}_zero_height")]
        public void PutNextRectangle_Should_Fail_On(int width,int height)
        {
            var size = new Size(width, height);
            Action putNextRectangle = () => circularCloudLayouter?.PutNextRectangle(size);

            putNextRectangle.Should().Throw<ArgumentException>();
        }

        [TestCase(0,3,2,2, TestName = "{m}_ReturnTrue_When_Rectangle_Intersects_Other_Rectangles",ExpectedResult = true)]
        [TestCase(-2, 0, 2, 2, TestName = "{m}_ReturnFalse_When_Rectangle_Intersects_Other_Rectangles", ExpectedResult = false)]
        public bool IsIntersectsOthersRectangles_Should(int x, int y, int width,int height)
        {
            var rectangle = new Rectangle(x, y, width, height);
            var notIntersectRectangles = new List<Rectangle>()
            {
                new Rectangle(-1,4,2,2),
                new Rectangle(1,1,2,2)
            };

            var result = rectangle.IsIntersectsOthersRectangles(notIntersectRectangles);

            return result;
        }

        [Test]
        public void IsIntersectsOthersRectangles_Should_Return_False_On_EmptyEnumerable()
        {
            var rectangle = new Rectangle(0,0,1,1);
            var rectangles = new List<Rectangle>();

            var result = rectangle.IsIntersectsOthersRectangles(rectangles);

            result.Should().BeFalse();
        }

        [Test]
        public void IsIntersectsOthersRectangles_Should_Return_False_On_NullEnumerable()
        {
            var rectangle = new Rectangle(0, 0, 1, 1);

            Action action =()=> rectangle.IsIntersectsOthersRectangles(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Test, Category("LayoutTest")]
        public void PutNextRectangle_Should_Return_Two_Not_Intersects_Rectangles()
        {
            var firstRectangle = circularCloudLayouter!.PutNextRectangle(new Size(10, 1));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(10,1));

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [TestCase(5,10,5,ExpectedResult = false, TestName = "{m}_5_AddedRectangles"), Category("LayoutTest")]
        [TestCase(500,10,5, ExpectedResult = false, TestName = "{m}_500_AddedRectangles"), Category("LayoutTest")]
        public bool PutNextRectangle_Should_Return_Rectangle_Not_Intersects_Past(int n,int width, int height)
        {
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < n; i++)
                rectangles.Add(circularCloudLayouter!.PutNextRectangle(new Size(10, 5)));

            var rectangle = circularCloudLayouter!.PutNextRectangle(new Size(10, 2));

            var result = rectangle.IsIntersectsOthersRectangles(rectangles);


            return result;
        }

        [Test]
        public void PutNextRectangle_Should_Return_Rectangle_Not_Intersects_Past_Added_Rectangles_With_Different_Sizes()
        {
            var rectangles = new List<Rectangle>();
            for (int i = 1; i <= 10; i++)
                rectangles.Add(circularCloudLayouter!.PutNextRectangle(new Size(i, i)));
            var rectangle = circularCloudLayouter!.PutNextRectangle(new Size(10, 2));

            var result = rectangle.IsIntersectsOthersRectangles(rectangles);

            result.Should().BeFalse();
        }

        [TearDown]
        public void AfterTests()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = @"C:\Users\harle\source\repos\tdd\cs\TagsCloudVisualization\FailedImages\" +
                           TestContext.CurrentContext.Test.Name + ".jpg";
                LayoutSaver.SaveFailedLayoutImageAsJpeg(path,new Size(100,100), circularCloudLayouter.Rectangles);
                TestContext.WriteLine("Tag cloud visualization saved to file <{0}>", path);
            }
        }
    }
}