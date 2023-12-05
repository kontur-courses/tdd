using System.Drawing.Imaging;

namespace TagCloud.Tests
{
    public class CircularCloudLayouterTests
    {
        private readonly string projectDirectory
            = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        private Point center;
        private Size imageSize;

        private CircularCloudLayouter layouter;
        private Size[] sizes;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var path = Path.Combine(projectDirectory, "FailureImages");

            var dir = new DirectoryInfo(path);

            foreach(var file in dir.EnumerateFiles()) file.Delete();
        }
        
        [SetUp]
        public void Setup()
        {
            imageSize = new Size(2000, 2000);

            center = new Point(imageSize.Width / 2, imageSize.Height / 2);
            layouter = new CircularCloudLayouter(center);
            GenerateSizes();
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

            Assert.True(rect.Width == sizes[0].Width && rect.Height == sizes[0].Height);
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
                        if (rect1.IntersectsWith(rect2))
                            Assert.False(rect1.IntersectsWith(rect2));
                            Console.WriteLine(" ");
                }
            }
        }

        [Test]
        public void PutNextRectangle_PlaceManyRectangles_ShouldFitInCircle()
        {
            var rectangles = sizes.Select(x => layouter.PutNextRectangle(x)).ToArray();


            var totalArea = rectangles.Select(x => x.Width * x.Height).Sum();
            var radius = Math.Sqrt(totalArea / Math.PI) * 1.5;

            foreach (var rect in rectangles)
            {
                Assert.True(new PointF(rect.X, rect.Y).DistanceTo(center) < radius);
            }
        }

        private void GenerateSizes()
        {
            var sizes = new List<Size>();

            for (int x = 30; x < 55; x++)
            {
                for (int y = 30; y < 55; y++)
                {
                    sizes.Add(new Size(x, y));
                }
            }

            this.sizes = sizes.ToArray();
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            var fileName = $"{context.Test.Name}.jpg";
            var pathToSave = Path.Combine(projectDirectory, "FailureImages", fileName);

            if (context.Result.FailCount != 0)
            {
                var img = TagCloudVisualizer.DrawImage(layouter.Rectangles, imageSize);
                img.Save(pathToSave, ImageFormat.Jpeg);
                Console.WriteLine($"Tag cloud visualization saved to file {pathToSave}");
            }
        }
    }
}