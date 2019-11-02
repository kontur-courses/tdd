using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter ccl;
        private List<Size> sizes;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            var rnd = new Random();

            sizes = new List<Size>();
            int count = rnd.Next(10, 50);
            for (int i = 0; i < count; i++)
            {
                int h = rnd.Next(10, 20);
                int w = h * (int)(2 + 8 * rnd.NextDouble());
                sizes.Add(new Size(w, h));
            }

            var center = new Point(rnd.Next(-100, 100), rnd.Next(-100, 100));
            ccl = new CircularCloudLayouter(center);
            sizes.ForEach(s => ccl.PutNextRectangle(s));
        }

        [Test]
        public void AllocateRectanglesWithoutIntersects()
        {
            Assert.IsTrue(false);
        }

        [Test]
        public void BeLikeCircle()
        {
            Assert.IsTrue(false);
        }

        [Test]
        public void BeCompact()
        {
            Assert.IsTrue(false);
        }
    }
}
