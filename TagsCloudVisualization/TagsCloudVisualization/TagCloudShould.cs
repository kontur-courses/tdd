using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class TagCloudShould
    {
        private Visualizator _visualizator;
        [SetUp]
        public void SetUp()
        {
            _visualizator = new Visualizator();
        }

        [Test]
        public void NoVisualization_NoIntersections()
        {
            foreach (var rectangle in _visualizator.rectangles)
            {
                _visualizator.Run();
                var a = _visualizator.rectangles.Where(x=>x!=rectangle).Select(r => r.IntersectsWith(rectangle));
                a.Select(x=>x.Should().BeFalse());
            }
        }
        [Test]
        public void NoVisualization_NoArgumentExceptionWhenNotSpaceForSmallestTag()
        {
            _visualizator.Run();
            Action runVisual = () => ArgumentException_WhenHaveEmptySpaceSmall();
            runVisual.Should().NotThrow<Exception>();
        }

        public void ArgumentException_WhenHaveEmptySpaceSmall()
        {
            var arithmeticSpiral = new ArithmeticSpiral(new Point(_visualizator.srcSize / 2));
            var point = arithmeticSpiral.GetPoint();
            var smallRectangle = _visualizator.rectangles.Last();
            var smallOptions = new Tuple<string, Size, Font>("small", smallRectangle.Size, new Font("Times", 5));
            while (!new Rectangle(point - (smallOptions.Item2 / 2), smallOptions.Item2).IntersectsWith(smallRectangle))
            {
                point = arithmeticSpiral.GetPoint();
                if (!_visualizator.rectangles
                        .Select(x =>
                            x.IntersectsWith(new Rectangle(point - (smallOptions.Item2 / 2), smallOptions.Item2)))
                        .Contains(true))
                {
                    throw new Exception($"Rectangle {smallOptions.Item2} input in space: {point - smallOptions.Item2 / 2}.But must in {smallRectangle.Location - smallOptions.Item2 / 2}");
                }
            }
        }
        [Test,Timeout(12000)]
        public void NoVisualization_Timeout()
        {
            _visualizator.Run();
             
        }
        [Test, Timeout(12000)]
        public void Visualization_Timeout()
        {
            _visualizator.RunWithSave("Visualization_Timeout");
        }

        [Test]
        public void Visualization_WithRandomSave()
        {
            var random = new Random(123);
            var strsplt = new string[400];


            for (int i = 0; i < 400; i++)
            {
                strsplt[i]=random.Next(1, 100).ToString();
            }


            _visualizator = new Visualizator(new FrequencyTags(string.Join(", ", strsplt).Split(", ")));
            _visualizator.RunWithSave("Visualization_WithRandomSave");
        }
    }
}
