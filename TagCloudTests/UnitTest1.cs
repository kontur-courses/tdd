using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloud2;

namespace TagCloudTests
{
    [TestFixture]
    public class Tests
    {
        private SpiralTagCloudEngine _engine = null!;
        
        [SetUp]
        public void Setup()
        {
            _engine = new SpiralTagCloudEngine(new Point(1280/2, 768/2));
        }
        
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) 
                return;
            
            SpiralTagCloudBitmapDrawer drawer = new(new Size(1280, 768), "Consolas");
            drawer.DrawRectangles(_engine.Rectangles.ToArray());
            drawer.Bitmap.Save($"{TestContext.CurrentContext.Test.Name}.jpg");
            Console.WriteLine($"Test {TestContext.CurrentContext.Test.Name} saved to \"{TestContext.CurrentContext.Test.Name}.jpg\"");
        }
        
        [Test]
        public void Test_ShouldFail()
        {
            for (var a = 0; a < 100; a++)
                _engine.PutNextRectangle(new Size(50, 50));
            _engine.PutNextRectangle(new Size(-1, -1));
        }
        
        [Test]
        public void Test_ShouldFai2()
        {
            for (var a = 0; a < 100; a++)
                _engine.PutNextRectangle(new Size(50, 50));
            _engine.PutNextRectangle(new Size(-1, -1));
        }

        [Test,Timeout(3000)]
        public void PutNextRectangle_ShouldBeFast_WhenInsertingManyItems()
        {
            for (var i = 0; i < 500; i++)
                _engine.PutNextRectangle(new Size(200, 60));
        }
        
        [Test]
        public void PutNextRectangle_ShouldAddRectangle_WhenInsertingItems()
        {
            for (var i = 0; i < 500; i++)
                _engine.PutNextRectangle(new Size(200, 60));

            _engine.Rectangles.Count.Should().Be(500);
        }
        
        [Test]
        [TestCase(500, 500, 0, 60, TestName = "bad rectangle size")]
        [TestCase(500, 500, 200, -1, TestName = "bad rectangle size")]
        public void PutNextRectangle_ShouldThrow_WhenBadRectSize(int px, int py, int rw, int rh)
        {
            Action a = () => _engine.PutNextRectangle(new Size(rw, rh));

            a.Should().Throw<ArgumentException>();
        }
        
        [Test]
        [TestCase(0, 500,  TestName = "bad point")]
        [TestCase(500, -1, TestName = "bad point")]
        public void PutNextRectangle_ShouldThrow_WhenBadPoint(int px, int py)
        {
            Action a = () =>  new SpiralTagCloudEngine(new Point(px, py));
            a.Should().Throw<ArgumentException>();
        }
    }
}
