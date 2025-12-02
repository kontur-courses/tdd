using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class Tests
{
    private Point _center;
    private Size _size;
    private IPointGenerator _pointGenerator;
    private Func<Point, float> GetDistance;
    private List<Rectangle> _layout;
    
    [SetUp]
    public void Setup()
    {
        _center = new Point(GenerationConstants.CenterX, GenerationConstants.CenterY);
        _size = new Size(GenerationConstants.DefaultRectWidth, GenerationConstants.DefaultRectHeight);
        var density = 1;
        _pointGenerator = new ArchimedeanSpiralPointGenerator(_center, density);
        GetDistance = p => (float)Math.Sqrt(Math.Pow(p.X - _center.X, 2) + Math.Pow(p.Y - _center.Y, 2));
    }
    
    [Test]
    public void CircularCloudLayout_ShouldCorrectlyPlaceFirstRectangle_OnCenterPoint()
    {
        var layouter = new CircularCloudLayouter(_center);
        var expected = new Rectangle(_center - _size / 2, _size);

        var result = layouter.PutNextRectangle(_size);
        _layout = layouter.GetLayout().ToList();
        
        expected.Should().BeEquivalentTo(result);
    }

    [Test]
    public void GetLayout_ShouldReturn_EquivalentRectangles_AsPutNextRectangle()
    {
        var layouter = new CircularCloudLayouter(_center);
        
        var firstRect = layouter.PutNextRectangle(_size);
        var secondRect = layouter.PutNextRectangle(_size);
        var thirdRect = layouter.PutNextRectangle(_size);
        _layout = layouter.GetLayout().ToList();
        var i = 0;

        foreach (var rect in layouter.GetLayout())
        {
            if (i == 0)
                firstRect.Should().BeEquivalentTo(rect);
            if (i == 1)
                secondRect.Should().BeEquivalentTo(rect);
            if (i == 2)
                thirdRect.Should().BeEquivalentTo(rect);
            ++i;
        }
    }

    [Test]
    public void CircularCloudLayout_ShouldPlace_SecondRectangle_FurtherFrom_CenterPoint()
    {
        var layouter = new CircularCloudLayouter(_center);
        
        var firstRect = layouter.PutNextRectangle(_size);
        var result = layouter.PutNextRectangle(_size);
        _layout = layouter.GetLayout().ToList();
        
        firstRect.IntersectsWith(result).Should().BeFalse();
        GetDistance(firstRect.GetCenter()).Should().BeLessThan(GetDistance(result.GetCenter()));
    }
    
    [Test]
    public void CircularCloudLayout_ShouldNotPlace_TwoRectangles_OnSamePosition()
    {
        var layouter = new CircularCloudLayouter(_center);
        
        var firstRect = layouter.PutNextRectangle(_size);
        var secondRect = layouter.PutNextRectangle(_size);
        _layout = layouter.GetLayout().ToList();
        
        secondRect.IntersectsWith(firstRect).Should().BeFalse();
    }

    [Test]
    public void ArchimedeanSpiralGenerator_ShouldStartFromCenter()
    {
        var firstPoint = _pointGenerator.GetNextPoint().First();
        _layout = new List<Rectangle> { new(firstPoint, _size) };
        firstPoint.Should().BeEquivalentTo(_center);
    }
    
    [Test]
    public void PressRectangleToCenter_ShouldMoveRectangle_AsCloseToTheCenterAsPossible()
    {
        var rect = new Rectangle(_pointGenerator.GetNextPoint().First(), _size);
        var layouter = new CircularCloudLayouter(_center);
        var expected = _center - _size / 2;
        
        var result = layouter.PressRectangleToCenter(rect);
        _layout = layouter.GetLayout().ToList();
        
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void CircularCloudLayouter_ShouldPlaceRectangles_AlmostInCircle()
    {
        var layouter = new CircularCloudLayouter(_center);
        var random = new Random();
        for (var i = 0; i < GenerationConstants.DefaultRectNumber; ++i)
            layouter.PutNextRectangle(new Size(
                random.Next(GenerationConstants.DefaultRectWidth, GenerationConstants.DefaultRectHeight),
                random.Next(GenerationConstants.DefaultRectWidth, GenerationConstants.DefaultRectHeight)));
        var lastRect = layouter.GetLayout().Last();
        var lastRectDist = GetDistance(lastRect.GetCenter());
        // Условно считаем, что последний виток начинается на 0.85 от расстояния последнего прямоугольника до центра
        // (для 100 прямоугольников размером 10х10, количество и размер сильно влияют)
        var outerLoopRatio = 0.85;
        var circlenessRatio = 0.85f;
        _layout = layouter.GetLayout().ToList();
        
        var outerLoop = layouter.GetLayout()
            .Where(x => GetDistance(x.GetCenter()) >= lastRectDist * outerLoopRatio);
        var maxDist = GetDistance(outerLoop.MaxBy(x => GetDistance(x.GetCenter())).GetCenter());
        var minDist = GetDistance(outerLoop.MinBy(x => GetDistance(x.GetCenter())).GetCenter());
        var distRatio = minDist / maxDist;
        
        distRatio.Should().BeGreaterThan(circlenessRatio).And.BeLessThan(1);
    }

    [Test]
    public void CircularCloudLayouter_ShouldPlaceRectangle_AsTightlyAsPossible()
    {
        var layouter = new CircularCloudLayouter(_center);
        var random = new Random();
        for (var i = 0; i < GenerationConstants.DefaultRectNumber; ++i)
            layouter.PutNextRectangle(new Size(
                random.Next(GenerationConstants.DefaultRectWidth, GenerationConstants.DefaultRectHeight),
                random.Next(GenerationConstants.DefaultRectWidth, GenerationConstants.DefaultRectHeight)));
        var lastRect = layouter.GetLayout().Last();
        var radius = GetDistance(lastRect.GetCenter());
        var potentialCircleArea = Math.PI * radius * radius;
        var tightRatio = 0.7f;
        _layout = layouter.GetLayout().ToList();

        float rectanglesArea = layouter.GetLayout()
            .Sum(x => x.Size.Width * x.Size.Height);
        var areaRatio = rectanglesArea / potentialCircleArea;
        
        areaRatio.Should().BeGreaterThan(tightRatio).And.BeLessThan(1);
        
        Console.WriteLine($"Текущая плотность: {areaRatio}"); // Просто приятный бонус
    }

    // Этот тест перестал работать из-за уменьшения шага в генераторе спирали, так что теперь точки ближе => прямоугольники
    // не влезают так, как раньше, оставляю для демонстрации отрисовки раскладки на упавших тестах
    [Test]
    public void ArchimedeanSpiralGenerator_ShouldGeneratePoints_ByArchimedeanSpiral()
    {
        // Для наглядности пропустил побольше значений, чтобы было более наглядно видно на .bmp упавшего теста
        var points = _pointGenerator.GetNextPoint().Skip(10000).Take(100).ToList();
        _layout = new List<Rectangle>();
        foreach (var point in points)
            _layout.Add(new Rectangle(point, new Size(5, 5)));
        
        for (var i = 1; i < points.Count; ++i)
            GetDistance(points[i]).Should().BeGreaterThan(GetDistance(points[i - 1]));
    }

    [TearDown]
    public void TearDown()
    {
        var testOutcome = TestContext.CurrentContext.Result.Outcome.Status;
        if (testOutcome == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            var layout = _layout;
            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{TestContext.CurrentContext.Test.Name}_failed.bmp");
            TagsCloudVisualizationApp.CloudVisualizer.DrawRectangles(layout, _center, path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}