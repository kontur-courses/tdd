using System;
using System.Collections.Generic;
using NUnit;
using FluentAssertions;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class NewTestClass
	{
		[Test, Order(1)]
		public void ShoudNotThrowExceptionWhenCreateCenter()
		{
			var center = new Point(0,0);
			Action act = () => new CircularCloudLayouterBasedOnSpiral(center);
			act.Should().NotThrow<NotImplementedException>();
		}

		[Test, Order(2)]
		public void PutFirstRectangle()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouterBasedOnSpiral(center);
			cloud.PutNextRectangle(GetRandomSize());
			cloud.AllRectangles.Should().NotBeNullOrEmpty();
		}

		[Test, Order(3)]
		public void PutSecondRectangle()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouterBasedOnSpiral(center);
			cloud.PutNextRectangle(GetRandomSize());
			cloud.PutNextRectangle(GetRandomSize());
			var intersection = IsIntersects(cloud.AllRectangles);
			intersection.Should().BeFalse();
		}

		[Test, Order(6)]
		public void PutManyRectangle()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouterBasedOnSpiral(center);
			for (var i = 0; i < 500; i++)
			{
				cloud.PutNextRectangle(GetRandomSize());
			}
			var intersection = IsIntersects(cloud.AllRectangles);
			var graphics = new Graphics();
			graphics.SaveMap(cloud.AllRectangles, TestContext.CurrentContext.Test.FullName);
			intersection.Should().BeFalse();
		}


		public Random rnd = new Random();
		private Size GetRandomSize()
		{
			return new Size(rnd.Next(10,50), rnd.Next(10, 50));
		}
		private bool IsIntersects(List<Rectangle> AllRectangles)
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
	}
}