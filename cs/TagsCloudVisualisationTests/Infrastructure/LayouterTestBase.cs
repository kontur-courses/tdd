using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualisation;
using TagsCloudVisualisation.Visualisation;

namespace TagsCloudVisualisationTests.Infrastructure
{
    /// <summary>
    /// Base class for ICircularCloudLayouter test
    /// Provides functionality to save visualisations of failed or broken tests
    /// </summary>
    [TestFixture]
    public abstract class LayouterTestBase
    {
        private TestStatus[] statusesToSaveImage;

        private static string TestingFilesDirectory =>
            Path.Combine(TestContext.CurrentContext.WorkDirectory, "test-results");

        /// <summary>
        /// Test subject
        /// </summary>
        protected ICircularCloudLayouter Layouter
        {
            get => layouterHolder;
            set => layouterHolder.Layouter = value;
        }

        private CircularCloudLayouterHolder layouterHolder;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            statusesToSaveImage = GetType().GetCustomAttribute<SaveLayouterResultsAttribute>()?.ValidStatuses ??
                                  new TestStatus[0];
        }

        [SetUp]
        public virtual void SetUp()
        {
            layouterHolder = new CircularCloudLayouterHolder();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (statusesToSaveImage.Contains(TestContext.CurrentContext.Result.Outcome.Status))
                SaveTestResult();
        }

        private void SaveTestResult()
        {
            if (layouterHolder.Layouter == null)
            {
                TestWriteLine($"Test subject {nameof(Layouter)} is null, can't save");
                return;
            }

            if (!layouterHolder.ResultRectangles.Any())
            {
                TestWriteLine("Nothing to save, output is empty");
                return;
            }

            var visualiser = new RectanglesVisualiser(Layouter.CloudCenter);
            foreach (var rectangle in layouterHolder.ResultRectangles)
                visualiser.Draw(rectangle.Colored(RandomColor()));

            var image = visualiser.GetImage()
                .DrawAxis(5, 1, Color.DarkGray, Color.Black)
                .FillBackground(Color.Bisque);

            if (!Directory.Exists(TestingFilesDirectory))
                Directory.CreateDirectory(TestingFilesDirectory);
            var filePath = Path.Combine(TestingFilesDirectory,
                $"{TestContext.CurrentContext.Test.Name}-test-result.png");

            try
            {
                image.Save(filePath);
            }
            catch (Exception e)
            {
                TestWriteLine($"Can't save cloud visualisation: {e}");
            }

            TestWriteLine($"Tag cloud visualization saved to file <{filePath}>");
        }

        private Color RandomColor() => Color.FromKnownColor(Randomizer.CreateRandomizer().NextEnum<KnownColor>());

        private static void TestWriteLine(string message) => TestContext.Out.WriteLine(message);

        private class CircularCloudLayouterHolder : ICircularCloudLayouter
        {
            private ICircularCloudLayouter layouter;
            public List<Rectangle> ResultRectangles = new List<Rectangle>();

            public ICircularCloudLayouter Layouter
            {
                get => layouter;
                set
                {
                    layouter = value;
                    ResultRectangles = new List<Rectangle>();
                }
            }

            public Point CloudCenter => layouter.CloudCenter;

            public Rectangle PutNextRectangle(Size rectangleSize)
            {
                var result = layouter.PutNextRectangle(rectangleSize);
                ResultRectangles.Add(result);
                return result;
            }
        }
    }
}