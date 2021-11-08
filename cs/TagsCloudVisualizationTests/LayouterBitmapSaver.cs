﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class LayouterBitmapSaver
    {
        private CircularCloudLayouter defaultLayouter;

        [SetUp]
        public void SetUp()
        {
            defaultLayouter = new CircularCloudLayouter();
        }

        public static List<Size> CreateRandomRectangles(int count)
        {
            var random = new Random();
            return Enumerable.Range(0, count).Select(_ =>
            {
                var width = random.Next(40, 100);
                var height = width / 5 + random.Next(-5, 5);
                return new Size(width, height);
            }).ToList();
        }

        [Test]
        [Explicit]
        public void PutNextRectangle_Squares_SaveToBitmap()
        {
            var square = new Size(10, 10);
            Enumerable.Range(0, 1000).ToList().ForEach(_ => defaultLayouter.PutNextRectangle(square));
            SaveRectanglesToBitmap(defaultLayouter, new RedPenStyle(), "squares-red");
        }

        [TestCaseSource(nameof(RandomRectanglesStyles))]
        [Explicit]
        public void CircularCloudLayouter_ColoredFillRandomRectangles_SaveToBitmap(IRectangleStyle style,
            string postfix)
        {
            CreateRandomRectangles(1000)
                .ForEach(rectangle => defaultLayouter.PutNextRectangle(rectangle));

            SaveRectanglesToBitmap(defaultLayouter, style, postfix);
        }

        private static IEnumerable<TestCaseData> RandomRectanglesStyles()
        {
            yield return new TestCaseData(new RedPenStyle(), "random-red");
            yield return new TestCaseData(new ColoredStyle(), "random-colored");
            yield return new TestCaseData(new ColoredFillStyle(), "random-colored-fill");
        }

        private static void SaveRectanglesToBitmap(CircularCloudLayouter layouter, IRectangleStyle style,
            string filenamePostfix = "")
        {
            var visualizer = new RectangleVisualizer(layouter.Rectangles) {Style = style};
            var savePath = Path.Combine(Directory.GetCurrentDirectory(),
                $"CircularCloudLayouter.Rectangles-{filenamePostfix}.bmp");

            new VisualOutput(visualizer).SaveToBitmap(savePath);
            TestContext.WriteLine($"Saved to '{savePath}'");
        }
    }
}