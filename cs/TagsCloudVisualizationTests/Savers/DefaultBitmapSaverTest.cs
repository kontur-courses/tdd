using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Savers;

namespace TagsCloudVisualizationTests.Savers;

[TestFixture]
[TestOf(typeof(DefaultBitmapSaver))]
public class DefaultBitmapSaverTest
{

    [Test]
    public void SaveBitmap_ShouldCreatesDirectory()
    {
        var directory = "TestDirectory";
        try
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
            var saver = new DefaultBitmapSaver(directory);
            var bitmap = new Bitmap(100, 100);

            saver.SaveBitmap(bitmap, directory);

            Directory.Exists(directory).Should().BeTrue();
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
    
    
    [Test]
    public void SaveBitmap_ShouldCreatesJpegFile()
    {
        const string directory = "TestDirectory";
        const string filename = "TestFilename.jpg";
        try
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
            var saver = new DefaultBitmapSaver(directory);
            var bitmap = new Bitmap(100, 100);

            saver.SaveBitmap(bitmap, filename);
            File.Exists(Path.Combine(directory, filename)).Should().BeTrue();
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
}