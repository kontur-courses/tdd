using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests;

[TestFixture]
[TestOf(typeof(CircularCloudLayouterImpl))]
public class CircularCloudLayouterTests
{

    private static readonly string FailReportFolderPath = "./failed";
    private static readonly int MaxDistanceFromBarycenter = 20;
    
    private ICircularCloudLayouter _circularCloudLayouter;

    [SetUp]
    public void SetUp()
    {
        _circularCloudLayouter = new CircularCloudLayouterImpl(Point.Empty);
    }

    [OneTimeSetUp]
    public void EmptyFailReportFolder()
    {
        if (Directory.Exists(FailReportFolderPath))
            Directory.Delete(FailReportFolderPath, true);
    }

    [TearDown]
    public void ReportFailures()
    {
        var context = TestContext.CurrentContext;
        if (context.Result.Outcome.Status == TestStatus.Failed)
        {
            if (context.Test.MethodName == null)
            {
                Console.WriteLine("Teardown error: test method name is null");
                return;
            }

            var testType = DetermineTestType(context.Test.MethodName);
            if (testType == TestType.NoTearDown)
                return;
            
            var rectangles = _circularCloudLayouter.Layout.ToArray();
            
            var savingPath = $"{FailReportFolderPath}/{context.Test.Name}.png";
            Directory.CreateDirectory(FailReportFolderPath);

            var args = context.Test.Arguments;
            var center = new Point((int) args[0]!, (int) args[1]!);

#pragma warning disable CA1416
            
            new Bitmap(1000, 1000)
                .DrawFailedTestImage(
                    rectangles.ToArray(),
                    center,
                    ComputeBaryCenter(rectangles),
                    MaxDistanceFromBarycenter,
                    testType)
                .Save(savingPath, ImageFormat.Png);

#pragma warning restore CA1416
            
            Console.WriteLine($"Failure was reported to {Path.GetFullPath(savingPath)}");
        }
    }

    private static TestType DetermineTestType(string methodName)
    {
        if (methodName == nameof(PutNextRectangle_ThrowsOnHeightOrWidth_BeingLessOrEqualToZero))
            return TestType.NoTearDown;
        
        if (methodName == nameof(RectanglesCommonBarycenterIsCloseToTheProvidedCenter))
            return TestType.BarycenterTest;
        
        return TestType.OtherTest;
    }

    [Test]
    [Description("Проверяем, что поле CloudCenter бросает исключение, " +
                 "если процесс генерации облака уже начался")]
    public void CloudCenter_ThrowsIfLayoutContainsGeneratedRectangles()
    {
        _circularCloudLayouter.PutNextRectangle(new Size(1, 1));

        Action act = () => _circularCloudLayouter.CloudCenter = new Point(1, 1);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Can not change cloud center after generation start");
    }

    [Test]
    [Description("Проверяем, что метод PutNextRectangle бросает ArgumentException, " +
                 "если ему передан размер прямоугольника с высотой или шириной " +
                 "меньше либо равной нулю")]
    [TestCase(0, -4, 0, 4)]
    [TestCase(0, 1, -2, 4)]
    [TestCase(0, 0, 4, -2)]
    [TestCase(2, 3, 2, 0)]
    [TestCase(1, 2, -2, -1)]
    [TestCase(-1, 0, -1, 0)]
    public void PutNextRectangle_ThrowsOnHeightOrWidth_BeingLessOrEqualToZero(
        int centerX,
        int centerY,
        int width,
        int height)
    {
        Arrange(centerX, centerY);

        Action act = () => _circularCloudLayouter.PutNextRectangle(new Size(width, height));

        act.Should()
            .Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    [Description("Проверяем, что прямоугольники не пересекаются друг с другом")]
    [TestCaseSource(typeof(TestCaseData), nameof(TestCaseData.IntersectionTestSource))]
    public void RectanglesShouldNotIntersectEachOther(
        int centerX,
        int centerY,
        (int, int)[] sizes)
    {
        Arrange(centerX, centerY);

        var rectangles = GenerateTestLayout(sizes);

        Assert_RectanglesDoNotIntersect(rectangles);
    }

    [Test]
    [Description("Проверяем, что центр прямоугольника, размер которого был передан первым " +
                 "совпадает с центром, переданным в аргумент конструктора CircularCloudLayouter")]
    [TestCaseSource(typeof(TestCaseData), nameof(TestCaseData.FirstRectanglePositionTestSource))]
    public void FirstRectangleShouldBePositionedAtProvidedCenter(
        int centerX,
        int centerY,
        (int, int)[] sizes)
    {
        Arrange(centerX, centerY);

        var rectangles = GenerateTestLayout(sizes);
        
        Assert_FirstRectangleIsPositionedAtProvidedCenter(rectangles, new Point(centerX, centerY));
    }

    [Test]
    [Description("Проверяем, что прямоугольники расположены наиболее плотно, " +
                 "то есть максимум из попарных расстояний между центрами " +
                 "прямоугольников не превышает maxDistance")]
    [TestCaseSource(typeof(TestCaseData), nameof(TestCaseData.DensityTestSource))]
    public void RectanglesShouldBeCloseToEachOther(
        int centerX,
        int centerY,
        int maxDistance,
        (int, int)[] sizes)
    {
        Arrange(centerX, centerY);

        var rectangles = GenerateTestLayout(sizes);
        
        Assert_RectanglesArePositionedCloseToEachOther(rectangles, maxDistance);
    }

    [Test]
    [Description("Проверяем, что общий центр масс всех прямоугольников находится " +
                 "рядом с центром, переданным в конструктор CircularCloudLayouter")]
    [TestCaseSource(typeof(TestCaseData), nameof(TestCaseData.CenterTestSource))]
    public void RectanglesCommonBarycenterIsCloseToTheProvidedCenter(
        int centerX,
        int centerY,
        (int, int)[] sizes)
    {
        Arrange(centerX, centerY);

        var rectangles = GenerateTestLayout(sizes);
        
        Assert_RectanglesBarycenterIsCloseToCenter(rectangles, new Point(centerX, centerY));
    }
    
    private void Arrange(int centerX, int centerY)
    {
        _circularCloudLayouter.CloudCenter = new Point(centerX, centerY);
    }

    private IEnumerable<Rectangle> GenerateTestLayout((int, int)[] sizes)
    {
        return _circularCloudLayouter.GenerateLayout(
            sizes.Select(size => new Size(size.Item1, size.Item2)).ToArray());
    }

    private void Assert_RectanglesDoNotIntersect(IEnumerable<Rectangle> rectangles)
    {
        rectangles.CheckForAllPairs(pair => !pair.Item1.IntersectsWith(pair.Item2))
            .Should()
            .BeTrue();
    }

    private void Assert_FirstRectangleIsPositionedAtProvidedCenter(IEnumerable<Rectangle> rectangles, Point center)
    {
        rectangles
            .First()
            .RectangleCenter()
            .Should()
            .BeEquivalentTo(center);
    }

    private void Assert_RectanglesArePositionedCloseToEachOther(
        IEnumerable<Rectangle> rectangles,
        int maxDistance)
    {
        rectangles.CheckForAllPairs(pair => pair.Item1.DistanceToOtherIsNotGreaterThan(pair.Item2, maxDistance))
            .Should()
            .BeTrue();
    }

    private Point ComputeBaryCenter(IEnumerable<Rectangle> rectangles)
    {
        var (totalX, totalY, count) = rectangles
            .Aggregate((0, 0, 0), ((int totalX, int totalY, int count) res, Rectangle rect) =>
            {
                var rectCenter = rect.RectangleCenter();
                return (res.totalX + rectCenter.X, res.totalY + rectCenter.Y, ++res.count);
            });
        return new Point(totalX / count, totalY / count);
    }

    private void Assert_RectanglesBarycenterIsCloseToCenter(
        IEnumerable<Rectangle> rectangles,
        Point center)
    {
        var barycenter = ComputeBaryCenter(rectangles);
        var deviationFromCenter = barycenter.SquaredDistanceTo(center);
        deviationFromCenter.Should().BeLessOrEqualTo(MaxDistanceFromBarycenter * MaxDistanceFromBarycenter);
    }
}