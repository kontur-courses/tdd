using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class ArchimedeanSpiralTests
    {
        #region Instance
        public static IEnumerable<TestCaseData> Instance_IncorrectParameters
        {
            get
            {
                yield return new TestCaseData(new Point(1, 1), -1, 1).SetName("startRadius < 0");
                yield return new TestCaseData(new Point(1, 1), 1, -1).SetName("extendRatio < 0");
                yield return new TestCaseData(new Point(1, 1), 1, 0).SetName("extendRatio == 0");
                yield return new TestCaseData(new Point(1, 1), -1, -1).SetName("startRadius < 0 and extendRatio < 0");
                yield return new TestCaseData(new Point(1, 1), -1, 0).SetName("startRadius < 0 and extendRatio == 0");
            }
        }
        
        [TestCaseSource(nameof(Instance_IncorrectParameters))]
        public void Instance_IncorrectParameters_ShouldFail(Point startPoint, int startRadius, int extendRatio)
        {
            Action instantiating = () => new ArchimedeanSpiral(startPoint, startRadius, extendRatio);
            instantiating.Should().Throw<ArgumentException>();
        }
        
        public static IEnumerable<TestCaseData> Instance_CorrectParameters
        {
            get
            {
                yield return new TestCaseData(new Point(1, 1), 1, 1);
                yield return new TestCaseData(new Point(1, 1), 0, 1);
                yield return new TestCaseData(new Point(1, 1), 2, 3);
            }
        }
        
        [TestCaseSource(nameof(Instance_CorrectParameters))]
        public void Instance_CorrectParameters_ShouldNotFail(Point startPoint, int startRadius, int extendRatio)
        {
            Action instantiating = () => new ArchimedeanSpiral(startPoint, startRadius, extendRatio);
            instantiating.Should().NotThrow<ArgumentException>();
        }
        #endregion

        private ArchimedeanSpiral _spiral;
        [SetUp]
        public void SetUp()
        {
            _spiral = new ArchimedeanSpiral(new Point(500, 500), 0, 1);
        }
        public static IEnumerable<TestCaseData> GetPoint
        {
            get
            {
                yield return new TestCaseData(0, Point.Empty).SetName("Zero angle should return start position");
                yield return new TestCaseData(Math.PI / 2, new Point(0, Convert.ToInt32(Math.PI / 2))).SetName("Pi/2 angle");
                yield return new TestCaseData(Math.PI, new Point(-Convert.ToInt32(Math.PI), 0)).SetName("Pi angle");
                yield return new TestCaseData(3 * Math.PI / 2, new Point(0, -Convert.ToInt32(3 * Math.PI / 2))).SetName("3*Pi/2 angle");
                yield return new TestCaseData(2 * Math.PI, new Point(Convert.ToInt32(2 * Math.PI), 0)).SetName("2*Pi angle");
                yield return new TestCaseData(4 * Math.PI, new Point(Convert.ToInt32(4 * Math.PI), 0)).SetName("4*Pi angle");
            }
        }

        [TestCaseSource(nameof(GetPoint))]
        public void GetPoint_ShouldReturnPoint(double angle, Point result)
        {
            _spiral.GetPoint(angle).Should().Be(result + (Size)_spiral.StartPoint);
        }
    }
}