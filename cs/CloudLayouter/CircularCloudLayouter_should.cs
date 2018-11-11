using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;


namespace CloudLayouter
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private string directoryToSave = @"C:\Users\TheDAX\Desktop\Shpora\tdd\cs\bitmaps"; //Enter your own dirrectory to get Drawing result
        private CircularCloudLayouter cloudLayouter;

        [SetUp]
        public void Initalization()
        {
            cloudLayouter = new CircularCloudLayouter(new Point(0,0));
            cloudLayouter = new CircularCloudLayouter(new Point(0,0));
        }
        
        [Test]
        public void PutNextRectangleThrowsExeptionOnEmptySize()
        {
            CircularCloudLayouter cloudLayouter = new CircularCloudLayouter(new Point(0,0));
            Assert.Throws<ArgumentException>(() => cloudLayouter.PutNextRectangle(new Size()));
        }
        
        
        [Test]
        public void TwoRectanglesShouldNotIntersect()
        {
            var rect1 = cloudLayouter.PutNextRectangle(new Size(100, 50));
            var rect2 = cloudLayouter.PutNextRectangle(new Size(50, 50));
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [TestCase(11)]
        [TestCase(23)]
        [TestCase(31)]
        public void RectangleCountTest(int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(random.Next(30, 51), random.Next(30, 51)));
            cloudLayouter.Count.Should().Be(count);
        }
       
        
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public void NextRectangleShouldNotIntersectWitPrevious(int count)
        {
            Random rnd = new Random();
            Rectangle prevRect = cloudLayouter.PutNextRectangle(new Size(rnd.Next(100), rnd.Next(100)));
            Rectangle nextRect = new Rectangle();
            for (int i = 0; i < count; i++)
            {
                nextRect = cloudLayouter.PutNextRectangle(new Size(rnd.Next(100), rnd.Next(100)));
                prevRect.IntersectsWith(nextRect).Should().BeFalse();
                prevRect = nextRect;
            }
        }

        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public void AllRectanglesShouldNotIntersectWithEachOther(int count)
        {
            Random rnd = new Random();
            List<Rectangle> rectList = new List<Rectangle>();
            for (int i = 0; i < count; i++)
            {
                rectList.Add(cloudLayouter.PutNextRectangle(new Size(rnd.Next(100), rnd.Next(100))));
            }

            foreach (var rect in rectList)
            {
                rect.IntersectsWith(rectList.Where(x => x != rect)).Should().BeFalse();
            }
        }
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public void AllRectanglesShouldNotIntersectWithEachOtherAfterReforming(int count)
        {
            Random rnd = new Random();
            List<Rectangle> rectList = new List<Rectangle>();
            for (int i = 0; i < count; i++)
            {
                rectList.Add(cloudLayouter.PutNextRectangle(new Size(rnd.Next(100), rnd.Next(100))));
            }
            
            foreach (var rect in rectList)
            {
                rect.IntersectsWith(rectList.Where(x => x != rect)).Should().BeFalse();
            }
        }

        [TestCase(20)]
        [TestCase(30)]
        public void RectangleCountTestAfterReforming(int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(random.Next(30, 51), random.Next(30, 51)));
            cloudLayouter.ReformTagCloud();
            cloudLayouter.Count.Should().Be(count);
        }
        
        [TestCase("test1.png",100)]
        [TestCase("test2.png",150)]
        [TestCase("test3.png",200)]
        public void DrawingTest(string filename, int  tagCount)
        {
            if (System.IO.Directory.Exists(directoryToSave))
            {
                using (Bitmap bmp = new Bitmap(500,500))
                {
                    CircularCloudLayouter cloudLayouter = new CircularCloudLayouter(new Point(250, 250));
                    Random rand = new Random();
                    for (int i = 0; i < tagCount; i++)
                        cloudLayouter.PutNextRectangle(new Size(rand.Next(20, 51), rand.Next(10, 31)));
                    cloudLayouter.Draw(bmp);
                    bmp.Save(directoryToSave + Path.DirectorySeparatorChar + filename, ImageFormat.Png); 
                }
            }
        }
        
        [TestCase("test1r.png",100)]
        [TestCase("test2r.png",150)]
        [TestCase("test3r.png",200)]
        public void DrawingTestAfterReforming(string filename, int  tagCount)
        {
            if (System.IO.Directory.Exists(directoryToSave))
            {
                using (Bitmap bmp = new Bitmap(500,500))
                {
                    CircularCloudLayouter cloudLayouter = new CircularCloudLayouter(new Point(250, 250));
                    Random rand = new Random();
                    for (int i = 0; i < tagCount; i++)
                        cloudLayouter.PutNextRectangle(new Size(rand.Next(20, 51), rand.Next(10, 31)));
                    cloudLayouter.ReformTagCloud();
                    cloudLayouter.Draw(bmp);
                    bmp.Save(directoryToSave + Path.DirectorySeparatorChar + filename, ImageFormat.Png); 
                }
            }
        }
        
    }
}