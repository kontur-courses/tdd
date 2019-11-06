using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter cloud;

        [SetUp]
        public void SetUp()
        {
            cloud = new CircularCloudLayouter(new Point(0, 0));
        }
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed))
            {
                var picturePath = String.Format(@"{0}\{1}.jpg", TestContext.CurrentContext.TestDirectory, TestContext.CurrentContext.Test.Name);
                var bitmap = Visualizer.GetCloudVisualization(cloud);
                bitmap.Save(picturePath, ImageFormat.Jpeg);
                Console.WriteLine(String.Format("Tag cloud visualization saved to file {0}", picturePath));
            }
        }

        private Point GetRectCenter(Rectangle rect)
        {
            return new Point(rect.Location.X + rect.Width / 2, rect.Location.Y + rect.Height / 2);
        }

        [Test]
        public void CloudsCenter_IsEqual_ToPointInConstructor()
        {
            var cloud = new CircularCloudLayouter(new Point(-5, 6));
            cloud.Center.Should().Be(new Point(-5, 6));
        }


        [Test]
        public void CloudsRectangles_IsEmpty_BeforeAnyWasPut()
        {
            cloud.Rectangles.Should().BeEmpty();
        }

        [TestCase(-1, 1, TestName = "Negative width")]
        [TestCase(1, -1, TestName = "Negative height")]
        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        public void PutNextRectangle_ThrowsArgumentException_OnWrongRectangle(int width, int height)
        {
            Action act = () => cloud.PutNextRectangle(new Size(width, height));
            act.ShouldThrow<ArgumentException>();
        }
    
    [TestCase(4, 2, TestName = "With even size")]
    [TestCase(1, 3, TestName = "With odd size")]
    public void PutNextRectangle_ReturnsCenter_OnFirstRectanglePut(int width, int height)
    {
        var center = new Point(-5, 20);
        cloud = new CircularCloudLayouter(center);
        var rectangleSize = new Size(width, height);
        var expectedLocation = new Point(cloud.Center.X - width / 2, cloud.Center.Y - height / 2);

        var rectangle = cloud.PutNextRectangle(rectangleSize);

        rectangle.Location.Should().Be(expectedLocation);
    }

    [Test]
    public void PutNextRectangle_LocateTwoRectangles_OnDifferentPoint()
    {
        var rect1 = cloud.PutNextRectangle(new Size(1, 1));
        var rect2 = cloud.PutNextRectangle(new Size(1, 1));

        rect1.Location.Should().NotBe(rect2.Location);
    }

    [Test]
    public void PutNextRectangle_LocateTwoRectangles_WithNoIntersections()
    {
        var rect1 = cloud.PutNextRectangle(new Size(10, 2));
        var rect2 = cloud.PutNextRectangle(new Size(40, 200));

        rect1.IntersectsWith(rect2).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_CollectNotIntersectingRectangles_On100Iterations()
    {
        var random = new Random();
            var count = 100;
        foreach (var size in SizeGenerator.GenerateRandomSize())
            {
                cloud.PutNextRectangle(size);
                count--;
                if (count <= 0) break;
            }

        foreach (var rectangle in cloud.Rectangles)
                rectangle.IntersectsWithAny(cloud.Rectangles
                    .Where(rect => !rect.Equals(rectangle)))
                    .Should().BeFalse();
        }
        public void PutNextRectangle_PutsRectanglesInCircleShape()
        {
            var random = new Random();
            var count = 100;
            var center = cloud.Center;
            foreach (var size in SizeGenerator.GenerateRandomSize())
            {
                cloud.PutNextRectangle(size);
                count--;
                if (count <= 0) break;
            }
            var minX = cloud.Rectangles.Min(rect => rect.Left);
            var minY = cloud.Rectangles.Min(rect => rect.Top);
            var maxX = cloud.Rectangles.Max(rect => rect.Right);
            var maxY = cloud.Rectangles.Max(rect => rect.Bottom);
            var rad = Math.Max(maxY - minY, maxX - minX);
            // (x - center_x)^2 + (y - center_y)^2 < radius^2.

            foreach (var rect in cloud.Rectangles)
            {
                var rectCenter = GetRectCenter(rect);
                var localRad = Math.Pow(rectCenter.X - center.X, 2)
                    + Math.Pow(rectCenter.Y - center.Y, 2);
                localRad.Should().BeLessThan(Math.Pow(rad, 2));
            }
        }


        //Тестирование работы TearDown
        [Test]
    public void PutNextRectangle_300Times_Fails()
    {
        for (var i = 0; i < 300; i++)
            cloud.PutNextRectangle(new Size(i+10, i+10));
        cloud.Rectangles.Count.Should().Be(10);
    }
}
}