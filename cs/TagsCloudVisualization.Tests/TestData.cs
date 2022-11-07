using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class TestData
    {
        public static TestCaseData[] DefaultPointsAndSizeForPlace =
        {
            new TestCaseData(0, 0, 4, 4).Returns(new Rectangle(new Point(-2, -2), new Size(4, 4))),
            new TestCaseData(-2, -2, 4, 4).Returns(new Rectangle(new Point(-4, -4), new Size(4, 4))),
            new TestCaseData(0, 0, 10, 10).Returns(new Rectangle(new Point(-5, -5), new Size(10, 10))),
        };

        public static TestCaseData[] IncorrectStepCount =
        {
            new TestCaseData(0).SetName("Set zero in Step"),
            new TestCaseData(-5).SetName("Set negative number in Step")
        };
            
        public static TestCaseData[] CorrectStepCount = {
            new TestCaseData(1).SetName("Set one to Steps"),
            new TestCaseData(25).SetName("Set 25 to Steps")
        };
    }
}