using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.IO;
using System.Threading;

namespace TagsCloudVisualization_Should
{
    [TestFixture]
    public class NotThrowExceptions
    {
        [TestCase(0, 0, TestName = "CreationZeroZero")]
        [TestCase(1, 2, TestName = "CreationBothPositive")]
        public void Creation(int x, int y)
        {
            var point = new Point(x,y);

            Action act = () => new CircularCloudLayouter(point);
            
            act.ShouldNotThrow();
        }
    }

    [TestFixture]
    public class ThrowExceptions
    {
        [Test]
        public void WhenCreateImageButNoRects()
        {
            var path = Directory.GetCurrentDirectory();
            var cloud = new CircularCloudLayouter(new Point(100,100));

            Action act = () => cloud.CreateImage(path,"NoRects");

            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void CreationBothNegative()
        {
            var point = new Point(-1, -1);

            Action act = () => new CircularCloudLayouter(point);

            act.ShouldThrow<ArgumentException>();
        }
        [Test]
        public void AddRectWithNegativeSize()
        {
            var cloud = new CircularCloudLayouter(new Point(100,100));

            Action act = () => cloud.PutNextRectangle(new Size(-100, -100));

            act.ShouldThrow<ArgumentException>();
        }
    }

    [TestFixture]
    public class AddRectangles
    {
        [TestCase(10,20,10,20,TestName = "AddOneRectangle_ReturnOneRectangle")]
        [TestCase(10, 10, 10, 10, TestName = "AddOneSquare_ReturnOneSquare")]
        public void AddRectangle(int x, int y, int expectedX, int expectedY, int startPoseX = 0, int startPoseY = 0)
        {
            var expectedSize = new Size(expectedX, expectedY);
            var expectedRectangle = new Rectangle(new Point(startPoseX,startPoseY), expectedSize);
            var cloud = new CircularCloudLayouter(new Point(startPoseX,startPoseY));

            var actual = cloud.PutNextRectangle(new Size(x, y));

            actual.Should().Equals(expectedRectangle);
        }

        [Test]
        public void RandomTenRectangles_RectanglesLengthIsTen()
        {
            var rnd = new Random();
            var cloud = new CircularCloudLayouter(new Point(500,500));

            for (var i = 0; i < 10; i++)
            {
                var size = new Size(rnd.Next(10,200), rnd.Next(10,200));
                cloud.PutNextRectangle(size);
            }

            cloud.Rects.Count.Should().Be(10);
        }
        
    }

    [TestFixture]
    public class CreateImage
    {
        [Test]
        public void AddTenRectangles()
        {
            var cloud = new CircularCloudLayouter(new Point(500, 500));
            var path = Directory.GetCurrentDirectory();
            var fileName = "TenRects";

            for (var i = 1; i <= 10; i++)
                cloud.PutNextRectangle(new Size(10 * i, 10 * i));
            cloud.CreateImage(path, fileName);

            File.Exists(path + "\\" + fileName + ".bmp").Should().BeTrue();
        }

        [Test]
        public void AddRandomOneHundredRectangles()
        {
            var cloud = new CircularCloudLayouter(new Point(500, 500));
            var path = Directory.GetCurrentDirectory();
            var fileName = "RandomOneHundredRects";
            var rnd = new Random();

            for (var i = 1; i <= 100; i++)
                cloud.PutNextRectangle(new Size(rnd.Next(10, 100), rnd.Next(10, 100)));
            cloud.CreateImage(path, fileName);

            File.Exists(path + "\\" + fileName + ".bmp").Should().BeTrue();
        }
    }

}