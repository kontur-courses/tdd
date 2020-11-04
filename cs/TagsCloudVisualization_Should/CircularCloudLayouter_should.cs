using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Should
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        
        [Test]
        public void CreateCircularCloudLayouterShould_ThrowArgumentException_CenterWithNegativeXOrY()
        {
            var point = new Point(-1, -1);

            Action act = () => new CircularCloudLayouter(point);

            act.ShouldThrow<ArgumentException>().WithMessage("X or Y of center was negative");
        }

        [Test]
        public void PutNextRectangle_ThrowArgumentException_SizeOfRectangleHaveNegativeValue()
        {
            var cloud = new CircularCloudLayouter(new Point(100,100));

            Action act = () => cloud.PutNextRectangle(new Size(-100, -100));

            act.ShouldThrow<ArgumentException>().WithMessage("Width or height of rectangle was negative");
        }

        [Test]
        public void PutNextRectangle_ReturnSameRectangle_OneRectangle()
        {
            var expectedRectangle = new Rectangle(new Point(25, 25), new Size(30, 30));
            var cloud = new CircularCloudLayouter(new Point(40, 40));

            var actual = cloud.PutNextRectangle(new Size(30, 30));

            actual.ShouldBeEquivalentTo(expectedRectangle);

        }
        
        [Test]
        public void Rectangles_CountIsTen_RandomTenRectangles()
        {
            var rnd = new Random();
            var cloud = new CircularCloudLayouter(new Point(500, 500));
            const int expectedLength = 10;

            for (var i = 0; i < 10; i++)
            {
                var size = new Size(rnd.Next(10, 200), rnd.Next(10, 200));
                cloud.PutNextRectangle(size);
            }
            var actualLength = cloud.Rectangles.Count;

            actualLength.Should().Be(expectedLength);
        }

        [Test]
        public void Rectangles_SameOrderLikeAdded_ThreeRectangles()
        {
            var cloud = new CircularCloudLayouter(new Point(500,500));
            var expectedRectangles = new List<Rectangle>
            {
                new Rectangle(new Point(485, 485), new Size(30, 30)),
                new Rectangle(new Point(485, 515), new Size(40, 40)),
                new Rectangle(new Point(465, 516), new Size(20, 20))
            };

            cloud.PutNextRectangle(new Size(30, 30));
            cloud.PutNextRectangle(new Size(40, 40));
            cloud.PutNextRectangle(new Size(20, 20));
            var actualRectangles = cloud.Rectangles;

            actualRectangles.ShouldAllBeEquivalentTo(expectedRectangles);

        }

        [Test]
        public void Center_HaveSameValues_AfterCreationObject()
        {
            var cloud = new CircularCloudLayouter(new Point(500, 500));
            var expectedCenter = new Point(500,500);

            var actualCenter = cloud.Center;

            actualCenter.ShouldBeEquivalentTo(expectedCenter);
        }
    }

    public class PointProviderShould
    {
        [Test]
        public void GetPoint_ReturnPoint_AfterCallingMethod()
        {
            var center = new Point(500,500);
            var pointProvider = new PointProvider(center);
            var expectedPoint = new Point(500,500);

            var actualPoint = pointProvider.GetPoint();

            actualPoint.ShouldBeEquivalentTo(expectedPoint);
        }

        [Test]
        public void CreatePointProvider_ThrowArgumentException_CenterWithNegativeXOrY()
        {
            var point = new Point(-1, -1);

            Action act = () => new PointProvider(point);

            act.ShouldThrow<ArgumentException>().WithMessage("X or Y of center was negative");
        }
    }

    public class DrawerShould
    {
        private Random rnd;

        [SetUp]
        public void SetUp()
        {
           rnd = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
        }

        [Test]
        public void DrawImage_ThrowArgumentException_CenterWithNegativeXOrY()
        {
            var rectangles = new List<Rectangle> { new Rectangle(1, 1, 1, 1) };
            Action act = () => Drawer.DrawImage(rectangles, new Point(-1, -1), "Image");

            act.ShouldThrow<ArgumentException>().WithMessage("X or Y of center was negative");
        }

        [Test]
        public void DrawImage_ThrowArgumentException_SequenceOfElementsIsEmpty()
        {

            Action act = () => Drawer.DrawImage(new List<Rectangle>(), new Point(), "Image");

            act.ShouldThrow<ArgumentException>().WithMessage("The sequence contains no elements");
        }

        [TestCase("1234567890", TestName = "DrawImage_ThrowArgumentException_FileNameContainsNumbers")]
        [TestCase(".,/\\!@#$%^&*()\";:?+-~`", TestName = "DrawImage_ThrowArgumentException_FileNameContainsSpecialSymbols")]
        public void DrawImage_ThrowArgumentException_FileNameContainsInvalidCharacters(string fileName)
        {
            var rectangles = new List<Rectangle> {new Rectangle(1, 1, 1, 1)};
            var center = new Point(2,1);

            Action act = () => Drawer.DrawImage(rectangles, center, fileName);

            act.ShouldThrow<ArgumentException>().WithMessage("File name contains invalid characters");
        }

        [Test]
        public void DrawImage_ImageExists_TenRandomRectangles()
        {
            var path = Directory.GetCurrentDirectory();
            const string fileName = "TenRectangles";
            var rectangles = RandomRectangles(100);

            Drawer.DrawImage(rectangles, new Point(500,500), fileName);

            File.Exists($"{path}\\{fileName}.bmp").Should().BeTrue();
        }


        [Test, Timeout(500)]
        public void DrawImage_TimeLessThan500Ms_TenThousandRandomRectangles()
        {
            const string fileName = "TenThousandRectangles";
            var rectangles = RandomRectangles(10000);

            Drawer.DrawImage(rectangles, new Point(500, 500), fileName);

        }

        private List<Rectangle> RandomRectangles(int count)
        {
            var rectangles = new List<Rectangle>();

            for(var i = 0; i < count; i++)
                rectangles.Add(new Rectangle(rnd.Next(100, 500), rnd.Next(100, 500), 
                    rnd.Next(100, 500), rnd.Next(100, 500)));

            return rectangles;
        }
    }
}