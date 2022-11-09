using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    public class Tests
    {
        private CircularCloudLayouter circularCloud;
        private IImageFromTestSaver imageSaver;
        private Painter<Rectangle> painter;
        private List<Rectangle> rectangles;

        [SetUp]
        public void Setup()
        {
            circularCloud = new CircularCloudLayouter(Point.Empty);
            imageSaver = new ErrorHandler();
            painter = new RectanglePainter();
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Warning || rectangles.Count == 0) 
                return;
            
            var bitmapSize = painter.GetBitmapSize(rectangles);
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
                
            painter.Paint(rectangles, bitmap, painter.SetRandomRectangleColor);
            var imageSavedSuccessfully =
                imageSaver.TrySaveImageToFile(TestContext.CurrentContext.Test.Name, bitmap, out var path);

            var outputMessage = imageSavedSuccessfully
                    ? $"Tag cloud visualization saved to file <{path}>"
                    : $"Tag cloud visualization not saved";

            TestContext.Out.WriteLine(outputMessage);
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