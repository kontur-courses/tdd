using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class FrequencyTagsShould
    {
        [Test]
        public void CalculateCount_WhenDifferentTags()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            frequencyTags.Count.Should().Be(4);
        }

        [Test]
        public void CalculateValueForEveryTag_WhenDifferentTags()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            foreach (var pair in frequencyTags.GetDictionary())
                pair.Value.Should().Be(1);
        }

        [Test]
        public void CalculateValue_WhenTagRepeat()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "2", "3" });
            frequencyTags.GetDictionary()["2"].Should().Be(2);
        }

        [Test]
        public void ThrowException_WhenFrequencyTagsFieldNull()
        {
            Action action = () => new FrequencyTags(null);
            action.Should().Throw<ArgumentNullException>();
        }

    }

    [TestFixture]
    public class DivideTagsShould
    {
        [TestCase(0, TestName = "ZeroSize")]
        [TestCase(-1, TestName = "NegativeSize")]
        [TestCase(int.MinValue, TestName = "MinIntSize")]
        public void ThrowException_WhenSizeTagNullOrNegative(int sizeTag)
        {
            Action action = () => new DivideTags(sizeTag);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestCase(5,1,TestName = "Round down")]// 5/4 = 1.25 round to 1  
        [TestCase(7,2,TestName = "Round up")]// 7/4 = 1.75 round to 2  
        [TestCase(6,2,TestName = "Round half fractional part of number")]// 6/4 = 1.5 round to 2  
        public void CheckTagSize_WhenMathRound(int tagSize,int expectedResult)
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            var dividedTags = new DivideTags(tagSize, frequencyTags);
            foreach (var pair in dividedTags.sizeDictionary)
                pair.Value.Should().Be(expectedResult); 
        }


        [Test]
        public void DivideTags_WhenDifferentSizeTags_CheckCount()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "2" });
            var dividedTags = new DivideTags(2, frequencyTags);
            dividedTags.sizeDictionary.Values.Count.Should().Be(2);
        }

        [Test]
        public void DivideTags_WhenDifferentSizeTags_CheckSize()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "2" });
            var dividedTags = new DivideTags(6, frequencyTags);
            dividedTags.sizeDictionary["2"].Should().Be(dividedTags.sizeDictionary["1"] * 2);
        }


        [Test]
        public void DivideTags_CheckForBigSize()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            var size = int.MaxValue;
            var dividedTags = new DivideTags(size, frequencyTags);
            foreach (var pair in dividedTags.sizeDictionary)
                pair.Value.Should().Be((int)Math.Round((double)size / frequencyTags.Count));
        }
    }

    [TestFixture]
    public class CircularCloudLayouterShould
    {
        [Test]
        public void CircularCloudLayouter_WhenTagsEmpty()
        {
            var frequencyTags = new FrequencyTags(new[] { "1" });
            var dividedTags = new DivideTags(1, frequencyTags);
            var circularCloudLayouter = new CircularCloudLayouter(dividedTags.sizeDictionary, new Size(1000, 2000));
            circularCloudLayouter.GetNextRectangle(new Point(0, 0));
            circularCloudLayouter.GetNextRectangle(new Point(0, 0)).IsEmpty.Should().BeTrue();
        }

        [Test]
        public void CircularCloudLayouterThrowException_WhenTagsEmpty()
        {
            var frequencyTags = new FrequencyTags(new[] { "1" });
            var dividedTags = new DivideTags(1, frequencyTags);
            var circularCloudLayouter = new CircularCloudLayouter(dividedTags.sizeDictionary, new Size(1000, 2000));
            for (var i = 0; i < 2; i++)
                circularCloudLayouter.GetNextRectangle(new Point(0, 0));
            Action exceptionAction = () => circularCloudLayouter.GetNextRectangle(new Point(0, 0));
            exceptionAction.Should().Throw<InvalidOperationException>();
        }
    }

    [TestFixture]
    public class ArithmeticSpiralShould
    {
        [Test, Timeout(500)]
        public void TimeoutArithmeticSpiral_WhenBigCountOperation()
        {
            var spiral = new ArithmeticSpiral(new Point(0, 0));
            for (var i = 0; i < 10000000; i++)
                spiral.GetPoint();
        }
        [Test]
        public void CreateArithmeticSpiral_WhenPointsIsPerpendicularlyEqual()
        {
            var spiral = new ArithmeticSpiral(new Point(0, 0));
            var pointList = new List<Point>();
            for (var i = 0; i < 100000; i++)
                pointList.Add(spiral.GetPoint());
            var yPoints = pointList.OrderBy(p => p.Y);
            var xPoints = pointList.OrderBy(p => p.X);
            Math.Abs(yPoints.First().Y).Should()
                .BeInRange(Math.Abs(yPoints.Last().Y) - 5, Math.Abs(yPoints.Last().Y) + 5);
            Math.Abs(xPoints.First().X).Should()
                .BeInRange(Math.Abs(xPoints.Last().X) - 5, Math.Abs(xPoints.Last().X) + 5);
        }
    }
    [TestFixture]
    public class TextRectangleShould
    {
        [TestCase("", TestName = "TextRectangle text is \"\"")]
        [TestCase(null, TestName = "TextRectangle text is null")]
        public void ThrowException_WhenNullTextRectangleFieldText(string text)
        {

            Action action = () => new TextRectangle(new Rectangle(10, 10, 10, 10), text, new Font("Times", 10));
            action.Should().Throw<ArgumentNullException>();
        }
        [Test]
        public void ThrowException_WhenNullTextRectangleFieldFont()
        {

            Action action = () => new TextRectangle(new Rectangle(10, 10, 10, 10), "text", null);
            action.Should().Throw<ArgumentNullException>();
        }
    }

    [TestFixture]
    public class TagCloudShould
    {
        private TagCloud tagCloud;
        private Visualizator visualizator;

        [TearDown]
        public void PrintPathAndSaveIfTestIsDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            if (tagCloud.GetRectangles().Count == 0)
                tagCloud.CreateTagCloud();
            visualizator = new Visualizator(tagCloud);
            var testName = TestContext.CurrentContext.Test.Name + TestContext.CurrentContext.Result.FailCount;
            visualizator.Save(testName, tagCloud);
            Console.WriteLine("Tag cloud visualization saved to file: " + Environment.CurrentDirectory + "\\" +
                              testName + ".png");
        }

        [SetUp]
        public void SetUp()
        {
            tagCloud = new TagCloud();
        }

        [Test]
        public void NoVisualization_NoIntersections()
        {
            var tagCloud = new TagCloud();
            tagCloud.CreateTagCloud();
            var rectangles = tagCloud.GetRectangles();
            foreach (var rectangle in rectangles)
            foreach (var thisRectangle in rectangles.Where(rect => rect != rectangle))
                thisRectangle.rectangle.IntersectsWith(rectangle.rectangle).Should().BeFalse();
        }
        [Test]
        public void ThrowException_WhenVisualizatorFieldNull()
        {
            Action action = () => new Visualizator(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void NoVisualization_NoArgumentExceptionWhenNotSpaceForSmallestTag()
        {
            Action runVisual = () => ArgumentException_WhenHaveEmptySpaceSmall();
            runVisual.Should().NotThrow<Exception>();
        }

        public void ArgumentException_WhenHaveEmptySpaceSmall()
        {
            tagCloud.CreateTagCloud();
            var srcSize = tagCloud.GetScreenSize();
            var arithmeticSpiral = new ArithmeticSpiral(new Point(srcSize / 2));
            var point = arithmeticSpiral.GetPoint();
            var rectangles = tagCloud.GetRectangles();
            var smallRectangle = rectangles.Last().rectangle;
            var smallOptions = new Tuple<string, Size, Font>("small", smallRectangle.Size, new Font("Times", 5));
            while (!new Rectangle(point - (smallOptions.Item2 / 2), smallOptions.Item2).IntersectsWith(smallRectangle))
            {
                point = arithmeticSpiral.GetPoint();
                if (!rectangles
                        .Select(x =>
                            x.rectangle.IntersectsWith(new Rectangle(point - (smallOptions.Item2 / 2),
                                smallOptions.Item2)))
                        .Contains(true))
                {
                    throw new Exception(
                        $"Rectangle {smallOptions.Item2} input in space: {point - smallOptions.Item2 / 2}.But must in {smallRectangle.Location - smallOptions.Item2 / 2}");
                }
            }
        }

        [Test, Timeout(10000)]
        public void NoVisualization_Timeout()
        {
            tagCloud.CreateTagCloud();
        }

        [Test, Timeout(10000)]
        public void Visualization_Timeout()
        {
            tagCloud.CreateTagCloud();
            visualizator = new Visualizator(tagCloud);
            visualizator.Save(TestContext.CurrentContext.Test.Name, tagCloud);
        }

        [Test]
        public void Visualization_WithRandomSave()
        {
            var random = new Random(123);
            var strsplt = new string[400];
            for (var i = 0; i < 400; i++)
                strsplt[i] = random.Next(1, 100).ToString();
            tagCloud = new TagCloud(new FrequencyTags(string.Join(", ", strsplt).Split(", ")));
            tagCloud.CreateTagCloud();
            visualizator = new Visualizator(tagCloud);
            visualizator.Save(TestContext.CurrentContext.Test.Name, tagCloud);

        }
    }
}
