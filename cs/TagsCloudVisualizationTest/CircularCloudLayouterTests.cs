using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Renderers;

namespace TagsCloudVisualizationTest;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter layout;

    private List<Rectangle> rectangles;

    [SetUp]
    public void SetUp()
    {
        layout = new CircularCloudLayouter();
        rectangles = [];
    }

    [Test]
    public void PutNextRectangle_ReturnRectangleEmpty_OnSizeEmpty()
    {
        layout.PutNextRectangle(Size.Empty).Should().BeEquivalentTo(Rectangle.Empty);
    }
    
    [TestCase(100,100)]
    [TestCase(400,100)]
    [TestCase(100,400)]
    [TestCase(0,0)]
    public void PutNextRectangle_ReturnRectangleCentered_WhenLayoutHaveCenter(int x, int y)
    {
        var center = new Point(x, y);
        layout = new CircularCloudLayouter(center);
        
        var expected = new Rectangle(x,y,0,0);
        
        layout.PutNextRectangle(Size.Empty).Should().BeEquivalentTo(expected);
    }

    [TestCase(0, 0)]
    [TestCase(5, 5)]
    [TestCase(0, 5)]
    public void PutNextRectangle_ReturnLocationZero_OnFirstInput(int height, int width)
    {
        var size = new Size(width, height);
        var expected = new Rectangle(0, 0, width, height);
        var result = layout.PutNextRectangle(size);

        rectangles.Add(result);

        result.Should().BeEquivalentTo(expected);
    }

    [TestCase(-1, 0)]
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    [TestCase(0, -1)]
    public void PutNextRectangle_ThrowArgumentException_WhenSizeIsNegative(int height, int width)
    {
        var size = new Size(width, height);

        var act = () => layout.PutNextRectangle(size);

        act.Should().Throw<ArgumentOutOfRangeException>($"{size} меньше 0");
    }

    [Test]
    public void IntersectsWith_BeFalse()
    {
        layout = LayoutRegistry.GetRandomFilledCloud();
        rectangles = LayoutRegistry.RandomFilledCloudRectangles;

        var isIntersect = rectangles
            .SelectMany((rect, i) => rectangles.Skip(i + 1).Select(r => (rect, r)))
            .Any(pair => pair.rect.IntersectsWith(pair.r));

        isIntersect.Should().BeFalse();
    }

    [Test]
    [Repeat(5)]
    public void Layout_BeCircle_WhenDeviationIsLessThanTolerance()
    {
        layout = LayoutRegistry.GetRandomFilledCloud();
        rectangles = LayoutRegistry.RandomFilledCloudRectangles;
        
        const double tolerance = 0.15;
        
        var totalArea = rectangles
            .Sum(rect => rect.Width * rect.Height);

        var weightedXSum = rectangles
            .Sum(rect => 
                (rect.X + rect.Width / 2.0) * rect.Width * rect.Height);

        var weightedYSum = rectangles
            .Sum(rect => 
                (rect.Y + rect.Height / 2.0) * rect.Width * rect.Height);

        var maxRadius = rectangles.Max(GetDistanceToCenter);

        var centerXOfMass = weightedXSum / totalArea;
        var centerYOfMass = weightedYSum / totalArea;
        var deviationLenght = Math.Sqrt(Math.Pow(centerXOfMass, 2) + Math.Pow(centerYOfMass, 2));

        var maxDeviation = maxRadius * tolerance;

        deviationLenght.Should().BeLessThan(maxDeviation, "Прямоугольники слишком сильно отклонены от центра");
    }

    [Test]
    [Repeat(5)]
    public void Layout_BeDense()
    {
        layout = LayoutRegistry.GetRandomFilledCloud();
        rectangles = LayoutRegistry.RandomFilledCloudRectangles;

        //Процент самых дальних квадратов
        const double outliersPercent = 0.02;
        var outliersCount = (int)(rectangles.Count * outliersPercent);

        var totalArea = rectangles
            .Select(x => x.Height * x.Width)
            .Sum();

        var maxRadius = rectangles
            .Select(GetDistanceToCenter)
            .OrderDescending()
            .Skip(outliersCount)
            .First();

        var circleArea = Math.PI * Math.Pow(maxRadius, 2);

        var dense = totalArea / circleArea;

        dense.Should().BeGreaterThan(0.5, "Плотность слишком низкая");
    }

    [Test]
    [Repeat(5)]
    public void Layout_HasUniformDensityInFourDirections()
    {
        layout = LayoutRegistry.GetRandomFilledCloud();
        rectangles = LayoutRegistry.RandomFilledCloudRectangles;

        const int centerX = 0;
        const int centerY = 0;
        //Минимальный % от полной площади, который должен быть в "направлении"
        const double targetPercent = 0.15;

        var quadrants = new Dictionary<string, double>
        {
            { "TopRight", 0 },
            { "TopLeft", 0 },
            { "BottomRight", 0 },
            { "BottomLeft", 0 }
        };

        foreach (var rect in rectangles)
        {
            var rectCenterX = rect.X + rect.Width / 2.0;
            var rectCenterY = rect.Y + rect.Height / 2.0;
            var area = rect.Width * rect.Height;

            switch (rectCenterX)
            {
                case >= centerX when rectCenterY >= centerY:
                    quadrants["TopRight"] += area;
                    break;
                case < centerX when rectCenterY >= centerY:
                    quadrants["TopLeft"] += area;
                    break;
                case >= centerX when rectCenterY < centerY:
                    quadrants["BottomRight"] += area;
                    break;
                default:
                    quadrants["BottomLeft"] += area;
                    break;
            }
        }

        var totalArea = quadrants.Values.Sum();
        var targetArea = totalArea * targetPercent;

        quadrants
            .Should()
            .AllSatisfy(distance => distance.Value
                .Should()
                .BeGreaterThan(targetArea, $"Плотность {distance.Key} ниже среднего"));
    }

    [Test]
    [Repeat(5)]
    public void Outliers_NotBeFarAway()
    {
        layout = LayoutRegistry.GetRandomFilledCloud();
        rectangles = LayoutRegistry.RandomFilledCloudRectangles;

        //Процент самых дальних квадратов
        const double outliersPercent = 0.02;
        const double percentile = 0.95;
        const double tolerance = 0.1;
        var outliersCount = (int)(rectangles.Count * outliersPercent);

        var sortedDistance = rectangles
            .Select(GetDistanceToCenter)
            .OrderDescending()
            .ToArray();
        
        var percentileValue = sortedDistance
            .Skip((int)(sortedDistance.Length * (1 - percentile)))
            .First();
       
        //умножаем на сколько процентов оно может быть дальше
        var maxAllowedDistance = percentileValue * (1 + tolerance);

        sortedDistance
            .Take(outliersCount)
            .Should()
            .AllSatisfy(distance => distance
                .Should()
                .BeLessThan(maxAllowedDistance));
    }

    private static double GetDistanceToCenter(Rectangle x)
    {
        return Math.Sqrt(Math.Pow(x.X + x.Width / 2.0, 2) + Math.Pow(x.Y + x.Height / 2.0, 2));
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.FailCount <= 0) return;

        var render = new ContentFittingRenderer();
        var name = $"{TestContext.CurrentContext.Test.Name}.png";
        
        rectangles.ForEach(render.AddRectangle);

        render.SaveImage(name);
        Console.WriteLine($"Tag cloud visualization saved to file {name}");
    }
}