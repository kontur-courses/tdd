using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class RectangleStorageTests
    {
        [TestCase(0, ExpectedResult = Sector.First, TestName = "ReturnFirstOnAnglesFromFirstSector1")]
        [TestCase(1, ExpectedResult = Sector.First, TestName = "ReturnFirstOnAnglesFromFirstSector2")]
        [TestCase(Math.PI / 2, ExpectedResult = Sector.First, TestName = "ReturnFirstOnAnglesFromFirstSector2")]
        [TestCase(2, ExpectedResult = Sector.Second, TestName = "ReturnSecondOnAnglesFromSecondSector1")]
        [TestCase(Math.PI, ExpectedResult = Sector.Second, TestName = "ReturnSecondOnAnglesFromSecondSector2")]
        [TestCase(4.14, ExpectedResult = Sector.Third, TestName = "ReturnThirdOnAnglesFromThirdSector1")]
        [TestCase(3 * Math.PI / 2, ExpectedResult = Sector.Third, TestName = "ReturnThirdOnAnglesFromThirdSector2")]
        [TestCase(5, ExpectedResult = Sector.Fourth, TestName = "ReturnFourthOnAnglesFromFourthSector")]
        public Sector DetermineSectorByDirectionShould(double direction)
        {
            return RectangleStorage.DetermineSectorByDirection(direction);
        }
    }
}
