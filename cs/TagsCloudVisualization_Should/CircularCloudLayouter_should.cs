using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Should
{
    public class Tests
    {
        [Test]
        public void TagsCloudVisualization_DrawImageNoRectangles_ThrowException()
        {
            var cloud = new CircularCloudLayouter(new Point(100,100));

            Action act = () => cloud.CreateImage("");

            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void TagsCloudVisualization_CreateObjectWithNegativeXY_ThrowException()
        {
            var point = new Point(-1, -1);

            Action act = () => new CircularCloudLayouter(point);

            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void TagsCloudVisualization_AddRectangleWithNegativeSize_ThrowException()
        {
            var cloud = new CircularCloudLayouter(new Point(100,100));

            Action act = () => cloud.PutNextRectangle(new Size(-100, -100));

            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void TagsCloudVisualization_AddOneRectangle_ReturnOneRectangle()
        {
            var expectedRectangle = new Rectangle(new Point(30, 30), new Size(10, 20));
            var cloud = new CircularCloudLayouter(new Point(40, 40));

            var actual = cloud.PutNextRectangle(new Size(30, 30));

            actual.Should().Equals(expectedRectangle);
        }

        [Test]
        public void TagsCloudVisualization_RandomTenRectangles_RectanglesLengthIsTen()
        {
            var rnd = new Random();
            var cloud = new CircularCloudLayouter(new Point(500, 500));

            for (var i = 0; i < 10; i++)
            {
                var size = new Size(rnd.Next(10, 200), rnd.Next(10, 200));
                cloud.PutNextRectangle(size);
            }

            cloud.Rectangles.Count.Should().Be(10);
        }

        

        [Test]
        public void TagsCloudVisualization_AddTenRectanglesAndCreateImage_ImageExists()
        {
            var cloud = new CircularCloudLayouter(new Point(500, 500));
            var path = Directory.GetCurrentDirectory();
            var fileName = "10 rectangles-" + DateTime.Now.ToFileTime();

            for (var i = 1; i <= 10; i++)
                cloud.PutNextRectangle(new Size(10 * i, 10 * i));
            cloud.CreateImage(fileName);

            File.Exists($"{path}\\{fileName}.bmp").Should().BeTrue();
        }

        [Test, Timeout(150)]
        public void TagsCloudVisualization_AddRandomOneHundredRectanglesAndCreateImage_TimeLessThan150()
        {
            var cloud = new CircularCloudLayouter(new Point(500, 500));
            var path = Directory.GetCurrentDirectory();
            var fileName = "100 rectangles-" + DateTime.Now.ToFileTime();
            var rnd = new Random();

            for (var i = 1; i <= 100; i++)
                cloud.PutNextRectangle(new Size(rnd.Next(10, 100), rnd.Next(10, 100)));
            cloud.CreateImage(fileName);

            File.Exists($"{path}\\{fileName}.bmp").Should().BeTrue();
        }
    }
}