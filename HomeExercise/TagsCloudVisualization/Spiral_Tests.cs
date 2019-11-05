using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class Spiral_Tests
	{
		[TestCase(0, 0, 0.0, TestName = "Zero size")]
		[TestCase(2, 2, 0.1592, TestName = "Square")]
		[TestCase(5, 3, 0.4775, TestName = "Horizontal rectangle")]
		[TestCase(3, 5, 0.4775, TestName = "Vertical rectangle")]
		public void CalculateDensity_ReturnsCorrectDensity(int width, int height, double expectedDensity)
		{
			var actualDensity = Spiral.CalculateDensity(new Size(width, height));
			actualDensity = Math.Round(actualDensity, 4);
			actualDensity.Should().Be(expectedDensity);
		}

		[Test]
		public void GetNextLocation_CalculateCorrectPoint()
		{
			
		}
	}
}