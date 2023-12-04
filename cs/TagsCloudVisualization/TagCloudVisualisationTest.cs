using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class TagCloudVisualisationTest
    {
        //[SetUp]
        //public void GenerateRandomSizes()
        //{

        //}

        [TestCase(-1, -1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(0, 0)]
        public void CircularCloudLayouterConstructor_ThrowExceptionOnIncorrectCentralPoins(int x, int y)
        {
            Action a = () => new CircularCloudLayouter(new Point(x, y));
            a.Should().Throw<ArgumentException>();
        }

        [TestCase(1)]
        [TestCase(123)]
        public void CreateCloud_ReturnCorrectNumberOfRectangles(int rectCount)
        {
            var layouter = new CircularCloudLayouter(new Point(50, 50));

            var sizes = new List<Size>();

            for (int i = 0; i < rectCount; i++)
            {
                sizes.Add(new Size(1, 1));
            }

            var rects = layouter.CreateCloud(sizes);

            rects.Count().Should().Be(rectCount);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangle()
        {
            var layouter = new CircularCloudLayouter(new Point(50, 50));
            layouter.PutNextRectangle(new Size(1, 2)).Should().BeOfType(typeof(Rectangle));
        }

        [TestCase(1, 1)]
        [TestCase(20, 1)]
        [TestCase(256, 255)]
        [TestCase(1, 20)]
        public void PutNextRectangle_ShouldReturnRectangleOfCorrectSize(int width, int height)
        {
            var layouter = new CircularCloudLayouter(new Point(500, 500));
            var rect = layouter.PutNextRectangle(new Size(width, height));
            rect.Width.Should().Be(width);
            rect.Height.Should().Be(height);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void PutNextRectangle_ShouldThrowExceptionOnIncorrectSize(int width, int height)
        {
            var layouter = new CircularCloudLayouter(new Point(50, 50));
            Action a = () => layouter.PutNextRectangle(new Size(width, height));
            a.Should().Throw<ArgumentException>();
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(200)]
        public void CreateCloud_RectanglesShouldNotIntersect(int rectCount)
        {
            var layouter = new CircularCloudLayouter(new Point(500, 500));
            var sizes = new List<Size>();
            var rnd = new Random();

            for (int i = 0; i < rectCount; i++)
            {
                sizes.Add(new Size(rnd.Next(1, 100), rnd.Next(1, 100)));
            }

            var cloud = layouter.CreateCloud(sizes);

            for (int i = 0; i < cloud.Count; i++)
            {
                for (int j = i; j < cloud.Count; j++)
                {
                    if (i == j) continue;
                    var isIntersect = cloud[i].IntersectsWith(cloud[j]);
                    if (isIntersect)
                    {
                        var img = layouter.CreateImage();

                        //mark intersection
                        Graphics gr = Graphics.FromImage(img);
                        Pen pen = new Pen(Color.Red);
                        gr.DrawRectangle(pen, cloud[i]);
                        gr.DrawRectangle(pen, cloud[j]);
                        string filename = "error - " + DateTime.Now.ToString("H - mm - ss") + ".png";
                        img.Save(filename);
                        Console.WriteLine("Tag cloud visualization saved to file {0}", filename);
                    }

                    isIntersect.Should().BeFalse();
                }
            }
        }

    }
}
