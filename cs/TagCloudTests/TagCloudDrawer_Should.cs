using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class TagCloudDrawer_Should
{
    [Test]
    public void ImageSize_ThrowArgumentException_OnNegativeSize()
    {
        var drawer = new TagCloudDrawer();
        var size = new Size(-1, -1);

        var act = () => drawer.ImageSize = size;

        act.Should().Throw<ArgumentException>()
            .WithMessage($"Width and height of the image must be positive, but {size}");
    }
}