using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using TagCloud;

namespace TagCloudTests;

public class CircularCloudLayouterTests
{
    private Point center = new Point(500, 500);
    private readonly Random random = new Random();
    private CircularCloudLayouter layouter;

    [SetUp]
    public void Setup()
    {
        layouter = new CircularCloudLayouter(new Point(500, 500))
        {
            Density = 0.1,
            AngleStep = 0.1
        };
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            var bitmap = new Bitmap(center.X * 2, center.Y * 2);
            var g = Graphics.FromImage(bitmap);
            layouter.Rectangles.ForEach(r
               => g.DrawRectangle(new Pen(new SolidBrush(Color.Orange), 2) { Alignment = PenAlignment.Inset }, r));

            var testName = TestContext.CurrentContext.Test.Name;
            var debugPath = TestContext.CurrentContext.TestDirectory;

            var path = $@"{debugPath}/{testName}_Failed.jpg";
            bitmap.Save(path);

            TestContext.Error.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }

    [TestCase(0, 0)]
    [TestCase(1, 0)]
    [TestCase(0, 1)]
    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    [TestCase(-1, -1)]
    public void PutNextRectangle_ThrowArgumentException_OnNonPozitiveSize(int width, int height)
    {
        Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));
    }

    [Test]
    public void PutNextRectangle_NotThrowArgumentException_OnPozitiveSize()
    {
        Action act = () => layouter.PutNextRectangle(new Size(1, 1));
        act.Should().NotThrow<ArgumentException>();
    }

    [Test]
    [Repeat(10)]
    public void PutNextRectangle_AddOne()
    {
        layouter.PutNextRectangle(new Size(random.Next(10, 100), random.Next(10, 100)));
        layouter.Rectangles.Count.Should().Be(1);
    }

    [Test]
    [Repeat(10)]
    public void PutNextRectangle_FirstInCenter()
    {
        var size = new Size(random.Next(10, 100), random.Next(10, 100));
        layouter.PutNextRectangle(size);
        layouter.Rectangles.First().Location.Should().Be(new Point(center.X - size.Width / 2, center.Y - size.Height / 2));
    }

    [Test]
    [Repeat(10)]
    public void PutNextRectangle_AddedWithCorrectSize()
    {
        var size = new Size(random.Next(10, 100), random.Next(10, 100));
        layouter.PutNextRectangle(size);
        layouter.Rectangles.First().Size.Should().Be(size);
    }

    private static List<TestCaseData> GetOverlapTests()
    {
        var cases = new List<TestCaseData>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                var name = "HasOverlapWith: " + (x < 0 ? "Left " : (x > 0) ? "Right " : "Center ")
                         + (y < 0 ? "Top " : (y > 0) ? "Bottom " : "Center ");
                const int sideLength = 6;
                cases.Add(new TestCaseData(sideLength * x - 2, sideLength * y - 2, sideLength, sideLength).Returns(true).SetName(name + "overlap"));

                if(x != 0 && y != 0)
                    cases.Add(new TestCaseData((sideLength + 20) * x, (sideLength + 20) * y, sideLength, sideLength).Returns(false).SetName(name + "not overlap"));
            }
        }

        cases.Add(new TestCaseData(-10, -10, 20, 40).Returns(true).SetName("HasOverlapWith: Area inside targer area"));
        cases.Add(new TestCaseData(-6, -6, 12, 12).Returns(true).SetName("HasOverlapWith: The same area"));
        return cases;
    }

    [TestCaseSource(nameof(GetOverlapTests))]
    public bool HasOverlapWith_CorrectAnswer_OnOverlap(int xOffset, int yOffset, int width, int height)
    {
        var staticSize = new Size(12, 12);
        layouter.PutNextRectangle(staticSize);
        var rectangle = new Rectangle(new Point(center.X + xOffset, center.Y + yOffset), new Size(width, height));
        return layouter.HasOverlapWith(rectangle);
    }

    [TestCase(10)]
    [TestCase(100)]
    [Repeat(50)]
    public void PutNextRectangle_Error_OnOverlap(int wordCount)
    {
        for (int i = 0; i < wordCount; i++)
            layouter.PutNextRectangle(new Size(50 + random.Next(-20, 70), 30 + random.Next(-20, 20)));

        var rectengles = layouter.Rectangles;
        for (int i = 0; i < rectengles.Count - 1; i++)
        {
            var rect = rectengles[i];
            for (int j = i + 1; j < rectengles.Count; j++)
            {
                var otherRect = rectengles[j];
                if (otherRect.IntersectsWith(rect))
                    throw new Exception("Rectangles overlap!");
            } 
        }
    }

    [Test]
    public void PutNextRectangle_IsСloudDensely()
    {
        const double accuracy = 0.2;
        layouter.Density = 0.01;

        // The more rectangles, the more accurately repeats the shape of a circle
        for (int i = 0; i < 200; i++)
            layouter.PutNextRectangle(new Size(50, 20));

        var rects = layouter.Rectangles;
        var rectangleArea = rects.Select(x => x.Width * x.Height).Sum();
        var horizontalRadius = Math.Abs(rects.Max(x => x.Right) - rects.Min(x => x.Left)) / 2 ;
        var verticalRadius = Math.Abs(rects.Max(x => x.Bottom) - rects.Min(x => x.Top)) / 2;
        var boundingElipseArea = Math.PI * horizontalRadius * verticalRadius;
        (Math.Abs(rectangleArea / boundingElipseArea - 1) < accuracy).Should().BeTrue();
    }


    [Test]
    public void PutNextRectangle_DefaultСloudIsCircle()
    {
        for (int i = 0; i < 13; i++)
            layouter.PutNextRectangle(new Size(50, 50));

        var rects = layouter.Rectangles;
        var horizontalRadius = Math.Abs(rects.Max(x => x.Right) - rects.Min(x => x.Left)) / 2;
        var verticalRadius = Math.Abs(rects.Max(x => x.Bottom) - rects.Min(x => x.Top)) / 2;
        var centers = rects.Select(r => new Point(r.X + r.Width / 2, r.Y + r.Height / 2));

        var distance = (Point p) => Math.Sqrt(Math.Pow(p.X - layouter.Crenter.X, 2) + Math.Pow(p.Y - layouter.Crenter.Y, 2));
        centers.All(center => distance(center) < Math.Max(horizontalRadius, verticalRadius)).Should().BeTrue();
    }
}