using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization;

public class RectanglesDrawerTests
{
    private RectanglesDrawer drawer;
    private string imagePath;
    private Rectangle[] rectangles;
    private Point center;
    
    [SetUp]
    public void SetUp()
    {
        imagePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "test_cloud.png");
        center = new Point(0, 0);
        rectangles = new Rectangle[3];
        
        rectangles[0] = new Rectangle(new Point(0, 0), new Size(100, 10));
        rectangles[1] = new Rectangle(new Point(-80, 0), new Size(80, 20));
        rectangles[2] = new Rectangle(new Point(0, 20), new Size(30, 30));
        
        drawer = new RectanglesDrawer(rectangles, center);
    }
    
    [Test]
    public void GenerateImage_ShouldSaveImage()
    {
        drawer.GenerateImage(imagePath);
        
        File.Exists(imagePath).Should().BeTrue("image not create");
    }
    
    [Test]
    public void GenerateImage_EmptyRectangles_ShouldThrowExeption()
    {
        var emptyRectangles = Array.Empty<Rectangle>();
        var drawer = new RectanglesDrawer(emptyRectangles, center);
        
        var act = () => drawer.GenerateImage(imagePath);

        act.Should().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void GenerateImage_EmptyPath_ShouldThrowExeption()
    {
        var act = () => drawer.GenerateImage(null);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [Test]
    public void GenerateImage_ShouldSaveImage_WithMinSizeForRectangles()
    {
        drawer.GenerateImage(imagePath);
        
        using (var image = Image.FromFile(imagePath))
        {
            image.Width.Should().Be(200);
            image.Height.Should().Be(100);
        }
    }
}