using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class TestData
    {
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