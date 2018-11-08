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
			cloud.AllRectangles.Should().NotBeNullOrEmpty();
		}

		[Test]
		[Order(3)]
		public void CreateFirstLayerWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 4; i++)
				PutRandomRectangle(cloud);
			IsIntersects(cloud).Should().BeFalse();
		}

		[Test]
		[Order(4)]
		public void CorrectWorkWithDifferentCenter()
		{
			var center = new Point(-10, -10);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 4; i++)
				PutRandomRectangle(cloud);
			IsIntersects(cloud).Should().BeFalse();
		}

		[Test]
		[Order(5)]
		public void StartSecondLayerWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 5; i++)
				PutRandomRectangle(cloud);
			IsIntersects(cloud).Should().BeFalse();
		}

		[Test]
		[Order(6)]
		public void PutALotOfRectanglesWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 150; i++)
				PutRandomRectangle(cloud);
			IsIntersects(cloud).Should().BeFalse();
		}

		[TearDown]
		public void TearDown()
		{
			var graphics = new Graphics();
			//graphics.SaveMap(cloud.AllRectangles, TestContext.CurrentContext.Test.FullName);
		}

		private void PutRandomRectangle(CircularCloudLayouter cloud)
		{
			var size = new Size(Random.Next(50, 100), Random.Next(50, 100));
			var rectangle = cloud.PutNextRectangle(size);
		}

		private bool IsIntersects(CircularCloudLayouter cloud)
		{
			for (var i = 0; i < cloud.AllRectangles.Count; i++)
			{
				for (var j = i + 1; j < cloud.AllRectangles.Count; j++)
				{
					if (cloud.AllRectangles[i].IntersectsWith(cloud.AllRectangles[j]))
						return true;
				}
			}
			return false;
		}
	}
}