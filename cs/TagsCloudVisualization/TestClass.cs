using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class CircularCloudLayouterShould
	{
		public static Random Random = new Random();

		private List<Rectangle> AllRectangles;

		[SetUp]
		public void SetUp()
		{
			AllRectangles = new List<Rectangle>();
		}

		[TearDown]
		public void TearDown()
		{
			var graphics = new Graphics();
			graphics.SaveMap(AllRectangles, TestContext.CurrentContext.Test.FullName);
		}

		private void PutRandomRectangle(CircularCloudLayouter cloud)
		{
			var size = new Size(Random.Next(50, 100), Random.Next(50, 100));
			var rectangle = cloud.PutNextRectangle(size);
			AllRectangles.Add(rectangle);
		}

		private bool IsIntersects()
		{
			for (var i = 0; i < AllRectangles.Count; i++)
			{
				for (var j = i + 1; j < AllRectangles.Count; j++)
				{
					if (AllRectangles[i].IntersectsWith(AllRectangles[j]))
						return true;
				}
			}
			return false;
		}

		[Test]
		[Order(1)]
		public void CreateCenterOfCloud()
		{
			var center = new Point(0, 0);
			Action act = () => new CircularCloudLayouter(center);
			act.Should().NotThrow<NotImplementedException>();
		}

		[Test]
		[Order(2)]
		public void PutFirstRectangle()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			PutRandomRectangle(cloud);
			AllRectangles.Should().NotBeNullOrEmpty();
		}

		[Test]
		[Order(3)]
		public void CreateFirstLayerWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 4; i++)
				PutRandomRectangle(cloud);
			IsIntersects().Should().BeFalse();
		}

		[Test]
		[Order(4)]
		public void CorrectWorkWithDifferentCenter()
		{
			var center = new Point(-10, -10);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 4; i++)
				PutRandomRectangle(cloud);
			IsIntersects().Should().BeFalse();
		}

		[Test]
		[Order(5)]
		public void StartSecondLayerWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 5; i++)
				PutRandomRectangle(cloud);
			IsIntersects().Should().BeFalse();
		}

		[Test]
		[Order(6)]
		public void PutALotOfRectanglesWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 150; i++)
				PutRandomRectangle(cloud);
			IsIntersects().Should().BeFalse();
		}
	}
}