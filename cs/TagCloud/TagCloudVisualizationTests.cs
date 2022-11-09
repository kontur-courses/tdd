using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    public class TagCloudVisualizationTests : TagCloudVisualization
    {
        [Test]
        public void GetRandomBrush_GenerateRandomBrush()
        {
            var brushes = new HashSet<Brush>();

            for (int i = 0; i < 10; i++)
                brushes.Add(GetRandomBrush());

            brushes.Count.Should().BeGreaterThan(7);
        }

        [Test]
        public void SaveAsBitmap_TagCloudInFile_Success()
        {
            var tagCloud = new CircularCloudLayouter(new Point(500, 500));
            var tempBmpFile = "temp.bmp";

            File.Delete(tempBmpFile);

            File.Exists(tempBmpFile).Should().BeFalse($"file {tempBmpFile} was deleted");

            for (int i = 200; i > 1; i -= 2)
                tagCloud.PutNextRectangle(new Size(i, i / 2));
            SaveAsBitmap(tagCloud, tempBmpFile);

            File.Exists(tempBmpFile).Should().BeTrue($"file {tempBmpFile} must be generated");
        }
    }
}
