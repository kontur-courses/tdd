using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagsCloudVisualization.Task_2;


namespace TagsCloudVisualization;


[TestFixture]
public class CircularCloudLayouterTests
{
    private readonly List<Rectangle> listRectangles = [];

    [Test]
    public void CircularCloudLayouter_CorrectInitialization_NoExceptions()
    {
        var createAConstructor = () => new CircularCloudLayouter(new Point(960, 540));

        createAConstructor
            .Should()
            .NotThrow();
    }

    [Test]
    public void PutNextRectangle_RandomSizes_MustBeRightSize()
    {
        var cloud = new CircularCloudLayouter(new Point(960, 540));
        var random = new Random();

        for (var i = 0; i < 50; i++)
        {
            var width = random.Next(30, 200);
            var actualSize = new Size(width, random.Next(width / 6, width / 3));

            var rectangle = cloud.PutNextRectangle(actualSize);

            actualSize
                .Should()
                .Be(rectangle.Size);
        }
    }

    [Test]
    public void PutNextRectangle_RandomSizes_ShouldNotIntersect()
    {
        var cloudLayouter = new CircularCloudLayouter(new Point(960, 540));
        var random = new Random();
        
        for (int i = 0; i < 100; i++)
        {
            var width = random.Next(30, 200);

            var rectangle = cloudLayouter.PutNextRectangle(new(width, random.Next(width / 6, width / 3)));

            listRectangles.Any(rect => rect.IntersectsWith(rectangle))
                .Should()
                .BeFalse("Прямоугольники не должны пересекаться");

            listRectangles.Add(rectangle);
        }
    }

    [TearDown]
    public void CreateReportInCaseOfAnError()
    {
        if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
        {
            var colors = new[] { Color.Red, Color.Green, Color.Brown, Color.Yellow, Color.Blue };
            var path = "../../../../TagsCloudVisualization/TestErrorReports/сloud.png";
            var visual = new VisualizationCloudLayout(1920, 1080, Color.White, colors);

            visual.CreateImage(listRectangles)
                .Save(path);

            Console.WriteLine($"Tag cloud visualization saved to file {Path.GetFullPath(path)}");
        }

        listRectangles.Clear();
    }
}
