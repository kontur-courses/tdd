using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class ArchimedeanSpiral_Should
    {
        private List<Point> GetPoints(ArchimedeanSpiral archSpiral, int border)
        {
            var actual = new List<Point>();
            var count = 0;
            
            foreach (var point in archSpiral.GetNextSpiralPoint())
            {
                if (count == border)
                    break;
                actual.Add(point);
                count++;
            }

            return actual;
        }
        [Test]
        public void GetNextSpiralPoint_WhenStartIsZero_WorksCorrectly()
        {
            ArchimedeanSpiral archSpiral = new(1, 1, 0);
            var actual = GetPoints(archSpiral, 6);

            var expected = new List<Point>();
            expected.Add(new Point(0, 0));
            expected.Add(new Point(1, 1));
            expected.Add(new Point(-1, 2));
            expected.Add(new Point(-3, 0));
            expected.Add(new Point(-3, -3));
            expected.Add(new Point(1, -5));


            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetNextSpiralPoint_WithNonZeroStart_WorksCorrectly()
        {
            ArchimedeanSpiral archSpiral = new(1, 3, 5);
            var actual = GetPoints(archSpiral, 6);

            var expected = new List<Point>();
            expected.Add(new Point(0, 0));
            expected.Add(new Point(4, 7));
            expected.Add(new Point(-5, 10));
            expected.Add(new Point(-14, 2));
            expected.Add(new Point(-11, -13));
            expected.Add(new Point(6, -19));


            actual.Should().BeEquivalentTo(expected);
        }
    }
}

