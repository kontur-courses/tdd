using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

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

        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        [TestCase(-1, 1, TestName = "Negative width")]
        [TestCase(1, -1, TestName = "Negative height")]
        [Parallelizable(scope: ParallelScope.All)] 
        public void PutNextRectangle_IncorrectSize_ArgumentException(int width, int height)
        {
            var putNextRectangle = (Action) (() => circularCloud.PutNextRectangle(new Size(width, height)));
            putNextRectangle.Should().Throw<ArgumentException>();
        }

        [TestCase(new[] {3, 3, 2}, new[] {2, 1, 4})]
        [TestCase(new[] {1, 2, 4}, new[] {10, 10, 10})]
        [Parallelizable(scope: ParallelScope.None)] 
        public void PutNextRectangle_CorrectSize_FigureCount(int[] widths, int[] heights)
        {
            for (var index = 0; index < Math.Min(widths.Length, heights.Length); index++)
                circularCloud.PutNextRectangle(new Size(widths[index], heights[index]));

            circularCloud.Figures.Count.Should().Be(Math.Min(widths.Length, heights.Length));
        }
    }
}