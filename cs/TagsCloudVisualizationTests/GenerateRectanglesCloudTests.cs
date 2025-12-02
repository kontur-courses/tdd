using System.Drawing;
using System.Runtime.Versioning;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    [SupportedOSPlatform("windows")]
    public class CloudVisualizerEdgeCasesTests
    {
        private string output;

        [SetUp]
        public void SetUp()
        {
            output = Path.Combine(TestContext.CurrentContext.WorkDirectory, "visualizer_edge");
            Directory.CreateDirectory(output);
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldNotThrow_WhenFileNameIsNull()
        {
            var sizes = new List<Size> { new(10, 10) };

            var act = () => CloudVisualizer.GenerateRectanglesCloud(null!, sizes, outputDirectory: output);

            act.Should().NotThrow();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldNotThrow_WhenFileNameIsEmpty()
        {
            var sizes = new List<Size> { new(5, 5) };

            var act = () => CloudVisualizer.GenerateRectanglesCloud("", sizes, outputDirectory: output);

            act.Should().NotThrow();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldNotThrow_WhenFileExtensionMissing()
        {
            var sizes = new List<Size> { new(10, 10) };

            var act = () => CloudVisualizer.GenerateRectanglesCloud("file", sizes, outputDirectory: output);

            act.Should().NotThrow();
        }

        [TestCase("file.txt")]
        [TestCase("image.svg")]
        [TestCase("output.docx")]
        public void GenerateRectanglesCloud_ShouldNotThrow_OnAnyExtension(string file)
        {
            var sizes = new List<Size> { new(10, 10) };

            var act = () => CloudVisualizer.GenerateRectanglesCloud(file, sizes, outputDirectory: output);

            act.Should().NotThrow();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldNotThrow_WhenSizesNull()
        {
            var act = () => CloudVisualizer.GenerateRectanglesCloud("out.png", rectSizes: null!, outputDirectory: output);

            act.Should().NotThrow();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldNotThrow_WhenSizesEmpty()
        {
            var act = () => CloudVisualizer.GenerateRectanglesCloud("out.png", [], outputDirectory: output);

            act.Should().NotThrow();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldNotThrow_WhenSizesExceedLimit()
        {
            var sizes = new List<Size>();
            for (var i = 0; i < 10001; i++)
                sizes.Add(new Size(10, 10));

            var act = () => CloudVisualizer.GenerateRectanglesCloud("too_many.png", sizes, outputDirectory: output);

            act.Should().NotThrow();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldNotThrow_WhenImageSizeTooLarge()
        {
            var sizes = new List<Size> { new(20, 20) };
            var bigSize = new Size(20000, 20000);

            var act = () => CloudVisualizer.GenerateRectanglesCloud("large.png", sizes, 
                imageSize: bigSize, outputDirectory: output);

            act.Should().NotThrow();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldNotThrow_WhenImageSizeZero()
        {
            var sizes = new List<Size> { new(50, 50) };

            var act = () => CloudVisualizer.GenerateRectanglesCloud("bad.png", sizes, 
                imageSize: new Size(0, 0), outputDirectory: output);

            act.Should().NotThrow();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldRespectProvidedImageSize()
        {
            var file = Path.Combine(output, "fixed.png");
            var sizes = new List<Size> { new(10, 10), new(20, 20) };

            CloudVisualizer.GenerateRectanglesCloud(
                fileName: file,
                rectSizes: sizes,
                imageSize: new Size(300, 200));

            using var bmp = new Bitmap(file);
            bmp.Width.Should().Be(300);
            bmp.Height.Should().Be(200);
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldApplyCustomColors()
        {
            var file = Path.Combine(output, "colors.png");
            var sizes = new List<Size> { new(10, 10), new(20, 20), new(20, 10) };

            CloudVisualizer.GenerateRectanglesCloud(
                fileName: file,
                rectSizes: sizes,
                backgroundColor: Color.Yellow,
                baseRectangleColor: Color.Red,
                rectangleBorderColor: Color.Green,
                outputDirectory: output);

            File.Exists(file).Should().BeTrue();

            using var bmp = new Bitmap(file);
            var bgColor = bmp.GetPixel(1, 1);
            bgColor.ToArgb().Should().Be(Color.Yellow.ToArgb());
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldCreateOutputDirectoryIfMissing()
        {
            var newDir = Path.Combine(output, "nested1/nested2/nested3");
            var file = Path.Combine(newDir, "auto.png");

            var sizes = new List<Size> { new(10, 10) };

            CloudVisualizer.GenerateRectanglesCloud(file, sizes);

            Directory.Exists(newDir).Should().BeTrue();
            File.Exists(file).Should().BeTrue();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldOverwriteExistingFile()
        {
            var file = Path.Combine(output, "overwrite.png");

            File.WriteAllText(file, "old content");

            var sizes = new List<Size> { new(40, 40) };

            CloudVisualizer.GenerateRectanglesCloud(file, sizes);

            using var bmp = new Bitmap(file);
            bmp.Width.Should().BeGreaterThan(0);
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldPlaceRectanglesWithoutOverlap()
        {
            var file = Path.Combine(output, "layout.png");

            var sizes = new List<Size>
            {
                new(50, 40),
                new(30, 20),
                new(10, 40),
                new(20, 30)
            };

            CloudVisualizer.GenerateRectanglesCloud(file, sizes);

            var layouter = new CircularCloudLayouter(new Point(0, 0));
            foreach (var s in sizes)
                layouter.PutNextRectangle(s);

            var rects = layouter.PlacedRectangles;

            for (var i = 0; i < rects.Count; i++)
                for (var j = i + 1; j < rects.Count; j++)
                    rects[i].IntersectsWith(rects[j]).Should().BeFalse();
        }

        [Test]
        public void GenerateRectanglesCloud_ShouldBeDeterministic()
        {
            var file1 = Path.Combine(output, "det1.png");
            var file2 = Path.Combine(output, "det2.png");

            var sizes = new List<Size> { new(30, 30), new(20, 20), new(10, 10) };

            CloudVisualizer.GenerateRectanglesCloud(file1, sizes);
            CloudVisualizer.GenerateRectanglesCloud(file2, sizes);

            using var b1 = new Bitmap(file1);
            using var b2 = new Bitmap(file2);

            b1.Width.Should().Be(b2.Width);
            b1.Height.Should().Be(b2.Height);
        }
    }
}
