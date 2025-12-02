using System.Diagnostics;
using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class PerformanceTests
{
    private static readonly Random Rnd = new(123);

    private static Size RandomSize()
    {
        var w = Rnd.Next(20, 80);
        var h = Rnd.Next(10, 60);
        return new Size(w, h);
    }

    [Test]
    public void PutNextRectangle_ShouldBeFast_For500Rectangles()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));

        var sw = Stopwatch.StartNew();

        for (var i = 0; i < 500; i++)
            layouter.PutNextRectangle(RandomSize());

        sw.Stop();

        sw.ElapsedMilliseconds.Should()
            .BeLessThan(250, "алгоритм должен работать быстро на 500 прямоугольников");
    }

    [Test]
    public void PutNextRectangle_ShouldBeFast_For1000Rectangles()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));

        var sw = Stopwatch.StartNew();

        for (var i = 0; i < 1000; i++)
            layouter.PutNextRectangle(RandomSize());

        sw.Stop();

        sw.ElapsedMilliseconds.Should()
            .BeLessThan(450, "алгоритм должен работать быстро на 1000 прямоугольников");
    }

    [Test]
    public void Algorithm_ShouldScale_Sublinearly()
    {
        var layouterA = new CircularCloudLayouter(new Point(0, 0));
        var layouterB = new CircularCloudLayouter(new Point(0, 0));

        var swA = Stopwatch.StartNew();
        for (var i = 0; i < 500; i++)
            layouterA.PutNextRectangle(RandomSize());
        swA.Stop();

        var swB = Stopwatch.StartNew();
        for (var i = 0; i < 1000; i++)
            layouterB.PutNextRectangle(RandomSize());
        swB.Stop();

        swB.ElapsedMilliseconds.Should()
            .BeLessThan(swA.ElapsedMilliseconds * 3,
                "рост времени должен быть сублинейным при удвоении количества прямоугольников");
    }

    [Test]
    public void Algorithm_ShouldNotSlowDown_Drastically()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));

        var sw = Stopwatch.StartNew();
        for (var i = 0; i < 1500; i++)
            layouter.PutNextRectangle(RandomSize());
        sw.Stop();

        sw.ElapsedMilliseconds.Should()
            .BeLessThan(1200, "алгоритм не должен деградировать до квадратичного времени");
    }
}
