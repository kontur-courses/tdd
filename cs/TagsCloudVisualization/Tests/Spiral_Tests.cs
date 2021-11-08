using System;
using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Spiral_Tests
    {
        [Test]
        public void GetCoordinates_ReturnCoordinates_WhenCall()
        {
            var spiral = new Spiral();
            var i = 0;
            foreach (var p in spiral.GetPoints(new PointF()))
            {
                Console.WriteLine(p.X + ", " + p.Y);
                i++;
                if (i > 100) break;
            }
        }
    }
}