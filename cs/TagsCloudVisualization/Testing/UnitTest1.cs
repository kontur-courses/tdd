using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace Testing;

[TestFixture]
class RectanglesNetTest
{
    private RectanglesNet _net;

    [SetUp]
    public void SetUp()
    {
        _net = new RectanglesNet(Point.Empty);
    }

    [Test]
    public void Have_Zero_Rectangles_When_Created()
    {
        _net
            .Rectangles
            .Should()
            .HaveCount(0);
    }
    
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public void Have_Some_Rectangles_After_They_Are_Added(int count)
    {
        AddNRectangles(count);
        _net
            .Rectangles
            .Should()
            .HaveCount(count);
    }

    [TestCase(2)]
    [TestCase(3)]
    [TestCase(10)]
    [TestCase(100)]
    [TestCase(1000)]
    public void Rectangles_Should_Not_Intersect(int count)
    {
        AddNRectangles(count);
        _net
            .Rectangles
            .SelectMany(
                (x, i) => _net.Rectangles.Skip(i + 1), 
                (x, y) => Tuple.Create(x, y))
            .Any(t => t.Item1.IntersectsWith(t.Item2))
            .Should()
            .Be(false);
    }

    private void AddNRectangles(int n)
    {
        for (int i = 0; i < n; i++)
            _net.AddRectToNet(Size.Empty);
    }
}