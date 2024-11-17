using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace CircularCloudLayouterTests;

[TestFixture]
public class CircularCloudLayouterTests
{
	private const int imageWidth = 800;
	private const int imageHeights = 800;
	private readonly int imageWidthHalf = imageWidth / 2;
	private readonly int imageHeightHalf = imageHeights / 2;
	private static ICircularCloudLayouter layouter;
	private static List<Rectangle> rectangles;

	[SetUp]
	public void SetUp()
	{
		layouter = new CircularCloudLayouter(new Point(imageWidthHalf, imageHeightHalf));
	}

	[TearDown]
	public void TearDown()
	{
		if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
		{
			var dirPath = $"{TestContext.CurrentContext.WorkDirectory}\\FailedTest";
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			CloudVisualiser.DrawAndSaveCloud(rectangles, dirPath, TestContext.CurrentContext.Test.Name, imageWidth, imageHeights, Color.Bisque);
			Console.WriteLine($"Tag cloud visualization saved to file {dirPath}\\{TestContext.CurrentContext.Test.Name}.png");
		}
	}

	[TestCase(15, 10, 50)]
	[TestCase(30, 10, 50)]
	[TestCase(50, 10, 50)]
	public void PutNextRectangle_ShouldNotIntersect(int count, int minWidth, int maxWidth)
	{
		rectangles = CloudGenerator.GenerateRandomCloud(layouter, count, minWidth, maxWidth);
		for (var i = 0; i < rectangles.Count; i++)
			rectangles.Take(i).IsIntersecting(rectangles[i]).Should().BeFalse();
	}

	[TestCase(15, 10, 50)]
	[TestCase(30, 10, 50)]
	[TestCase(50, 10, 50)]
	public void PutNextRectangle_CloudIsCloseToCircle(int count, int minWidth, int maxWidth, double tolerance = 0.1)
	{
		rectangles = CloudGenerator.GenerateRandomCloud(layouter, count, minWidth, maxWidth);

		var boundingBoxSize = rectangles.GetBoundaryBox();

		var aspectRatio = (double)boundingBoxSize.Width / boundingBoxSize.Height;

		Math.Abs(aspectRatio - 1).Should().BeLessOrEqualTo(tolerance);
	}

	[TestCase(50, 10, 50)]
	[TestCase(100, 10, 50)]
	[TestCase(200, 10, 50)]
	public void PutNextRectangle_DensityShouldBeMaximised(int count, int minWidth, int maxWidth)
	{
		rectangles = CloudGenerator.GenerateRandomCloud(layouter, count, minWidth, maxWidth);

		var totalRectangleArea = rectangles.Sum(rect => rect.Width * rect.Height);
		var center = new Point(imageWidthHalf, imageWidthHalf);
		var maxDistance = 0.0;
		foreach (var rect in rectangles)
		{
			var corners = new List<Point>
			{
				new(rect.Left, rect.Top),
				new(rect.Right, rect.Top),
				new(rect.Left, rect.Bottom),
				new(rect.Right, rect.Bottom)
			};

			foreach (var corner in corners)
			{
				var distance = Math.Sqrt(Math.Pow(center.X - corner.X, 2) + Math.Pow(center.Y - corner.Y, 2));
				if (distance > maxDistance)
					maxDistance = distance;
			}
		}

		var circleArea = Math.PI * maxDistance * maxDistance;
		var density = totalRectangleArea / circleArea;

		density.Should().BeGreaterOrEqualTo(0.6);
	}
}