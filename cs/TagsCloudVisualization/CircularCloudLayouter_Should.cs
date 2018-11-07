﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    internal class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private Point center;
        private readonly Random rnd = new Random();
        private const int MinHeight = 6;
        private const int MaxHeight = 50;
        private const int MaxWidthHeightRatio = 10;
        private const int Hundred = 100;
        private const int Thousand = 100;

        [SetUp]
        public void SetUp()
        {
            center = new Point(120,150);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void HaveCorrectCenter()
        {
            layouter.Center.Should().BeEquivalentTo(center);
        }

        [Test]
        public void MakeRectangleWithCorrectSize()
        {
            var size = RandomSize();
            layouter.PutNextRectangle(size).Size.Should().BeEquivalentTo(size);
        }

        [Test]
        public void PutFirstRectangleCenterOnLayoutCenter()
        {
            var rect = layouter.PutNextRectangle(RandomSize());
            rect.Center().Should().BeEquivalentTo(center);
        }

        [Test]
        public void MakeManyCorrectRectangles()
        {
            const int count = 100;
            var sizes = count.Times(RandomSize).ToArray();
            sizes.Select(layouter.PutNextRectangle).Select(x => x.Size).Should().BeEquivalentTo(sizes);
        }

        private Size RandomSize()
        {
            var height = rnd.Next(MinHeight, MaxHeight);
            var width = rnd.Next(height, height* MaxWidthHeightRatio);
            return new Size(width, height);
        }

        private Rectangle[] GenerateRectangles(int count)=>
            count.Times(RandomSize).Select(layouter.PutNextRectangle).ToArray();

        [Test]  
        public void DoNotOverlapManyRectangles()
        {
            var rectangles = GenerateRectangles(Hundred);

            for (int i = 0; i < Hundred; i++) 
            for (int j = 0; j < i; j++)
                Assert.False(rectangles[i].IntersectsWith(rectangles[j]),
                    $"{i}th rectangle was {rectangles[i]}, and {j}th rectangle was{rectangles[j]}");
        }
        
        [Test]
        public void NotIntersectRectanglesRowwise()
        {
            GenerateRectangles(Hundred);
            foreach (var rowLayout in layouter.layout)
                rowLayout.Body.ForAllPairs<Rectangle>((x, y) => 
                {
                    Assert.False(x.IntersectsWith(y),
                        $"One rectangle was {x} on height, and other rectangle was{y}");
                });
        }

        [Test]
        public void HaveCorrectSizes()=>
            Hundred.Times(() =>
            {
                var size = RandomSize();
                layouter.PutNextRectangle(size).Size.Should().Be(size);
            });

        [Test]
        public void BoundsWidthShouldMatchReactanglesSum()
        {
            GenerateRectangles(Hundred);
            foreach (var rowLayout in layouter.layout)
                rowLayout.Body.Sum(x => x.Width).Should().Be(rowLayout.Bounds.Width);
        }

        [Test]
        public void HaveSameLayoutAsReturnedRectangles()
        {
            GenerateRectangles(Hundred).ToArray().Should().BeEquivalentTo(layouter.Layout);
        }

        [Test]
        public void MakeSquareFrom3Bricks1X3()
        {
            var square = 3.Times(() => layouter.PutNextRectangle(new Size(90, 30))).Unite();
            square.Width.Should().Be(square.Height,
                $"{layouter.Layout.First()} , {layouter.Layout.Skip(1).First()}, {layouter.Layout.Last()}");
        }

        [Test]
        public void NotMoveCenter3Bricks1X3()
        {
            var square = 3.Times(() => layouter.PutNextRectangle(new Size(90, 30))).Unite();
            square.Center().Should().Be(center);
        }

        [Test]
        public void NotOverlap3Bricks1X3()
        {
            3.Times(() => layouter.PutNextRectangle(new Size(90, 30))).ToArray().ForAllPairs((x, y) =>
            {
                Assert.False(x.IntersectsWith(y),
                    $"One rectangle was {x}, and other rectangle was{y}");
            });
        }

        [Test]
        public void FitRectanglesIntoBoundsOfItsRow()
        {
            GenerateRectangles(Thousand);
            foreach (var row in layouter.layout)
            foreach (var rect in row.Body)
                Assert.IsTrue(row.Bounds.Contains(rect),$"{rect}, does tot fit into {row.Bounds}");
        }

        [Test]
        public void NotIntersectRows()
        {
            GenerateRectangles(Hundred);
            layouter.layout.Select(x=>x.Bounds).ForAllPairs((x, y) =>
            {
                Assert.False(x.IntersectsWith(y), //TODO DRY
                    $"One rectangle was {x}, and other rectangle was{y}");
            });
                
        }
        
        [Test]
        public void FitManySameSizesInto2TimesBiggerCircle()
        {
            var size = new Size(24,120);
            //var space = sizes.Aggregate(0, (sum, size) => sum + size.Space());
            var space = size.Space() * Hundred;
            var radius = Math.Sqrt(2 * space / Math.PI);
            
            var rects = Hundred.Times(()=>layouter.PutNextRectangle(size));
            
            rects.SelectMany(x => x.Points())
                .Select(x => x.DistanceTo(center))
                .All(x => x < radius)
                .Should().BeTrue();
        }
        
    }
}
