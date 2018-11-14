using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    public abstract class TestsHandler : TestFixtureAttribute
    {
        public CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(center: new Point(500, 500));
        }

        [TearDown]
        public void TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var debugPath = TestContext.CurrentContext.TestDirectory;
            var subfolderName = "";
            var path = "";

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                subfolderName = "FailedTests";
                TestContext.Error.WriteLine($"Tag cloud visualization saved to file {path}");
            }
            else
                subfolderName = "SuccessTests";

            path = string.Format($@"{debugPath}\{subfolderName}\");
            Drawer.DrawAndSaveRectangles(new Size(1000, 1000), layouter.Rectangles, testName + ".png", path);
        }
    }
}
