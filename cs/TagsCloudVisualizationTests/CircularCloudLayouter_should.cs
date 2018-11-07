using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;


namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        public const int MaxWidth = 30;
        public const int MaxHeight = 20;

        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(100, 100));
        }


        [Test]
        public void not_contain_intersected_rectangles()
        {
            for (int i = 0; i < 4; i++)
            {
                layouter.PutNextRectangle(GetRandomSize());
            }

            foreach (var r1 in layouter.Rectangles)
                foreach (var r2 in layouter.Rectangles)
                    if (r1 != r2)
                        r1.IntersectsWith(r2).Should().BeFalse();
        }


        private Size GetRandomSize()
        {
            var r = new Random();
            return new Size(r.Next(MaxWidth), r.Next(MaxHeight));
        }
    }
}