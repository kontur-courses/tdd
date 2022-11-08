using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    public class Tests
    {
        private CircularCloudLayouter circularCloud;
        
        [SetUp]
        public void Setup()
        {
            circularCloud = new CircularCloudLayouter(Point.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Warning)
            {
            }
        }
        
        [TestCaseSource(typeof(TestData), nameof(TestData.IncorrectSize))]
        [Parallelizable(scope: ParallelScope.All)] 
        public void PutNextRectangle_IncorrectSize_ArgumentException(int width, int height)
        {
            var putNextRectangle = (Action) (() => circularCloud.PutNextRectangle(new Size(width, height)));
            putNextRectangle.Should().Throw<ArgumentException>();
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectSizes))]
        [Parallelizable(scope: ParallelScope.None)] 
        public void PutNextRectangle_CorrectSize_FigureCount(int[] widths, int[] heights)
        {
            for (var index = 0; index < Math.Min(widths.Length, heights.Length); index++)
                circularCloud.PutNextRectangle(new Size(widths[index], heights[index]));

            circularCloud.Figures.Count.Should().Be(Math.Min(widths.Length, heights.Length));
        }
    }
}