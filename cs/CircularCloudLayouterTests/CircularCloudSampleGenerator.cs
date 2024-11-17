using System.Drawing;
using TagsCloudVisualization;

namespace CircularCloudLayouterTests;

[TestFixture]
public class CircularCloudSampleGenerator
{
	private readonly string projectDir = Directory.GetParent(TestContext.CurrentContext.WorkDirectory)?.Parent?.Parent?.FullName ?? throw new NullReferenceException();
	private const string SamplesDirName = "Samples";

	[OneTimeSetUp]
	public void SetUp()
	{
		var samplesDir = Path.Combine(projectDir, SamplesDirName);
		if (!Directory.Exists(samplesDir))
		{
			Directory.CreateDirectory(samplesDir);
		}
	}

	[TestCase(50, 10, 50, "TestSample1")]
	[TestCase(100, 20, 60, "TestSample2")]
	[TestCase(200, 20, 70, "TestSample3")]
	public void PutNextRectangle_CreateTestSamples(int count, int minWidth, int maxWidth, string fileName)
	{
		var layouter = new CircularCloudLayouter(new Point(400, 400));
		var rectangles = CloudGenerator.GenerateRandomCloud(layouter, count, minWidth, maxWidth);
		CloudVisualiser.DrawAndSaveCloud(rectangles, Path.Combine(projectDir, SamplesDirName), fileName, 800, 800, Color.Azure);
	}
}