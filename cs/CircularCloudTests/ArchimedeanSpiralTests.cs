using System;
using System.Collections.Generic;
using System.Drawing;
using CircularCloud;
using FluentAssertions;
using NUnit.Framework;

namespace CircularCloudTests
{
    [TestFixture]
    public class ArchimedeanSpiral_Should
    {
        private ArchimedeanSpiral spiral;


        [SetUp]
        public void SetUp()
        {
            spiral = new ArchimedeanSpiral(0, 0);
        }

        [Test]
        public void GetNextPoint_Void_FirstPointIsCenter()
        {
            spiral.GetNextPoint().Should().Be(spiral.Center);
        }

        [Test]
        public void GetNextPoint_Void_ReturnsDifferentPoints()
        {
            var set = new HashSet<Point>();
            for (var i = 0; i < 10000; i++) set.Add(spiral.GetNextPoint());
            set.Count.Should().Be(10000);
        }

        [Test]
        public void GetNextPoint_Void_PointsLocatedCloseEnoughToEachOther()
        {
            var a = spiral.GetNextPoint();
            for (var i = 0; i < 10000; i++)
            {
                var b = spiral.GetNextPoint();
                var distance = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
                distance.Should().BeLessOrEqualTo(5,
                    "distance between points should be less or equal to 5 pixels. " +
                    $"Failed with Point number {i + 1} = {a} , and point number {i + 2} = {b}");
                a = b;
            }
        }
    }
}