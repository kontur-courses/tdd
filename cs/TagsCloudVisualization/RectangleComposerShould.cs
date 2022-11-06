﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class RectangleComposerShould
    {
        [Test]
        public void FindFreePlaceOnSpiral_WhenNoFreePlace_ShouldExpandSpiral()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var beginSpiralLength = layouter.Composer.Spiral.Points.Count;
            var rect = new Rectangle(0, 0, 10, 10);

            layouter.Composer.FindFreePlaceOnSpiral(rect);
            layouter.Composer.FindFreePlaceOnSpiral(rect);
            layouter.Composer.FindFreePlaceOnSpiral(rect);

            layouter.Composer.Spiral.Points.Count.Should().BeGreaterThan(beginSpiralLength);
        }

        [TestCase(100, 100)]
        [TestCase(-100, 100)]
        [TestCase(-100, -100)]
        [TestCase(100, -100)]
        public void MoveToCenter_SingleRectAside_ShouldMoveToCenter(int posX, int posY)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rect = new Rectangle(posX, posY, 10, 10);

            var offsetRect = layouter.Composer.MoveToCenter(rect);
            var offsetRectCenterX = offsetRect.X + offsetRect.Width / 2;
            var offsetRectCenterY = offsetRect.Y + offsetRect.Height / 2;


            Math.Abs(offsetRectCenterX).Should().BeLessThan(RectangleComposer.centerAreaRadius);
            Math.Abs(offsetRectCenterY).Should().BeLessThan(RectangleComposer.centerAreaRadius);
        }

        [Test]
        public void MoveToCenter_RectAsideAndRectInCenter_AsideMoveToCenter()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            layouter.PutNextRectangle(new Size(100, 100));
            var rect = new Rectangle(500, 0, 10, 10);
            var expectedRectLocation = new Point(52, 0);

            var offsetRect = layouter.Composer.MoveToCenter(rect);

            offsetRect.Location.Should().Be(expectedRectLocation);
        }

        [Test]
        public void GetNextPointToCenter_ZeroAngle_PointShouldBeZero()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var expectedPoint = new Point(RectangleComposer.stepToCenter, 0);

            var nextPoint = layouter.Composer.GetNextPointToCenter(0);

            nextPoint.Should().Be(expectedPoint);
        }

        [Test]
        public void IsRectangleInCenter_PointInCenterArea_ShouldReturnTrue()
        {
            var center = new Point(0, 0);
            var rect = new Rectangle(-5, -5, 5, 5);

            var checkLocationInArea = RectangleComposer.IsRectangleInCenter(rect, center);

            checkLocationInArea.Should().BeTrue();
        }

        [Test]
        public void IsRectangleInCenter_PointNotInCenterArea_ShouldReturnFalse()
        {
            var center = new Point(0, 0);
            var rect = new Rectangle(15, 5, 10, 10);

            var checkLocationInArea = RectangleComposer.IsRectangleInCenter(rect, center);

            checkLocationInArea.Should().BeFalse();
        }

        [Test]
        public void IsRectangleNotIntersectOther_CommonData_ShouldReturnTrue()
        {
            var rectangle = new Rectangle(100, 100, 10, 10);
            var rects = new List<Rectangle>
            {
                new Rectangle(0, 0, 50, 50),
                new Rectangle(-100, -100, 10, 10),
                new Rectangle(200, 200, 50, 50),
                new Rectangle(30, 30, 5, 5)
            };

            var intersectFlag = RectangleComposer.IsRectangleNotIntersectOther(rectangle, rects);

            intersectFlag.Should().BeTrue();
        }

        [Test]
        public void IsRectangleNotIntersectOther_IntersectsRects_ShouldReturnFalse()
        {
            var rectangle = new Rectangle(100, 100, 100, 100);
            var rects = new List<Rectangle>
            {
                new Rectangle(0, 0, 125, 125),
            };

            var intersectFlag = RectangleComposer.IsRectangleNotIntersectOther(rectangle, rects);

            intersectFlag.Should().BeFalse();
        }
    }
}
