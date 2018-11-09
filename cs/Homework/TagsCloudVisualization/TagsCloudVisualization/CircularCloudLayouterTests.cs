using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    public static class Drawer
    {
        public static void DrawAndSave(HashSet<Rectangle> rectangles, string fullPath)
        {
            var bitmap = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, 1000, 1000);
            foreach (var rect in rectangles)
                graphics.DrawRectangle(Pens.Black, rect);

            bitmap.Save(fullPath, ImageFormat.Png);
        }
    }


    [TestFixture]
    public class PutNextRectangle_Should
    {
        private readonly CircularCloudLayouter layouter = new CircularCloudLayouter(center: new Point(500, 500));
        private readonly Dictionary<string, HashSet<Rectangle>> rectanglesInTests =
            new Dictionary<string, HashSet<Rectangle>>();

        [Test]
        public void ReturnRectangleWithTheSameSize()
        {
            var newRectangle = layouter.PutNextRectangle(new Size(20, 15));

            Assert.That(newRectangle, Is.Not.Null);
            Assert.That(newRectangle.Size, Is.EqualTo(newRectangle.Size));

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name, new HashSet<Rectangle>{ newRectangle });
            layouter.Rectangles.Clear();
        }

        [TestCase(-1, 15, TestName = "Negative width")]
        [TestCase(0, 15, TestName = "Zero width")]
        [TestCase(15, -1, TestName = "Negative height")]
        [TestCase(15, 0, TestName = "Zero height")]
        public void ThrowArgumentException(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));
        }

        [Test]
        public void ReturnTwoNotIntersectingRectangles()
        {
            var rnd = new Random();
            var first = layouter.PutNextRectangle(new Size(rnd.Next(1, 100), rnd.Next(1, 100)));
            var second = layouter.PutNextRectangle(new Size(rnd.Next(1, 100), rnd.Next(1, 100)));

            layouter.HaveIntersection(first, second).Should().BeFalse();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name, new HashSet<Rectangle> { first, second });
            layouter.Rectangles.Clear();
        }

        [Test]
        public void ReturnTwoNotNestedRectangles()
        {
            var rnd = new Random();
            var first = layouter.PutNextRectangle(new Size(rnd.Next(1, 100), rnd.Next(1, 100)));
            var second = layouter.PutNextRectangle(new Size(rnd.Next(1, 100), rnd.Next(1, 100)));

            layouter.IsNestedRectangle(first, second).Should().BeFalse();
            layouter.IsNestedRectangle(second, first).Should().BeFalse();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name, new HashSet<Rectangle> { first, second });
            layouter.Rectangles.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            if (!rectanglesInTests.ContainsKey(testName))
                return;
            var debugPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var subfolderName = "";
            var fullPath = "";
            var args = TestContext.CurrentContext.Test.Arguments;

            if (TestContext.CurrentContext.Result.Outcome.Status.Equals(AssertionStatus.Failed))
            {
                subfolderName = "FailedTests";
                fullPath = string.Format("{0}{1}/{2}.png", debugPath, subfolderName, testName);

                Console.Error.WriteLine(string.Format("Tag cloud visualization saved to file {0}", fullPath));
            }
            else
            {
                subfolderName = "SuccessTests";
                fullPath = string.Format("{0}{1}/{2}.png", debugPath, subfolderName, testName);
            }

            Drawer.DrawAndSave(rectanglesInTests[testName], fullPath);
        }
    }

    [TestFixture]
    public class HaveIntersection_Should
    {
        private readonly CircularCloudLayouter layouter = new CircularCloudLayouter(
            center: new Point(500, 500));
        private readonly Dictionary<string, HashSet<Rectangle>> rectanglesInTests =
            new Dictionary<string, HashSet<Rectangle>>();

        private readonly Rectangle staticRectangle = new Rectangle(new Point(5, 0), new Size(15, 15));

        [TestCase(0, 5, 10, 2, TestName = "Left side")]
        [TestCase(0, 5, 5, 2, TestName = "Left side touching")]
        [TestCase(15, 5, 10, 2, TestName = "Right side")]
        [TestCase(20, 5, 10, 2, TestName = "Right side touching")]
        [TestCase(10, -5, 2, 10, TestName = "Top side")]
        [TestCase(10, -5, 2, 5, TestName = "Top side touching")]
        [TestCase(10, 10, 2, 10, TestName = "Bottom side")]
        [TestCase(5, 15, 2, 10, TestName = "Bottom side touching")]
        [TestCase(0, 5, 30, 2, TestName = "Left and right sides")]
        [TestCase(20, 5, 2, 30, TestName = "Top and bottom sides")]
        public void ReturnTrue(int x, int y, int width, int height)
        {
            var rectangle = new Rectangle(new Point(x, y), new Size(width, height));

            layouter.HaveIntersection(staticRectangle, rectangle).Should().BeTrue();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name,
                new HashSet<Rectangle>{rectangle, staticRectangle});
        }
        
        [Test]
        public void ReturnFalse_OneInAnother()
        {
            var rectangle = new Rectangle(new Point(10, 5), new Size(2, 2));

            layouter.HaveIntersection(rectangle, staticRectangle).Should().BeFalse();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name,
                new HashSet<Rectangle> { rectangle, staticRectangle });
        }

        [Test]
        public void ReturnFalse_NotIntersecting()
        {
            var rectangle = new Rectangle(new Point(50, 50), new Size(2, 2));

            layouter.HaveIntersection(rectangle, staticRectangle).Should().BeFalse();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name,
                new HashSet<Rectangle> { rectangle, staticRectangle });
        }

        [TearDown]
        public void TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            if (!rectanglesInTests.ContainsKey(testName))
                return;
            var debugPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var subfolderName = "";
            var fullPath = "";
            var args = TestContext.CurrentContext.Test.Arguments;

            if (TestContext.CurrentContext.Result.Outcome.Status.Equals(AssertionStatus.Failed))
            {
                subfolderName = "FailedTests";
                fullPath = string.Format("{0}{1}/{2}.png", debugPath, subfolderName, testName);

                Console.Error.WriteLine(string.Format("Tag cloud visualization saved to file {0}", fullPath));
            }
            else
            {
                subfolderName = "SuccessTests";
                fullPath = string.Format("{0}{1}/{2}.png", debugPath, subfolderName, testName);
            }

            Drawer.DrawAndSave(rectanglesInTests[testName], fullPath);
        }
    }

    [TestFixture]
    public class IsNestedRectangle_Should
    {
        private readonly CircularCloudLayouter layouter = new CircularCloudLayouter(
            center: new Point(500, 500));
        private readonly Dictionary<string, HashSet<Rectangle>> rectanglesInTests =
            new Dictionary<string, HashSet<Rectangle>>();

        [Test]
        public void ReturnTrue_OneInAnother()
        {
            var nestedRectangle = new Rectangle(new Point(10, 10), new Size(2, 2));
            var mainRectangle = new Rectangle(new Point(5, 5), new Size(20, 20));

            layouter.IsNestedRectangle(nestedRectangle, mainRectangle).Should().BeTrue();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name,
                new HashSet<Rectangle> { nestedRectangle, mainRectangle });
        }

        [Test]
        public void ReturnFalse_AnotherInOne()
        {
            var nestedRectangle = new Rectangle(new Point(5, 5), new Size(20, 20));
            var mainRectangle = new Rectangle(new Point(10, 10), new Size(2, 2));

            layouter.IsNestedRectangle(nestedRectangle, mainRectangle).Should().BeFalse();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name,
                new HashSet<Rectangle> { nestedRectangle, mainRectangle });
        }

        [Test]
        public void ReturnFalse_NotOneInAnother()
        {
            var nestedRectangle = new Rectangle(new Point(50, 50), new Size(2, 2));
            var mainRectangle = new Rectangle(new Point(5, 5), new Size(20, 20));

            layouter.IsNestedRectangle(nestedRectangle, mainRectangle).Should().BeFalse();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name,
                new HashSet<Rectangle> { nestedRectangle, mainRectangle });
        }

        [Test]
        public void ReturnFalse_TheSame()
        {
            var rectangle = new Rectangle(new Point(5, 5), new Size(20, 20));

            layouter.IsNestedRectangle(rectangle, rectangle).Should().BeFalse();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name,
                new HashSet<Rectangle> { rectangle });
        }

        [Test]
        public void ReturnFalse_WithTouching()
        {
            var nestedRectangle = new Rectangle(new Point(5, 5), new Size(5, 5));
            var mainRectangle = new Rectangle(new Point(5, 5), new Size(20, 20));

            layouter.IsNestedRectangle(nestedRectangle, mainRectangle).Should().BeFalse();

            rectanglesInTests.Add(TestContext.CurrentContext.Test.Name,
                new HashSet<Rectangle> { nestedRectangle, mainRectangle });
        }

        [TearDown]
        public void TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            if (!rectanglesInTests.ContainsKey(testName))
                return;
            var debugPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var subfolderName = "";
            var fullPath = "";
            var args = TestContext.CurrentContext.Test.Arguments;

            if (TestContext.CurrentContext.Result.Outcome.Status.Equals(AssertionStatus.Failed))
            {
                subfolderName = "FailedTests";
                fullPath = string.Format("{0}{1}/{2}.png", debugPath, subfolderName, testName);

                Console.Error.WriteLine(string.Format("Tag cloud visualization saved to file {0}", fullPath));
            }
            else
            {
                subfolderName = "SuccessTests";
                fullPath = string.Format("{0}{1}/{2}.png", debugPath, subfolderName, testName);
            }

            Drawer.DrawAndSave(rectanglesInTests[testName], fullPath);
        }
    }
}
