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
            var i = 0;
            foreach (var p in Spiral.GetCoordinates(new Size(2, 2), 1))
            {
                Console.WriteLine(p.X + ", " + p.Y);
                i++;
                if (i > 100) break;
            }
        }
    }
}