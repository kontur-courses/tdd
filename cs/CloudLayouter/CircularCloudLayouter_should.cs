using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;


namespace CloudLayouter
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private string directoryToSave = Path.GetTempPath() + "bitmaps"; 
        private CircularCloudLayouter cloudLayouter;
        private Random random;

        
        
        [SetUp]
        public void Initalization()
        {
            cloudLayouter = new CircularCloudLayouter(new Point(0,0));
            random = new Random();
        }
        
        [Test]
        public void PutNextRectangleThrowsExeptionOnEmptySize()
        {
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
            for (int i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(random.Next(30, 51), random.Next(30, 51)));
            cloudLayouter.Count.Should().Be(count);
        }
       
        
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public void NextRectangleShouldNotIntersectWitPrevious(int count)
        {
            var previosRectangle = cloudLayouter.PutNextRectangle(new Size(random.Next(100), random.Next(100)));
            var nextRectangle = new Rectangle();
            for (int i = 0; i < count; i++)
            {
                nextRectangle = cloudLayouter.PutNextRectangle(new Size(random.Next(100), random.Next(100)));
                previosRectangle.IntersectsWith(nextRectangle).Should().BeFalse();
                previosRectangle = nextRectangle;
            }
        }

        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public void AllRectanglesShouldNotIntersectWithEachOther(int count)
        {
            var rectList = new List<Rectangle>();
            for (int i = 0; i < count; i++)
            {
                rectList.Add(cloudLayouter.PutNextRectangle(new Size(random.Next(100), random.Next(100))));
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
            var rectList = new List<Rectangle>();
            for (int i = 0; i < count; i++)
            {
                rectList.Add(cloudLayouter.PutNextRectangle(new Size(random.Next(100), random.Next(100))));
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
            if (!System.IO.Directory.Exists(directoryToSave))
                System.IO.Directory.CreateDirectory(directoryToSave);
            using (Bitmap bmp = new Bitmap(500,500))
            {
                CircularCloudLayouter cloudLayouter = new CircularCloudLayouter(new Point(250, 250));
                for (int i = 0; i < tagCount; i++)
                    cloudLayouter.PutNextRectangle(new Size(random.Next(20, 51), random.Next(10, 31)));
                cloudLayouter.Draw(bmp);
                bmp.Save(directoryToSave + Path.DirectorySeparatorChar + filename, ImageFormat.Png); 
            }
            Console.WriteLine(string.Format("Saved to:{0}",directoryToSave));
            
        }
        
        [TestCase("test1r.png",100)]
        [TestCase("test2r.png",150)]
        [TestCase("test3r.png",200)]
        public void DrawingTestAfterReforming(string filename, int  tagCount)
        {
            if (!Directory.Exists(directoryToSave))
                Directory.CreateDirectory(directoryToSave);
            using (Bitmap bmp = new Bitmap(500,500))
            {
                CircularCloudLayouter cloudLayouter = new CircularCloudLayouter(new Point(250, 250));
                for (int i = 0; i < tagCount; i++)
                    cloudLayouter.PutNextRectangle(new Size(random.Next(20, 51), random.Next(10, 31)));
                cloudLayouter.ReformTagCloud();
                cloudLayouter.Draw(bmp);
                bmp.Save(directoryToSave + Path.DirectorySeparatorChar + filename, ImageFormat.Png); 
            }
            Console.WriteLine("Saved to:{0}",directoryToSave);
        }
    }
}