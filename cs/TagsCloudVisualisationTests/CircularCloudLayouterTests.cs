using System.Drawing;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests
{
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class CircularCloudLayouterTests : LayouterTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void PutNextRectangleShould_FirstRectangle_PutAtCenter()
        {
            var size = new Size(10, 10);
            Layouter.PutAndTest(size, new Point(-5, -5));
        }

        [Test]
        public void PutNextRectangleShould_SecondRectangle_PutOnTopOfFirst()
        {
            Layouter.Put(new Size(10, 10), out _)
                .PutAndTest(new Size(7, 5), new Point(-5, -11));
        }
    }
}