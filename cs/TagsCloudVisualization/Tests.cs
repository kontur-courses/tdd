using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class Tests
    {
        private CircularCloudLayouter cloud;
        private TestContext currentContext;

        [SetUp]
        public void SetUp()
        {
            cloud = new CircularCloudLayouter(new Point(0, 0), 0.5);
            currentContext = TestContext.CurrentContext;
        }

        [TearDown]
        public void TearDown()
        {
            if (currentContext.Result.FailCount != 0)
            {
                var visualisator = new Visualiser(new Size(600, 600));
                visualisator.ShowCurrentConfig(cloud, "D://DebugFile.bmp");
                Console.WriteLine("Tag cloud visualization saved to file D://DebugFile.bmp");
            }
        }

        [Test]
        public void TagCloudConstructor_ShouldMakeCenter()
        {
            cloud.center.Should().Be(new Point(0, 0));
        }


        [Test]
        public void FirstRectange_ShouldBeNearCenter()
        {
            var rect = cloud.PutNextRectangle(new Size(10, 10));
            rect.Contains(cloud.center).Should().BeTrue();
        }

        [Test]
        public void Rectangles_ShouldNotIntersect()
        {
            var firstRectangle = cloud.PutNextRectangle(new Size(20, 10));
            var secondRectangle = cloud.PutNextRectangle(new Size(10, 20));
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void ArchimedesSpiralePointsMaker_ShouldReturnCenterAtFirst()
        {
            var value = ArchimedesSpiralePointsMaker
                .GenerateNextPoint(new Point(0, 0), 2).First();
            value.Should().Be(new Point(0, 0));
        }
    }
}
