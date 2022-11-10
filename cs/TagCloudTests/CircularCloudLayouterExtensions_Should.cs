namespace TagCloudTests;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

public class CircularCloudLayouterExtensions_Should
{
    [Test]
    public void SaveAsImage_CreateFile()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        const string filename = "filename.jpg";
        
        layouter.SaveAsImage(filename, new Size(800, 600));
        
        File.Exists(filename).Should().BeTrue();
    }
}