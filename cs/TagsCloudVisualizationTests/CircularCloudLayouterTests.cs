using System.Drawing;
using TagsCloudVisualization;
using System.Drawing.Imaging;
using FakeItEasy;
using TagsCloudVisualization.Interface;


namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private AppSettings defaultSettings;
    private List<Rectangle> placesRectangles;
    private Point center;
    
    [SetUp]
    public void SetUp()
    {
        center = new Point(500, 500); 
        defaultSettings = new AppSettings(padding: 0, imageSize: new Size(1000, 1000));
        placesRectangles = new List<Rectangle>();
    }
    
    

    [Test]
    public void FirstRectangleInCenter_Test()
    {
        var customCenter = new Point(100, 60);
        var rectangleSize = new Size(20, 10);
        var fakeGenerator = A.Fake<IPointGenerator>();
        A.CallTo(() => fakeGenerator.Center).Returns(customCenter);
        var layouter = new CircularCloudLayouter(defaultSettings, fakeGenerator);
        
        var rectangle = layouter.PutNextRectangle(rectangleSize);
        var expectedX = customCenter.X - rectangleSize.Width / 2;
        var expectedY = customCenter.Y - rectangleSize.Height / 2;
        placesRectangles.Add(rectangle);
        
        Assert.That(rectangle.X, Is.EqualTo(expectedX));
        Assert.That(rectangle.Y, Is.EqualTo(expectedY));
        Assert.That(rectangle.Width, Is.EqualTo(rectangleSize.Width));
        Assert.That(rectangle.Height, Is.EqualTo(rectangleSize.Height));
    }
    

    [Test]
    public void PutNextRectangle_NoIntersection_Test()
    {
        var settingsWithPadding = new AppSettings(padding: 5);
        var generator = new SpiralPointGenerator(center, settingsWithPadding.SpiralDensity, 1);
        var fakeGenerator = A.Fake<IPointGenerator>();
        A.CallTo(() => fakeGenerator.GeneratePoints()).Returns(generator.GeneratePoints());
        A.CallTo(() => fakeGenerator.Center).Returns(center);
        
        var layouter = new CircularCloudLayouter(settingsWithPadding, fakeGenerator); 
        
        var random = new Random();
        var rectangles = new List<Rectangle>();
        var count = 50;
        
        for (int i = 0; i < count; i++)
        {
            var size = new Size(random.Next(10, 50), random.Next(10, 50));
            var rect = layouter.PutNextRectangle(size);
            rectangles.Add(rect);
            placesRectangles.Add(rect);
        }
        
        for (int i = 0; i < rectangles.Count; i++)
        {
            for (int j = i + 1; j < rectangles.Count; j++)
            {
                var rect1 = rectangles[i];
                var rect2 = rectangles[j];
                
                Assert.That(rect1.IntersectsWith(rect2), Is.False, 
                    $"Прямоугольники пересеклись.");
            }
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            SaveFailedLayout();
        }
    }
    

    private void SaveFailedLayout()
    {
        if (placesRectangles.Count == 0) return;
        var testName = TestContext.CurrentContext.Test.Name;
        var fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        var projectRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
            "..", "..", "..");
        var outputPath = Path.Combine(projectRootPath, "FailedTestsImages");
        Directory.CreateDirectory(outputPath);
        var fullPath = Path.Combine(outputPath, fileName);
        var dummyWordData = placesRectangles
            .Select(r => new WordData($"Word{r.Width}", new Font("Arial", 12), r.Size)) 
            .ToList();
        var cloudVisualizer = new CreateCloud(placesRectangles, defaultSettings, dummyWordData);
        cloudVisualizer.SaveImage(fullPath, ImageFormat.Png);
        
        TestContext.WriteLine($"Tag cloud visualization saved to file {fullPath}");
    }

    
    
    [Test]
    public void RectangleNotFit_Test()
    {
        var imageSize = new Size(100, 100);
        var appSettings = new AppSettings(
            imageSize: imageSize,
            maxFontSize: 10,
            padding: 0,
            spiralDensity: 0.01
        );
        var center = new Point(50, 50);
        var density = 5.0;
        var angleStep = 0.1;
        var generator = new SpiralPointGenerator(center, density, angleStep);
        var layouter = new CircularCloudLayouter(appSettings, generator);
        Assert.Throws<InvalidOperationException>(() =>
        {
            layouter.PutNextRectangle(new Size(200, 200));
        });
    }

    [Test]
public void RectanglesDensity_Test()
{
    const double requiredMinDensity = 0.5;
    
    var centerPoint = new Point(3000 / 2, 3000 / 2);
    var center = centerPoint;
    
    var settings = new AppSettings(padding: 1, 
        imageSize: new Size(3000, 3000), 
        spiralDensity: 0.1);
    
    var generator = new SpiralPointGenerator(centerPoint, settings.SpiralDensity, 1.0);
    var layouter = new CircularCloudLayouter(settings, generator);
    var random = new Random();
    var count = 100;
    var rectanglesArea = 0.0;
    
    for (int i = 0; i < count; i++)
    {
        var size = new Size(random.Next(20, 60), random.Next(20, 60));
        var rect = layouter.PutNextRectangle(size);
        placesRectangles.Add(rect);
        rectanglesArea += rect.Width * rect.Height;
    }
    
    double maxDistance = 0;
    foreach (var rect in layouter.PlacedRectangles)
    {
        maxDistance = Math.Max(maxDistance, DistanceToCenter(rect.Location, center));
        maxDistance = Math.Max(maxDistance, DistanceToCenter(new Point(rect.Right, rect.Top), center));
        maxDistance = Math.Max(maxDistance, DistanceToCenter(new Point(rect.Left, rect.Bottom), center));
        maxDistance = Math.Max(maxDistance, DistanceToCenter(new Point(rect.Right, rect.Bottom), center));
    }
    var circleArea = Math.PI * maxDistance * maxDistance;
    var densityFactor = rectanglesArea / circleArea;
    Assert.That(densityFactor, Is.GreaterThan(requiredMinDensity));
}
    
    private static double DistanceToCenter(Point p, Point center)
    {
        var dx = p.X - center.X;
        var dy = p.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
