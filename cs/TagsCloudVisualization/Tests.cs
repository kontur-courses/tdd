using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Primitives;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class Tests
    {
        private CircularCloudLayouter cloud;
        private TestContext currentContext;
        private const string filePath = "D://DebugFile.bmp";

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
                visualisator.RenderCurrentConfig(cloud, filePath);
                Console.WriteLine("Tag cloud visualization saved to file " + filePath);
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
        public void FirstRectangle_ShouldBeInCenter()
        {
            var rect = cloud.PutNextRectangle(new Size(10, 10));
            rect.Location.Should().Be(new Point(0, 0));
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
