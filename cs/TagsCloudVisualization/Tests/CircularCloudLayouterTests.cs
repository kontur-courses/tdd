using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private ICircularCloudLayouter circularCloudLayouter;

    private static IEnumerable<TestCaseData> withZeroSizeTestCases
        = CircularCloudLayouterTestCases.GetRectanglesWithZeroSizesTestData;

    private static IEnumerable<TestCaseData> checkRectangleLocationTestCases
        = CircularCloudLayouterTestCases.GetCorrectRectangleSizesWithEndsLocationTestData;

    private static IEnumerable<TestCaseData> checkRectangleSizeTestCases
        = CircularCloudLayouterTestCases.GetCorrectRectangleSizesTestData;

    private static IEnumerable<TestCaseData> checkUnusualRectangleSizeTestCases
        = CircularCloudLayouterTestCases.GetCorrectUnusualRectanglesSizeTestData;

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            return;
        var rectangles = circularCloudLayouter.GetRectangles();
        var imageSize = new Size(1000, 1000);
        using var tagsCloudVisualizer = new TagsCloudVisualizer(imageSize);
        tagsCloudVisualizer.AddVisualizationRectangles(rectangles);
        SaveErrorVisualization(tagsCloudVisualizer, TestContext.CurrentContext);
    }

    [Test]
    [TestCaseSource(nameof(withZeroSizeTestCases))]
    public void PutNextRectangle_ShouldThrowArgumentException_WhenSizeIsZero(Point center, Size rectangleSize)
    {
        circularCloudLayouter = new CircularCloudLayouter(center);

        var action = () => circularCloudLayouter.PutNextRectangle(rectangleSize);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Размер ширины м высоты должен быть больше 0.");
    }

    [Test]
    [TestCaseSource(nameof(checkRectangleLocationTestCases))]
    public void PutNextRectangle_ShouldLastRectanglesInLocation_WhenBeforePutNextRectangles(Point center,
        IList<Size> sizes, Point shouldLocation)
    {
        if (!sizes.Any())
        {
            throw new ArgumentNullException(nameof(sizes));
        }

        circularCloudLayouter = new CircularCloudLayouter(center);

        var rectangles = sizes.Select(size => circularCloudLayouter.PutNextRectangle(size)).ToList();
        var rectangle = rectangles.Last();

        rectangle.Location.Should()
            .BeEquivalentTo(shouldLocation);
    }

    [Test]
    [TestCaseSource(nameof(checkRectangleSizeTestCases))]
    public void PutNextRectangle_ShouldFirstRectanglesInCenter_WhenAfterPutNextRectangles(Point center,
        IList<Size> sizes)
    {
        if (!sizes.Any())
        {
            throw new ArgumentNullException(nameof(sizes));
        }

        circularCloudLayouter = new CircularCloudLayouter(center);
        var firstRectangleSize = sizes.First();

        var firstRectangle = circularCloudLayouter.PutNextRectangle(firstRectangleSize);
        var _ = sizes.Skip(1).Select(size => circularCloudLayouter.PutNextRectangle(size));

        firstRectangle.Center().Should()
            .BeEquivalentTo(center);
    }

    [Test]
    [TestCaseSource(nameof(checkRectangleSizeTestCases))]
    [TestCaseSource(nameof(checkUnusualRectangleSizeTestCases))]
    public void PutNextRectangle_ShouldRectanglesNotInCenter_WhenPutNextRectangles(Point center, IList<Size> sizes)
    {
        var rectangles = new List<Rectangle>();
        circularCloudLayouter = new CircularCloudLayouter(center);

        if (sizes.Any())
        {
            var _ = circularCloudLayouter.PutNextRectangle(sizes.First());
            rectangles.AddRange(sizes.Skip(1).Select(size => circularCloudLayouter.PutNextRectangle(size)));
        }

        rectangles.Should().NotContain(r =>
            r.Contains(center)
        );
    }

    [Test]
    [TestCaseSource(nameof(checkRectangleSizeTestCases))]
    [TestCaseSource(nameof(checkUnusualRectangleSizeTestCases))]
    public void PutNextRectangle_ShouldEquivalentSize_WhenPutNextRectangles(Point center, IList<Size> sizes)
    {
        circularCloudLayouter = new CircularCloudLayouter(center);
        var rectangles = sizes.Select(size => circularCloudLayouter.PutNextRectangle(size)).ToList();

        rectangles.Select(r => r.Size).Should()
            .BeEquivalentTo(sizes);
    }

    [Test]
    [TestCaseSource(nameof(checkRectangleSizeTestCases))]
    [TestCaseSource(nameof(checkUnusualRectangleSizeTestCases))]
    public void PutNextRectangle_NotShouldIntersect_WhenPutNextRectangles(Point center, IList<Size> sizes)
    {
        circularCloudLayouter = new CircularCloudLayouter(center);
        var rectangles = sizes.Select(size => circularCloudLayouter.PutNextRectangle(size)).ToList();
        var intersectingRectangles = new List<Rectangle>();

        foreach (var checkRectangle in rectangles)
        {
            intersectingRectangles.AddRange(rectangles
                .Where(rectangle => checkRectangle != rectangle && rectangle.IntersectsWith(checkRectangle)));
        }

        intersectingRectangles.Should().BeEmpty();
    }

    private void SaveErrorVisualization(ITagsCloudVisualizer tagsCloudVisualizer, TestContext testContext)
    {
        var imageFormat = ImageFormat.Png;
        var imagePath = GetTestErrorImagesPath(testContext, imageFormat);
        tagsCloudVisualizer.Save(imagePath, imageFormat);
        TestContext.WriteLine($"Tag cloud visualization saved to file {imagePath}");
    }

    private string GetTestErrorImagesPath(TestContext testContext, ImageFormat imageFormat)
    {
        const string imagesFolderName = "ImagesWhenErrorInTests";
        var path = testContext.TestDirectory;
        var imagesDirectoryPath = Path.Combine(path, imagesFolderName);
        var testName = testContext.Test.Name;
        Directory.CreateDirectory(imagesDirectoryPath);
        var fileName = Path.ChangeExtension(testName, imageFormat.ToString().ToLowerInvariant());
        return Path.Combine(imagesDirectoryPath, fileName);
    }
}