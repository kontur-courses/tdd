using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class InfrastructureTagCloudShould
    {
        [Test]
        public void ThrowException_WhenVisualizatorFieldNull()
        {
            Action action = () => new Visualizator(null);
            action.Should().Throw<ArgumentNullException>();
        }



        [Test]
        public void FrequencyTags_TagsCount()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            frequencyTags.Count.Should().Be(4);
        }
        [Test]
        public void FrequencyTags_TagsShouldBeEqual()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            foreach (var pair in frequencyTags.GetDictionary())
                pair.Value.Should().Be(1);
        }

      






        [TestCase(0,TestName = "ZeroSize")]
        [TestCase(-1, TestName = "NegativeSize")]
        [TestCase(int.MinValue, TestName = "MinIntSize")]

        public void ThrowException_WhenDivideTagsNullOrNegative(int sizeTag)
        {
            Action action = () => new DivideTags(sizeTag);
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void DivideTags_CheckRoundDownToIntSizeTags()
        {
            var frequencyTags = new FrequencyTags(new []{"1","2","3","4"});
            var dividedTags = new DivideTags(5,frequencyTags);
            foreach (var pair in dividedTags.sizeDictionary)
                pair.Value.Should().Be(1); // 5/4 = 1.25 round to 1  
        }
        [Test]
        public void DivideTags_CheckRoundUpToIntSizeTags()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            var dividedTags = new DivideTags(7, frequencyTags);
            foreach (var pair in dividedTags.sizeDictionary)
                pair.Value.Should().Be(2); // 7/4 = 1.75 round to 2  
        }
        [Test]
        public void DivideTags_CheckRoundToIntSizeTags()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            var dividedTags = new DivideTags(6, frequencyTags);
            foreach (var pair in dividedTags.sizeDictionary)
                pair.Value.Should().Be(2); // 6/4 = 1.5 round to 2  
        }
        [Test]
        public void DivideTags_WhenDifferentSizeTags_CheckCount()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "2"});
            var dividedTags = new DivideTags(2, frequencyTags);
            dividedTags.sizeDictionary.Values.Count.Should().Be(2);
        }
        [Test]
        public void DivideTags_WhenDifferentSizeTags_CheckSize()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "2" });
            var size = 6;
            var dividedTags = new DivideTags(size, frequencyTags);
            dividedTags.sizeDictionary["2"].Should().Be(dividedTags.sizeDictionary["1"] * 2);
        }


        [Test]
        public void DivideTags_CheckForBigSize()
        {
            var frequencyTags = new FrequencyTags(new[] { "1", "2", "3", "4" });
            var size = int.MaxValue;
            var dividedTags = new DivideTags(size, frequencyTags);
            foreach (var pair in dividedTags.sizeDictionary)
                pair.Value.Should().Be((int)Math.Round((double)size/frequencyTags.Count));
        }
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

        [Test,Timeout(500)]
        public void TimeoutArithmeticSpiral_WhenBigCountOperation()
        {
            var spiral = new ArithmeticSpiral(new Point(0, 0));
            for (var i = 0; i < 10000000; i++)
                spiral.GetPoint();
        }

        [Test]
        public void ThrowException_WhenFrequencyTagsFieldNull()
        {
            Action action = () => new FrequencyTags(null);
            action.Should().Throw<ArgumentNullException>();
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
   

        [Test]
        public void NoVisualization_NoIntersections()
        {
            var tagCloud = new TagCloud();
            tagCloud.CreateTagCloud();
            var rectangles = tagCloud.GetRectangles();
            foreach (var rectangle in rectangles)
            {
                foreach (var thisRectangle in rectangles.Where(rect=> rect != rectangle))
                {
                    thisRectangle.rectangle.IntersectsWith(rectangle.rectangle).Should().BeFalse();
                }
            }
        }
        [Test]
        public void NoVisualization_NoArgumentExceptionWhenNotSpaceForSmallestTag()
        {
            Action runVisual = () => ArgumentException_WhenHaveEmptySpaceSmall();
            runVisual.Should().NotThrow<Exception>();
        }

        public void ArgumentException_WhenHaveEmptySpaceSmall()
        {
            var tagCloud = new TagCloud();
            tagCloud.CreateTagCloud();
            var srcSize = tagCloud.GetScreenSize();
            var arithmeticSpiral = new ArithmeticSpiral(new Point( srcSize / 2));
            var point = arithmeticSpiral.GetPoint();
            var rectangles=tagCloud.GetRectangles();
            var smallRectangle = rectangles.Last().rectangle;
            var smallOptions = new Tuple<string, Size, Font>("small", smallRectangle.Size, new Font("Times", 5));
            while (!new Rectangle(point - (smallOptions.Item2 / 2), smallOptions.Item2).IntersectsWith(smallRectangle))
            {
                point = arithmeticSpiral.GetPoint();
                if (!rectangles
                        .Select(x =>
                            x.rectangle.IntersectsWith(new Rectangle(point - (smallOptions.Item2 / 2), smallOptions.Item2)))
                        .Contains(true))
                {
                    throw new Exception($"Rectangle {smallOptions.Item2} input in space: {point - smallOptions.Item2 / 2}.But must in {smallRectangle.Location - smallOptions.Item2 / 2}");
                }
            }
        }
        [Test,Timeout(20000)]
        public void NoVisualization_Timeout()
        {
            var tagCloud = new TagCloud();
            tagCloud.CreateTagCloud();
        }
        [Test]
        public void Visualization_Timeout()
        {
            var tagCloud = new TagCloud();
            var visualizator = new Visualizator(tagCloud);
            visualizator.Save("Visualization_Timeout", tagCloud);
        }

        [Test]
        public void Visualization_WithRandomSave()
        {
            var testName = TestContext.CurrentContext.Test.Name+TestContext.CurrentContext.Result.FailCount;
            var random = new Random(123);
            var strsplt = new string[400];
            for (var i = 0; i < 400; i++)
                strsplt[i] = random.Next(1, 100).ToString();
            var tagCloud = new TagCloud(new FrequencyTags(string.Join(", ", strsplt).Split(", ")));
            var visualizator = new Visualizator(tagCloud);
            
            visualizator.Save(testName, tagCloud);
            Console.WriteLine("Tag cloud visualization saved to file " +
                              $"./TagsCloudVisualization/saved_images/{testName}");

        }
    }
}
