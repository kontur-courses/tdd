using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class CircularCloudLayouterShould
	{
		[Test, Order(1)]
		public void CreateAndSaveFirstRectangle()
		{
			var cloud = CreateCloud(0, 0);
			cloud.PutNextRectangle(GetRandomSize());
			cloud.Rectangles.Should().NotBeNullOrEmpty();
		}

		[Test, Order(2)]
		public void PutSecondRectangleWithoutIntersection()
		{
			var cloud = CreateCloud(0, 0);
			cloud.PutNextRectangle(GetRandomSize());
			cloud.PutNextRectangle(GetRandomSize());
			var intersection = AreRectanglesIntersect(cloud.Rectangles);
			intersection.Should().BeFalse();
		}

		[Test, Order(3)]
		public void PutManyRectanglesWithoutIntersection()
		{
			var cloud = CreateCloud(0, 0);
			for (var i = 0; i < 100; i++)
				cloud.PutNextRectangle(GetRandomSize());
			var intersection = AreRectanglesIntersect(cloud.Rectangles);
			intersection.Should().BeFalse();
		}

		[Test, Order(4)]
		public void HaveCircleShape()
		{
			var cloud = CreateCloud(0, 0);
			for (var i = 0; i < 100; i++)
				cloud.PutNextRectangle(GetRandomSize());
			var isCircleShape = IsCircleShape(cloud);
			isCircleShape.Should().BeTrue();
		}

		[Test]
		[Order(5)]
		public void HaveCircleShapeWithNonZeroCenter()
		{
			var cloud = CreateCloud(200, -600);
			for (var i = 0; i < 100; i++)
				cloud.PutNextRectangle(GetRandomSize());
			var isCircleShape = IsCircleShape(cloud);
			isCircleShape.Should().BeTrue();
		}

		[Test]
		[Order(6)]
		public void BeDenselyFilledByRectangles()
		{
			var cloud = CreateCloud(0, 0);
			for (var i = 0; i < 100; i++)
				cloud.PutNextRectangle(GetRandomSize());
			var sumOfRectSquare = cloud.Rectangles.Select(x => x.Height * x.Width).Sum();
			var averageRadius = GetAverageRadiusOfCircle(cloud);
			var square = Math.PI * averageRadius * averageRadius;
			var density = sumOfRectSquare / square * 100;
			density.Should().BeInRange(60, 100);
		}
		
		[TestCase(10, 0, TestName = "Height are zero")]
		[TestCase(0, 10, TestName = "Width are zero")]
		[TestCase(0, 0, TestName = "Height and width are zero")]
		public void ThrowExceptionWhenZeroSize(int width, int height)
		{
			var cloud = CreateCloud(0, 0);
			Action act = () => cloud.PutNextRectangle(new Size(width, height));
			act.Should().Throw<ArgumentException>();
		}

		[TestCase(-10, 10, TestName = "Height are negative")]
		[TestCase(10, -10, TestName = "Width are negative")]
		[TestCase(-10, -10, TestName = "Height and width are negative")]
		public void ThrowExceptionWhenNegativeSize(int width, int height)
		{
			var cloud = CreateCloud(0, 0);
			Action act = () => cloud.PutNextRectangle(new Size(width, height));
			act.Should().Throw<ArgumentException>();
		}

		[TearDown]
		public void TearDown()
		{
			if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
			{
				var name = TestContext.CurrentContext.Test.FullName;
				var path = Path.Combine(Environment.CurrentDirectory, name);
				var graphics = new Graphics();
				graphics.SaveMap(cloudLayouter.Rectangles, path);
				TestContext.Error.Write($@"Tag cloud visualization saved to file <{path}>");
			}
		}

		private CircularCloudLayouter cloudLayouter;
		public Random Random = new Random();

		private CircularCloudLayouter CreateCloud(int x, int y)
		{
			var center = new Point(x, y);
			var cloud = new CircularCloudLayouter(center);
			cloudLayouter = cloud;
			return cloud;
		}

		private Size GetRandomSize()
		{
			return new Size(Random.Next(10, 50), Random.Next(10, 50));
		}

		private bool AreRectanglesIntersect(IReadOnlyList<Rectangle> allRectangles)
		{
			for (var i = 0; i < allRectangles.Count; i++)
			{
				for (var j = i + 1; j < allRectangles.Count; j++)
				{
					if (allRectangles[i].IntersectsWith(allRectangles[j]))
						return true;
				}
			}
			return false;
		}

		private bool IsCircleShape(CircularCloudLayouter cloud)
		{
			var pointsOfBound = new List<Point>();
			var maxRadius = GetMaxRadius(cloud);
			for (double angle = 0; angle <= 2 * Math.PI; angle += 0.01)
			{
				for (var currentRadius = maxRadius; currentRadius > 0; currentRadius--)
				{
					var x = (int)(currentRadius * Math.Cos(angle)) + cloud.center.X;
					var y = (int)(currentRadius * Math.Sin(angle)) + cloud.center.Y;
					var point = new Point(x, y);
					if (PointIntersectWithAnyRectangles(cloud.Rectangles, point))
					{
						pointsOfBound.Add(point);
						break;
					}
				}
			}
			var dispersion = GetDispersion(pointsOfBound, cloud.center);
			return dispersion < 20;
		}

		private bool PointIntersectWithAnyRectangles(IReadOnlyList<Rectangle> rectangles, Point point)
		{
			foreach (var rectangle in rectangles)
				if (rectangle.Contains(point))
					return true;
			return false;
		}

		private double GetDispersion(List<Point> pointOfIntersection, Point center)
		{
			var lengthsOfPoints = GetLenghtsOfPoints(pointOfIntersection, center);
			var averageLength = lengthsOfPoints.Sum() / lengthsOfPoints.Count;
			var sumOfSquareDifference = 0.0;
			foreach (var length in lengthsOfPoints)
				sumOfSquareDifference += (length - averageLength) * (length - averageLength);
			var dispersion = Math.Sqrt(sumOfSquareDifference / lengthsOfPoints.Count);
			return dispersion;
		}

		private List<double> GetLenghtsOfPoints(List<Point> pointOfIntersection, Point center)
		{
			var lengthsOfPoints = new List<double>();
			foreach (var point in pointOfIntersection)
			{
				var lengths = Math.Sqrt((point.X - center.X) * (point.X - center.X) + (point.Y - center.Y) * (point.Y - center.Y));
				lengthsOfPoints.Add(lengths);
			}
			return lengthsOfPoints;
		}

		private static int GetMaxRadius(CircularCloudLayouter cloud)
		{
			var boundingCoordinate = new BoundingCoordinate(cloud.Rectangles);
			var radius = new int[4];
			radius[0] = boundingCoordinate.MaxX - cloud.center.X;
			radius[1] = boundingCoordinate.MaxY - cloud.center.Y;
			radius[2] = cloud.center.X - boundingCoordinate.MinX;
			radius[3] = cloud.center.Y - boundingCoordinate.MinY;
			return radius.Max() + 100;
		}

		private int GetAverageRadiusOfCircle(CircularCloudLayouter cloud)
		{
			var boundingCoordinate = new BoundingCoordinate(cloud.Rectangles);
			var radiusX = (boundingCoordinate.MaxX - boundingCoordinate.MinX) / 2;
			var radiusY = (boundingCoordinate.MaxY - boundingCoordinate.MinY) / 2;
			var averageRadius = (radiusY + radiusX) / 2;
			return averageRadius;
		}
	}
}