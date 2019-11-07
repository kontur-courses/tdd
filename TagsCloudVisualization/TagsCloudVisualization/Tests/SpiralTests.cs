using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture()]
    public class SpiralTests
    {
        [TestCaseSource(nameof(_coordinateCenter))]
        public void GetPoints_ShouldGiveFirstPointCenter(Point center)
        {
            var spiral = new Spiral(center);
            var iter = spiral.GetPoints().GetEnumerator();
            iter.MoveNext();
            iter.Current.Should().BeEquivalentTo(center);
        }
        
        [TestCaseSource(nameof(_coordinateCenter))]
        public void GetPoints_ShouldBeSpiral_WithThirteenCalls(Point center)
        {
            var points = new List<Point>();
            for (var i = -2; i <= 2 ; i++)
            for (var j = -2; j <= 2; j++)
                if (Math.Pow(i, 2) + Math.Pow(j, 2) <= Math.Pow(2, 2))
                    points.Add(new Point(i +  center.X, j + center.Y));
            
            var spiral = new Spiral(center);
            var points1 = new List<Point?>();
            var iter = spiral.GetPoints().GetEnumerator();
            iter.MoveNext();
            for (var i = 0; i < points.Count; i++)
            {
                points1.Add(iter.Current);

                iter.MoveNext();
            }

            points1.Should().Contain(points);

        }
        
        private static IEnumerable<TestCaseData> _coordinateCenter = Enumerable 
            .Range(-1, 3) 
            .SelectMany(i => Enumerable 
                .Range(-1, 3) 
                .Select(j => new TestCaseData(new Point(i, j)).SetName("{m}: " + $"X = {i}, Y = {j}"))); 
    }
}