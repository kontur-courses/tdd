using System;
using System.Drawing;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private Point center;
        [SetUp]
        public void SetUp()
        {
            center = new Point(1000,1000);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void BeEmpty_OnCreate()
        {
            var layout = new CircularCloudLayouter(center);
            layout.GetCloud().Should().BeEmpty();
        }

        [Test]

        public void HaveRectangle_AfterPutting()
        {
            layouter.PutNextRectangle(new Size(10, 10));
            layouter.GetCloud().Count.Should().Be(1);
        }

        [Test]
        public void ReturnValidSizeRectangle_AfterPut()
        {
            var size = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Size.ShouldBeEquivalentTo(size);
        }
        [Test]
        public void ReturnValidPointRectangle_AfterPut()
        {
            var size = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Size.ShouldBeEquivalentTo(size);
        }

        [Test]
        public void HaveZeroPointCenter_FirstPut()
        {
            var size = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.X.ShouldBeEquivalentTo(center.X-size.Width/2);
            rectangle.Y.ShouldBeEquivalentTo(center.Y +size.Height/ 2);
        }
        [Test]
        public void HaveNonZeroPointCenter_SecondPut()
        {
            var size = new Size(10, 10);
            layouter.PutNextRectangle(size);
            var secondRectangle = layouter.PutNextRectangle(size);
            Assert.AreNotEqual(secondRectangle.X-size.Width/2, center.X);
            Assert.AreNotEqual(secondRectangle.Y - size.Height/ 2, center.Y);

        }

        [Test]
        public void NewRectangleHaveValidPoint_SecondPut()
        {
            var firstSize = new Size(10,10);
            var firstRectangle = layouter.PutNextRectangle(firstSize);
            var secondSize = new Size(6,6);
            var secondRectangle = layouter.PutNextRectangle(secondSize);
            layouter.PutNextRectangle(new Size(4, 6));
            layouter.PutNextRectangle(new Size(7, 8));
            layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8)); layouter.PutNextRectangle(new Size(7, 8));
        }

        [Test]
        public void OutBitmap()
        {
            var x = 100;
            var y = 40;
            var random = new Random();
            
            for (int i = 0; i < 60; i++)
            {
                layouter.PutNextRectangle(new Size(x, y));
                if(x>20)
                x = x - random.Next(2, 7);
                if(y>15) y = y - random.Next(2, 7);
            }
            layouter.SaveBitmap("result.bmp");
        }



        [TearDown]
        public void TearDown()
        {
            if(TestContext.CurrentContext.Result.State==TestState.Failure|| TestContext.CurrentContext.Result.State == TestState.Error)
            layouter.SaveBitmap(TestContext.CurrentContext.Test.FullName);
        }


    }
}