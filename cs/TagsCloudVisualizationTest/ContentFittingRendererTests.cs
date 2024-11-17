using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Renderers;

namespace TagsCloudVisualizationTest;

public class ContentFittingRendererTests
{
    [TestCase("test.png")]
    [TestCase("render.png")]
    public void SaveImage_CreateFile_WithoutRectangles(string name)
    {
        var render = new ContentFittingRenderer();
        
        render.SaveImage(name);

        File.Exists(name).Should().BeTrue();
    }

    [TestCase("test.png")] 
    [TestCase("render.png")]
    public void SaveImage_SizeEqualOne_WithoutRectangles(string name)
    {
        var render = new ContentFittingRenderer();
        
        render.SaveImage(name);
        
        using var bmp = (Bitmap)Image.FromFile(name);
        
        bmp.Width.Should().Be(1);
        bmp.Height.Should().Be(1);
    }
    
    [TestCase("test.png")] 
    [TestCase("render.png")]
    public void SaveImage_RenderSizeEqualLayouterSize(string name)
    {
        var render = new ContentFittingRenderer();
        var layout = LayoutRegistry.GetRandomFilledCloud();
        var rectangles = LayoutRegistry.RandomFilledCloudRectangles;
        var left = 0;
        var right = 0;
        var top = 0;
        var bottom = 0;

        foreach (var rectangle in rectangles)
        {
            left = Math.Min(left, rectangle.Left);
            right = Math.Max(right, rectangle.Right);
            top = Math.Min(top, rectangle.Top);
            bottom = Math.Max(bottom, rectangle.Bottom);
            
            render.AddRectangle(rectangle);
        }
        
        
        render.SaveImage(name);
        
        var width = Math.Abs(left) + Math.Abs(right) + 1;
        var height = Math.Abs(top) + Math.Abs(bottom) + 1;
        
        using var bmp = (Bitmap)Image.FromFile(name);
        
        bmp.Width.Should().Be(width);
        bmp.Height.Should().Be(height);
    }
}