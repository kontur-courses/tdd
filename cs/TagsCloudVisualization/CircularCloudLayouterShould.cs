using System;
using System.Collections.Generic;
using NUnit;
using FluentAssertions;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class CircularCloudLayouterShould
	{
		[Test, Order(1)]
		public void NotThrowExceptionWhenCreateCenter()
		{
			var center = new Point(0,0);
			Action act = () => new CircularCloudLayouter(center);
			act.Should().NotThrow<NotImplementedException>();
		}

		[Test, Order(2)]
		public void CreateAndSaveFirstRectangle()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			cloud.PutNextRectangle(GetRandomSize());
			cloud.AllRectangles.Should().NotBeNullOrEmpty();
		}

		[Test, Order(3)]
		public void PutSecondRectangleWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			cloud.PutNextRectangle(GetRandomSize());
			cloud.PutNextRectangle(GetRandomSize());
			var intersection = AreRectanglesIntersect(cloud.AllRectangles);
			intersection.Should().BeFalse();
		}

		[Test, Order(4)]
		public void PutManyRectanglesWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 1000; i++)
			{
				cloud.PutNextRectangle(GetRandomSize());
			}
			var intersection = AreRectanglesIntersect(cloud.AllRectangles);
			intersection.Should().BeFalse();
		}
		
		public Random Random = new Random();

		private Size GetRandomSize()
		{
			return new Size(Random.Next(10,50), Random.Next(10, 50));
		}

		private bool AreRectanglesIntersect(List<Rectangle> allRectangles)
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
	}
}