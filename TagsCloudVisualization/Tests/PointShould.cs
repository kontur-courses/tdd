using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class PointShould
    {
        [TestCase(0,0,3,4,ExpectedResult = 5)]
        public int CalculateDistance(int xOne,int yOne,int xTwo,int yTwo)
        {
            var pointOne = new Point(xOne,yOne);
            var pointTwo = new Point(xTwo, yTwo);
            return pointTwo.DistanceFrom(pointOne);
        }
    }
}