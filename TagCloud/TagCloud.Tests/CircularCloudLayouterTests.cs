using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagCloud.Creators;
using TagCloud.Shapes;
using TagCloud.SpiralGenerators;
using TagCloud.Visualization;

namespace TagCloud.Tests;
[TestFixture]
public class CircularCloudLayouterTests
{
    private const int ImageWidth = 1000;
    private const int ImageHeight = 1000;
    private CircularCloudLayouter _layouter;
    private Point _center;
    private ISpiralPointGenerator _archimedeanGenerator;
    private ICloudShapeCreator _shapeCreator;
    private string _failDirectory;

    [SetUp]
    public void SetUp()
    {
        _center = new Point(100, 100);
        _archimedeanGenerator = new ArchimedeanSpiralPointGenerator(_center);
        _shapeCreator = new RectangleCloudShapeCreator();
        _layouter = new CircularCloudLayouter(_center, _archimedeanGenerator, _shapeCreator);
        
        var baseDirectory = AppContext.BaseDirectory; 
        var solutionRoot = FindSolutionRoot(baseDirectory);
        _failDirectory = Path.Combine(solutionRoot, "TagCloud.Visualization",
            "FailedClouds");
        Directory.CreateDirectory(_failDirectory);
    }
    
    private static string FindSolutionRoot(string start)
    {
        var directory = new DirectoryInfo(start);
        while (directory is not null && directory.GetFiles("TagCloud.sln").Length == 0)
            directory = directory.Parent;

        return directory?.FullName ?? throw new InvalidOperationException("Solution root not found");
    }

    [TestCase(-1, -10, 
        TestName = "Должен бросать исключение если ширина и высота отрицательные")]
    [TestCase(0, 10,   
        TestName = "Должен бросать исключение если ширина равна нулю")]
    [TestCase(10, 0,  
        TestName = "Должен бросать исключение если высота равна нулю")]
    [TestCase(0, 0,    
        TestName = "Должен бросать исключение если ширина и высота равны нулю")]
    public void PutNextShape_ShouldThrowException_SizeIsNonPositive(int width, int height)
    {
        var size = new Size(width, height);
        
        Action action = () => _layouter.PutNextShape(size);
        
        action
            .Should()
            .Throw<ArgumentException>();
    }

    [TestCase(10, 20, 
        TestName = "Первый прямоугольник 10x20 должен быть размещен строго в центре")]
    [TestCase(1, 1,   
        TestName = "Первый прямоугольник 1x1 должен быть размещен строго в центре")]
    [TestCase(50, 30, 
        TestName = "Первый прямоугольник 50x30 должен быть размещен строго в центре")]
    public void PutNextShape_ShouldPlaceInCenter_FirstRectangle(int width, int height)
    {
        var size = new Size(width, height);

        var shape = _layouter.PutNextShape(size);

        shape.Center
            .Should()
            .Be(_center);
    }

    [TestCase(30, 20, 100, 
        TestName = "Сто прямоугольников 30x20 не должны пересекаться")]
    [TestCase(10, 10, 200, 
        TestName = "Двести прямоугольников 10x10 не должны пересекаться")]
    [TestCase(50, 30, 50,  
        TestName = "Пятьдесят прямоугольников 50x30 не должны пересекаться")]
    public void Rectangles_ShouldNotIntersect_PlacingManySimilarRectangles(
        int width, 
        int height, 
        int rectanglesCount)
    {
        var size = new Size(width, height);
        
        for (var i = 0; i < rectanglesCount; i++)
            _layouter.PutNextShape(size);

        var shapes = _layouter.Shapes.ToArray();

        for (var i = 0; i < shapes.Length; i++)
        {
            for (var j = i + 1; j < shapes.Length; j++)
            {
                shapes[i]
                    .IntersectsWith(shapes[j])
                    .Should()
                    .BeFalse($"The shapes {i} and {j} do not intersect");
            }
        }
    }

    [TestCase(40, 20, 150, 1.5, 
        TestName = "Облако из 150 прямоугольников 40x20 должно иметь почти круглую форму")]
    [TestCase(60, 10, 200, 1.7,
        TestName = "Облако из 200 прямоугольников 60x10 должно иметь почти круглую форму")]
    public void Cloud_ShouldBeCircular_ByBoundingBoxRatio(int width,
        int height,
        int rectanglesCount,
        double maxRatio)
    {
        var size = new Size(width, height);

        for (var i = 0; i < rectanglesCount; i++)
            _layouter.PutNextShape(size);

        var rectangleShapes = _layouter.Shapes
            .Cast<RectangleCloudShape>()
            .ToArray();

        var minX = rectangleShapes.Min(s => s.BoundingBox.Left);
        var maxX = rectangleShapes.Max(s => s.BoundingBox.Right);
        var minY = rectangleShapes.Min(s => s.BoundingBox.Top);
        var maxY = rectangleShapes.Max(s => s.BoundingBox.Bottom);

        var boundingWidth = maxX - minX;
        var boundingHeight = maxY - minY;
        var ratio = 
            (double)Math.Max(boundingWidth, boundingHeight) / Math.Min(boundingWidth, boundingHeight);

        ratio
            .Should()
            .BeLessThanOrEqualTo(maxRatio,
                "The rectangle that bounds the cloud should not be too elongated");
    }

    [TestCase(30, 20, 50,
        TestName = "Последний из пятидесяти прямоугольников 30x20 нельзя сдвинуть ближе к центру на один пиксель")]
    [TestCase(50, 30, 30,
        TestName = "Последний из тридцати прямоугольников 50x30 нельзя сдвинуть ближе к центру на один пиксель")]
    public void Rectangle_ShouldNotBeShiftableToCenterByOnePixel_PlacingManySimilarRectangles(     
        int width,
        int height,
        int alreadyPlacedCount)
    {
        var size = new Size(width, height);
        for (var i = 0; i < alreadyPlacedCount; i++)
            _layouter.PutNextShape(size);
        
        var lastShape = _layouter.PutNextShape(size);
        var shapes = _layouter.Shapes.ToArray();

        var lastCenter = lastShape.Center;

        var dx = 0;
        if (lastCenter.X > _center.X) dx = -1;
        else if (lastCenter.X < _center.X) dx = 1;

        var dy = 0;
        if (lastCenter.Y > _center.Y) dy = -1;
        else if (lastCenter.Y < _center.Y) dy = 1;

        var canBeShifted = false;

        if (dx != 0 || dy != 0)
        {
            var shifted = lastShape.Shift(dx, dy);

            canBeShifted = !shapes
                .Where(s => s != lastShape)
                .Any(s => s.IntersectsWith(shifted));
        }

        canBeShifted
            .Should()
            .BeFalse("The algorithm should push the rectangle as close to the center as possible");
    }
    
    [Test]
    public void TearDown_ShouldSaveVisualization_WhenTestFails()
    {
        var expectedFile = Path.Combine(_failDirectory,
            $"{TestContext.CurrentContext.Test.Name}.png");
        if (File.Exists(expectedFile))
            File.Delete(expectedFile);//тут есть вариант не удалять, а например указывать время/дату в названии визуализации

        _layouter.PutNextShape(new Size(30, 20));
        
        true.Should().BeFalse("Force failure to check TearDown saving");
    }
    
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            return;

        var fileName = $"{TestContext.CurrentContext.Test.Name}.png";
        var path = Path.Combine(_failDirectory, fileName);

        CloudVisualizer.SaveCloudImage(_layouter, ImageWidth, ImageHeight, path);

        TestContext.Progress.WriteLine($"Tag cloud visualization saved to file {path}");
    }
}
