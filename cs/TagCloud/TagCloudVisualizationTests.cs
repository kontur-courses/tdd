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
            for(int i=0;i<1000;i++)
                tagCloud.PutNextRectangle(new Size(20, 10));
            SaveAsBitmap(tagCloud, tempBmpFile);

            File.Exists(tempBmpFile).Should().BeTrue();
        }
    }
}
