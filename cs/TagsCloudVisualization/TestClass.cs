using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class TestClass
	{
		public static Random Random = new Random(10);

		private List<Rectangle> rectangles;

		[SetUp]
		public void SetUp()
		{
			rectangles = new List<Rectangle>();
		}

		private void PutRectangle(CircularCloudLayouter cloud)
		{
			var size = new Size(Random.Next(50, 100), Random.Next(50, 100));
			var rectangle = cloud.PutNextRectangle(size);
			rectangles.Add(rectangle);

		}
		private void PutFixRectangle(CircularCloudLayouter cloud)
		{
			var size = new Size(100,50);
			var rectangle = cloud.PutNextRectangle(size);
			rectangles.Add(rectangle);

		}

		private bool IsIntersects()
		{
			for (var i = 0; i < rectangles.Count; i++)
			{
				for (var j = i + 1; j < rectangles.Count; j++)
				{
					if (rectangles[i].IntersectsWith(rectangles[j]))
						return true;
				}
			}
			return false;
		}



		[Test, Order(1)]
		public void PutFirstRectangle()
		{
			var center = new Point(0,0);
			var cloud = new CircularCloudLayouter(center);
			PutRectangle(cloud);

			rectangles.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void CreateFirstLayerWithoutIntersection()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 4; i++)
			{
				PutRectangle(cloud);
			}
			IsIntersects().Should().BeFalse();
		}

		[Test]
		public void PutAlotOfRectangles()
		{
			var center = new Point(0, 0);
			var cloud = new CircularCloudLayouter(center);
			for (var i = 0; i < 500; i++)
			{
				PutRectangle(cloud);
			}
			IsIntersects().Should().BeFalse();
		}

		private void PutMYRectangle(CircularCloudLayouter cloud)
		{
			var size = new Size(25,25);
			var rectangle = cloud.PutNextRectangle(size);
			var size1 = new Size(25, 25);
			var rectangle1 = cloud.PutNextRectangle(size1);
			var size2 = new Size(25, 25);
			var rectangle2 = cloud.PutNextRectangle(size2);
			var size3 = new Size(25, 25);
			var rectangle3 = cloud.PutNextRectangle(size3);
			var size4 = new Size(25, 25);
			var rectangle4 = cloud.PutNextRectangle(size4);
			var size5 = new Size(25, 25);
			var rectangle5 = cloud.PutNextRectangle(size5);
			var size6 = new Size(25, 25);
			var rectangle6 = cloud.PutNextRectangle(size6);
			var size7 = new Size(25, 25);
			var rectangle7 = cloud.PutNextRectangle(size7);
			var size8 = new Size(25, 25);
			var rectangle8= cloud.PutNextRectangle(size8);
			var size9 = new Size(25, 25);
			var rectangle9 = cloud.PutNextRectangle(size9);
			var size10 = new Size(25, 25);
			var rectangle10 = cloud.PutNextRectangle(size10);
			var rectangle11 = cloud.PutNextRectangle(size10);
			var rectangle12 = cloud.PutNextRectangle(size10);
			var rectangle13 = cloud.PutNextRectangle(size10);
			var rectangle14 = cloud.PutNextRectangle(size10);
			var rectangle15 = cloud.PutNextRectangle(size10);
			var rectangle16 = cloud.PutNextRectangle(size10);
			var rectangle17 = cloud.PutNextRectangle(size10);
			rectangles.Add(rectangle);
			rectangles.Add(rectangle1);
			rectangles.Add(rectangle2);
			rectangles.Add(rectangle3);
			rectangles.Add(rectangle4);
			rectangles.Add(rectangle5);
			rectangles.Add(rectangle6);
			rectangles.Add(rectangle7);
			rectangles.Add(rectangle8);
			rectangles.Add(rectangle9);
			rectangles.Add(rectangle10);
			rectangles.Add(rectangle11);
			rectangles.Add(rectangle12);
			rectangles.Add(rectangle13);
			rectangles.Add(rectangle14);
			rectangles.Add(rectangle15);
			rectangles.Add(rectangle16);
			rectangles.Add(rectangle17);

		}

		[TearDown]
		public void TearDown()
		{
			var graphics = new Graphics();
			graphics.GetMap(rectangles, TestContext.CurrentContext.Test.FullName);
		}
	}
}