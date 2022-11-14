using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class PlacementTests
    {
        private BlockCloudLayouter layout;
        private TestsHelper helper;

        [SetUp]
        public void SetUp()
        {
            layout = new BlockCloudLayouter(new Point(1500, 1500));
            helper = new TestsHelper(layout);
        }

        [Test]
        public void FailTest()
        {
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                layout.PutNextRectangle(new Size(r.Next(10, 50), r.Next(1, 10)));
            }

            layout.PutNextRectangle(new Size(10, 10)).Should().Be(new Rectangle(0, 0, 0, 0));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                helper.FailedTestsDrawImage();
            }
        }
    }

    [TestFixture]
    [UseReporter(typeof(ApprovalTests.Reporters.VisualStudioReporter))]
    [UseApprovalSubdirectory(@"Results\BlockLayoutDrawingTests")]
    public class BlockLayoutDrawingTests
    {
        private TestsHelper helper;
        private BlockCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new BlockCloudLayouter(new Point(1000, 1000));
            helper = new TestsHelper(layouter);
        }

        [Test]
        public void DrawDefaultCloud()
        {
            layouter.PutNextRectangle(new Size(6, 2));
            layouter.PutNextRectangle(new Size(2, 4));
            layouter.PutNextRectangle(new Size(3, 2));
            layouter.PutNextRectangle(new Size(6, 1));
            layouter.PutNextRectangle(new Size(3, 2));
            layouter.PutNextRectangle(new Size(2, 2));
            layouter.PutNextRectangle(new Size(8, 2));
            layouter.PutNextRectangle(new Size(2, 4));
            layouter.PutNextRectangle(new Size(2, 1));
            layouter.PutNextRectangle(new Size(3, 2));
            layouter.PutNextRectangle(new Size(2, 3));

            helper.Approve();
        }

        [Test]
        public void DrawRandomCloud()
        {
            Random r = new Random(156);
            for (int i = 0; i < 500; i++)
            {
                layouter.PutNextRectangle(new Size(r.Next(10, 80), r.Next(1, 10)));
            }

            helper.Approve();
        }

        [Test]
        public void DrawGrowingSizeRectanglesCloud()
        {
            for (int i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(i, 2));
            }

            helper.Approve();
        }

        [Test]
        public void DrawSimpleCloud()
        {
            for (int i = 0; i < 2000; i++)
            {
                layouter.PutNextRectangle(new Size(4, 4));
            }

            helper.Approve();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                helper.FailedTestsDrawImage();
            }
        }
    }

    [TestFixture]
    [UseReporter(typeof(ApprovalTests.Reporters.VisualStudioReporter))]
    [UseApprovalSubdirectory(@"Results\SpiralLayoutDrawingTests")]
    public class SpiralLayoutDrawingTests
    {
        private SpiralCloudLayouter layouter;
        private TestsHelper helper;

        [SetUp]
        public void SetUp()
        {
            layouter = new SpiralCloudLayouter(new Point(1000, 1000));
            helper = new TestsHelper(layouter);
        }

        [Test]
        public void DrawSimpleCloud()
        {
            for (int i = 0; i < 2000; i++)
            {
                layouter.PutNextRectangle(new Size(4, 4));
            }

            helper.Approve();
        }

        [Test]
        public void DrawGrowingSizeRectanglesCloud()
        {
            for (int i = 0; i < 100; i++)
            {
                layouter.PutNextRectangle(new Size(i, 2));
            }

            helper.Approve();
        }

        [Test]
        public void Draw2Rectangles()
        {
            layouter.PutNextRectangle(new Size(1, 1));
            layouter.PutNextRectangle(new Size(5, 5));

            helper.Approve();
        }

        [Test]
        public void DrawRandomCloud()
        {
            Random r = new Random(223);
            for (int i = 0; i < 500; i++)
            {
                layouter.PutNextRectangle(new Size(r.Next(10, 80), r.Next(1, 10)));
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                helper.FailedTestsDrawImage();
            }
        }
    }

    internal class TestsHelper
    {
        private ICloudLayouter layouter;

        public TestsHelper(ICloudLayouter layouter)
        {
            this.layouter = layouter;
        }

        public void Approve()
        {
            Approvals.VerifyAll(TestContext.CurrentContext.Test.FullName, layouter.PlacedRectangles,
                "PlacedRectangles");
        }

        public void FailedTestsDrawImage()
        {
            TagCloudDrawer drawer = new TagCloudDrawer(layouter);
            drawer.SaveName = TestContext.CurrentContext.Test.FullName + ".jpg";
            drawer.Scale = 10;
            drawer.DrawImage();
            drawer.SaveImage();
        }
    }
}