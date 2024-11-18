using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using NUnit.Framework.Interfaces;

namespace BaseCloudDrawer_Tests;

[TestFixture]
public class BaseCloudDrawer_Tests
{
    private BaseCloudDrawer drawer;
    private string testDirectory;

    [SetUp]
    public void SetUp()
    {
        drawer = new BaseCloudDrawer();
        testDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestImages");
        Directory.CreateDirectory(testDirectory);
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var files = Directory.GetFiles(testDirectory);

        if (status == TestStatus.Passed &&
            files.Length > 0) //удаляем в случае успешного прохождения, пишем в консоль при падении теста
        {
            File.Delete(files.OrderByDescending(f => new FileInfo(f).CreationTime).First());
        }
        else if (status == TestStatus.Failed)
        {
            Console.WriteLine($"Test failed. Image saved to {testDirectory}/{TestContext.CurrentContext.Test.Name}");
        }
    }

    [Test]
    public void DrawCloud_ThrowsArgumentException_WhenWidthIsNonPositive()
    {
        var rectangles = new List<Rectangle> { new Rectangle(0, 0, 10, 10) };

        Action act = () => drawer.DrawCloud(rectangles, 0, 100);

        act.Should().Throw<ArgumentException>().WithMessage("Width must be positive number");
    }

    [Test]
    public void DrawCloud_ThrowsArgumentException_WhenHeightIsNonPositive()
    {
        var rectangles = new List<Rectangle> { new Rectangle(0, 0, 10, 10) };

        Action act = () => drawer.DrawCloud(rectangles, 100, 0);

        act.Should().Throw<ArgumentException>().WithMessage("Height must be positive number");
    }


    [Test]
    public void DrawCloud_ReturnsBitmapWithCorrectDimensions()
    {
        var rectangles = new List<Rectangle> { new Rectangle(0, 0, 10, 10) };
        var imageWidth = 100;
        var imageHeight = 100;

        var bitmap = drawer.DrawCloud(rectangles, imageWidth, imageHeight);

        var fileName = TestContext.CurrentContext.Test.Name;
        var format = ImageFormat.Png;
        drawer.SaveToFile(bitmap, fileName, testDirectory, format);
        bitmap.Width.Should().Be(imageWidth);
        bitmap.Height.Should().Be(imageHeight);
    }

    [Test]
    public void DrawCloud_DrawsAllRectangles()
    {
        var rectangles = new List<Rectangle>
        {
            new Rectangle(0, 0, 10, 10),
            new Rectangle(20, 20, 10, 10)
        };
        var imageWidth = 100;
        var imageHeight = 100;

        var bitmap = drawer.DrawCloud(rectangles, imageWidth, imageHeight);

        using (var graphics = Graphics.FromImage(bitmap))
        {
            foreach (var rectangle in rectangles)
            {
                var pixelColor = bitmap.GetPixel(rectangle.X + 1, rectangle.Y + 1);
                pixelColor.Should().NotBe(Color.White);
            }
        }

        var fileName = TestContext.CurrentContext.Test.Name;
        var format = ImageFormat.Png;
        drawer.SaveToFile(bitmap, fileName, testDirectory, format);
    }

    [Test]
    public void SaveToFile_SavesBitmapToSpecifiedPath()
    {
        var bitmap = new Bitmap(10, 10);
        var fileName = "test_image";
        var format = ImageFormat.Png;

        drawer.SaveToFile(bitmap, fileName, testDirectory, format);

        var fullPath = Path.Combine(testDirectory, fileName + ".png");
        File.Exists(fullPath).Should().BeTrue();
    }
}