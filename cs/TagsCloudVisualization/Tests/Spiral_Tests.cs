using System;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Spiral_Tests
    {
        [Test]
        public void GetCoordinates_ReturnCoordinates_WhenCall()
        {
            var spiral = new Spiral();
            var i = 0;
            foreach (var p in spiral.GetPoints(new Size(2, 2)))
            {
                Console.WriteLine(p.X + ", " + p.Y);
                i++;
                if (i > 100) break;
            }
        }
    }
}