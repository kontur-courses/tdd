using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
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
            var tagCloud = new CircularCloudLayouter(new Point(5, 5));
            var tempBmpFile = "temp.bmp";

            File.Delete(tempBmpFile);
            tagCloud.PutNextRectangle(new Size(200, 100));
            SaveAsBitmap(tagCloud, tempBmpFile);

            File.Exists(tempBmpFile).Should().BeTrue();
        }
    }
}
