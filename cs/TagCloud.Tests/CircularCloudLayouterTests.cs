using System.Drawing.Imaging;

namespace TagCloud.Tests
{
    public class CircularCloudLayouterTests
    {
        private readonly static string projectDirectory
            = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private readonly static string pathToFailureImagesFolder 
            = Path.Combine(projectDirectory, "FailureImages");

        private Point center;
        private Size imageSize;

        private CircularCloudLayouter layouter;
        private Size[] sizes;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EnsureFolderCreated(pathToFailureImagesFolder);

            var dir = new DirectoryInfo(pathToFailureImagesFolder);

            foreach(var file in dir.EnumerateFiles()) file.Delete();
        }

        public static void EnsureFolderCreated(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }

        [SetUp]
        public void Setup()
        {
            imageSize = new Size(2000, 2000);

            center = new Point(imageSize.Width / 2, imageSize.Height / 2);
            layouter = new CircularCloudLayouter(center);
            sizes = GenerateSizes();
        }

        [Test]
        public void PutNextRectangle_PlaceOneRectangle_DoesntThrowException()
        {
            Assert.DoesNotThrow(() => layouter.PutNextRectangle(sizes[0]));       
        }

        [Test]
        public void PutNextRectangle_PlaceOneRectangle_ReturnRightSizeRect()
        {
            var rect = layouter.PutNextRectangle(sizes[0]);

            Assert.True(rect.Size == sizes[0]);
        }

        [Test]
        public void PutNextRectangle_PlaceManyRectangles_ShouldNotIntersect()
        {
            var rectangles = sizes.Select(x => layouter.PutNextRectangle(x)).ToArray();


            foreach (var rect1 in rectangles)
            {
                foreach(var rect2 in rectangles)
                {
                    if (rect1 != rect2)
                        Assert.False(rect1.IntersectsWith(rect2));
                }
            }
        }

        [Test]
        public void PutNextRectangle_PlaceManyRectangles_ShouldFitInCircleAroundCenter()
        {
            var rectangles = sizes.Select(x => layouter.PutNextRectangle(x)).ToArray();


            var totalArea = rectangles.Select(x => x.Width * x.Height).Sum();
            var radius = Math.Sqrt(totalArea / Math.PI) * 1.2;

            foreach (var rect in rectangles)
            {
                Assert.True(new PointF(rect.X, rect.Y).DistanceTo(center) < radius);
            }
        }

        public static Size[] GenerateSizes()
        {
            var sizes = new List<Size>();

            for (int x = 30; x < 55; x++)
            {
                for (int y = 30; y < 55; y++)
                {
                    sizes.Add(new Size(x, y));
                }
            }

            return sizes.ToArray();
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            var fileName = $"{context.Test.Name}.jpg";
            var pathToSave = Path.Combine(pathToFailureImagesFolder, fileName);

            if (context.Result.FailCount != 0)
            {
                var img = TagCloudVisualizer.DrawImage(layouter.Rectangles, imageSize);
                img.Save(pathToSave, ImageFormat.Jpeg);
                Console.WriteLine($"Tag cloud visualization saved to file {pathToSave}");
            }
        }
    }
}