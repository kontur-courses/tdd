using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class TagsCloudDrawerTests
{
    [TestCase("test", "<>\\")]
    [TestCase("test", "name|123")]
    [TestCase("test", null)]
    [TestCase("test", "")]
    [TestCase(null, "filename")]
    [TestCase("","filename")]
    [TestCase("  ","filename")]
    [TestCase(@"\:\","filename")]
    public void SaveImage_ThrowsArgumentException_OnInvalidParameters(string dirPath, string filename)
    {
        Assert.Throws<ArgumentException>(() => TagsCloudDrawer.SaveImage(new Bitmap(1, 1), dirPath, filename));
    }
}