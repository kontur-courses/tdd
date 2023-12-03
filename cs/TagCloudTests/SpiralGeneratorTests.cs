namespace TagCloudTests
{
    [TestFixture]
    public class SpiralGeneratorTests
    {
        private const int Width = 1920;
        private const int Height = 1080;
        private SpiralGenerator spiral;
        private Point startPoint;

        [SetUp]
        public void Setup()
        {
            startPoint = new Point(Width / 2, Height / 2);
            spiral = new SpiralGenerator(startPoint);
        }

        [Test]
        public void GetNextPoint_ReturnsCenterPoint_OnFirstCall()
        {
            spiral.GetNextPoint().Should().BeEquivalentTo(startPoint);
        }

        [Test]
        public void GetNextPoint_ReturnsDifferentPoints_AfterMultipleCalls()
        {
            var spiralPoints = new HashSet<Point>();

            for (var i = 0; i < 50; i++)
            {
                spiralPoints.Add(spiral.GetNextPoint());
            }

            spiralPoints.Count.Should().BeGreaterThan(1);
        }

        [Test]
        public void SpiralGenerator_ThrowsException_WhenAnyCoordinateNegative()
        {
            Action action = () => { new SpiralGenerator(new Point(-1, 0)); };

            action.Should().Throw<ArgumentException>();
        }
    }
}