using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Drawing;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TagsCloudVisualizer
{
    [TestFixture]
    class CircularCloudLayouter_Tests
    {
        private readonly StringBuilder logs = new StringBuilder();
        private readonly Random random = new Random();
        private CircularCloudLayouter CCL;
        [SetUp]
        public void SetUpCCLAndLogs()
        {
            CCL = new CircularCloudLayouter(new ArchimedeanSpiral(new Point(500, 500)));
            logs.Clear();
        }

        [Test]
        public void PutNewRectangle_WithTwoRandomRectanglesRunning1000Times_MustNotIntersect()
        {
            for (int i = 0; i < 1000; i++)
            {
                SetUpCCLAndLogs();
                var rect1 = PutRandomRectangleAndLogIt(1, 10);
                var rect2 = PutRandomRectangleAndLogIt(1, 10);
                rect1.IntersectsWith(rect2).Should().BeFalse();
            }
        }

        public double GetDiagonalLength(Rectangle rect1)
        {
            var leftBottomCorner1 = new Point(rect1.Left, rect1.Bottom);
            var rightUpperCorner1 = new Point(rect1.Right, rect1.Top);
            return leftBottomCorner1.GetDistanceTo(rightUpperCorner1);
        }

        public double GetMaximalTightDistanceBetweenRectangles(Rectangle rect1, Rectangle rect2)
        {
            var rect1diagonal = GetDiagonalLength(rect1);
            var rect2diagonal = GetDiagonalLength(rect2);
            return rect1diagonal + rect2diagonal;
        }

        [Test]
        public void PutNewRectangle_WithTwoRandomRectanglesRunning10000Times_MustLocatedTightly()
        {
            for (int i = 0; i < 10000; i++)
            {
                SetUpCCLAndLogs();
                var rect1 = PutRandomRectangleAndLogIt(1, 30);
                var rect2 = PutRandomRectangleAndLogIt(1, 30);
                var distanceBetweenCenters = rect1.Location.GetDistanceTo(rect2.Location);
                var maxDistance = GetMaximalTightDistanceBetweenRectangles(rect1, rect2);
                distanceBetweenCenters.Should().BeLessThanOrEqualTo(maxDistance);
            }
        }

        public Rectangle PutRandomRectangleAndLogIt(int minSize, int maxSize)
        {
            var rect1 = CCL.PutNewRectangle(new Size(random.Next(minSize, maxSize),
                                                     random.Next(minSize, maxSize)));
            logs.Append(rect1.ToJson());
            return rect1;
        }

        [Test]
        public void PutNewRectangle_WithFourRandomRectanglesRunning1000Times_MustLocatedTightly()
        {
            for (int i = 0; i < 1000; i++)
            {
                SetUpCCLAndLogs();
                var rect1 = PutRandomRectangleAndLogIt(2, 8);
                for (int j = 0; j < 3; j++)
                {
                    logs.Append("\n");
                    var rectJ = PutRandomRectangleAndLogIt(2, 8);
                    var distanceBetweenCenters = rect1.Location.GetDistanceTo(rectJ.Location);
                    var maxDistance = GetMaximalTightDistanceBetweenRectangles(rect1, rectJ);
                    AssertThatWithLogging(distanceBetweenCenters <= maxDistance);
                }
            }
        }

        public void AssertThatWithLogging(bool assertion)
        {
            if (!assertion)
            {
                TestContext.WriteLine(logs.ToString());
            }
            Assert.That(assertion);
        }

        [Test]
        public void PutNewRectangle_WithNonPositiveSize_ShouldThrow()
        {
            for (int i = 0; i < 1000; i++)
            {
                var size = new Size(random.Next(-4, 1), random.Next(-4, 1));
                Action t = () => CCL.PutNewRectangle(size);
                t.Should().Throw<ArgumentException>();
            }
        }

        [TearDown]
        public void TearDown()
        {
            var result = TestContext.CurrentContext.Result.GetSecretUsingFieldInfo();
            var rects = result.Split('\n');
            var rectList = new List<Rectangle>();
            if (TestContext.CurrentContext.Result.FailCount > 0)
            {
                for (int i = 0; i < rects.Length - 1; i++)
                {
                    var rect = rects[i];
                    var j = (JsonConvert.DeserializeObject<JsonRectangle>(rect));
                    rectList.Add(new Rectangle(j.X, j.Y, j.Width, j.Height));
                }
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                BitmapSaver.SaveRectangleRainbowBitmap(rectList, filePath + "\\Bitmap4.bmp");
            }
        }
    }

    public class JsonRectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
    public static class SecretFinder
    {
        public static string GetSecretUsingFieldInfo(this TestContext.ResultAdapter keeper)
        {
            FieldInfo fieldInfo = typeof(TestContext.ResultAdapter).GetField("_result", BindingFlags.Instance | BindingFlags.NonPublic);
            return ((TestCaseResult)fieldInfo.GetValue(keeper)).Output;
        }
    }
}
