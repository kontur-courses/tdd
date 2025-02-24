using System.Drawing;
using System.Net;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisuliazation.Test;

public class CloudBitMapTests
{
    [Test]
    public void CloudBitMap_DirectoryShouldBeExist()
    {
        var filePath = "./../../../OuterWild/notIntersect-50.png";
        Action action = () => new CloudBitMap(5, 5, filePath);
        action.Should().Throw<DirectoryNotFoundException>();
    }
 
    [Test]
    public void CloudBitMap_ShouldBe_SizeWithPositiveNumbers()
    {
        var filePath = "./../../../photos/notIntersect-50.png";
        Action action = () => new CloudBitMap(-1, 5, filePath);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CloudBitMap_ShouldBe_CreatePhoto()
    {
        var filePath = "./../../../photos/test_create_photo.png";
        var cloudBitMap = new CloudBitMap(30, 300, filePath);
        
        cloudBitMap.Save();
        File.Exists(filePath).Should().BeTrue();

        File.Delete(filePath);
    }
}