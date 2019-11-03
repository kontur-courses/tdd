using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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

            int count = rnd.Next(10, 50);
            sizes = new List<Size>(count);
            for (int i = 0; i < count; i++)
            {
                int h = rnd.Next(10, 20);
                int w = h * (int)(2 + 3 * rnd.NextDouble());
                sizes.Add(new Size(w, h));
            }

            var center = new Point(rnd.Next(-100, 100), rnd.Next(-100, 100));
            ccl = new CircularCloudLayouter(center);
            sizes.ForEach(s => ccl.PutNextRectangle(s));
        }

        [Test]
        public void AllocateRectanglesWithoutIntersects()
        {
            for (int i = 0; i < ccl.Items.Count; i++)
            {
                for (int j = i + 1; j < ccl.Items.Count - 1; j++)
                {
                    var r1 = ccl.Items[i];
                    var r2 = ccl.Items[j];
                    r1.IntersectsWith(r2).Should().BeFalse();
                }
            }
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

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var filename = $"{Path.GetTempPath()}{TestContext.CurrentContext.Test.Name}-Failed_{(int)DateTime.Now.TimeOfDay.TotalSeconds}.bmp";
                ccl.SaveToFile(filename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {filename}");
            }
        }
    }
}
