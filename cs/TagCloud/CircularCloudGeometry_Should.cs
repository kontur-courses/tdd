using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    public class CircularCloudGeometry_Should
    {
        private CircularCloudGeometry geometry;
        private HashSet<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            rectangles = new HashSet<Rectangle>();
            geometry = new CircularCloudGeometry(new Point(0, 0), rectangles);
        }

        [TestCase(0, 0, 0, 0, 0.0)]
        [TestCase(0, 0, 2, 2, 2.8284271247461903)]
        [TestCase(0, 0, 1, 3, 3.1622776601683795)]
        [TestCase(0, 0, -3, 2, 3.6055512754639891)]
        [TestCase(1, 1, 1, 1, 0)]
        [TestCase(1, 1, 1, -1, 2)]
        [TestCase(1, 1, 4, 1, 3)]
        [TestCase(1, 1, 4, -1, 3.6055512754639891)]
        [TestCase(-5, 13, 4, 6, 11.40175425099138)]
        [TestCase(4, 6, -5, 13, 11.40175425099138)]
        public void GetDistanceBetweenPoints_Should(int xFrom, int yFrom, int xTo, int yTo, double expected)
        {
            geometry.GetDistanceBetweenPoints(new Point(xFrom, yFrom), new Point(xTo, yTo)).Should().Be(expected);
        }

        [TestCase(3, 2, 3.6056)]
        [TestCase(2, 3, 3.6056)]
        [TestCase(5, 5, 7.0711)]
        [TestCase(10, 1, 10.0499)]
        public void GetFurthestDistance_Should(int width, int height, double expected)
        {
            rectangles.Add(new Rectangle(new Point(0, 0), new Size(width, height)));
            Math.Round(geometry.GetFurthestDistance(), 4).Should().Be(expected);
        }

        [TestCase(1, 0.3673)]
        public void GetCloudFullness_Should(int rectCount, double expected)
        {
            rectangles.Add(new Rectangle(new Point(0, 0), new Size(3, 2)));
            Math.Round(geometry.GetCloudFullnessPercent(), 4).Should().Be(expected);
        }

        [TestCase(0, 0, 0, 0, -3, -2, 3.6056)]
        [TestCase(0, 0, 1, 1, -3, -2, 2.2361)]
        [TestCase(-1, 1, 2, 2, -3, -2, 3.1623)]
        [TestCase(1, 1, 0, 0, 3, 2, 2.2361)]
        [TestCase(0, 0, 0, 0, 1, 1, 1.4142)]
        public void GetDistanceToRectangle_Should(int xFrom, int yFrom, int xTo, int yTo, int width, int height,
            double expected)
        {
            Math.Round(
                    geometry.GetDistanceToRectangle(new Point(xFrom, yFrom),
                        new Rectangle(new Point(xTo, yTo), new Size(width, height))), 4)
                .Should().Be(expected);
        }
    }
}