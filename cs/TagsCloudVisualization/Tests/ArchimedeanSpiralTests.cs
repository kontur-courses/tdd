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
                yield return new TestCaseData(-1, 1).SetName("a < 0");
                yield return new TestCaseData(1, -1).SetName("b < 0");
                yield return new TestCaseData(1, 0).SetName("b == 0");
                yield return new TestCaseData(-1, -1).SetName("a <0 and b < 0");
                yield return new TestCaseData(-1, 0).SetName("a <0 and b == 0");
            }
        }
        
        [TestCaseSource(nameof(Instance_IncorrectParameters))]
        public void Instance_IncorrectParameters_ShouldFail(int a, int b)
        {
            Action instantiating = () => new ArchimedeanSpiral(a, b);
            instantiating.Should().Throw<ArgumentException>();
        }
        
        public static IEnumerable<TestCaseData> Instance_CorrectParameters
        {
            get
            {
                yield return new TestCaseData(1, 1);
                yield return new TestCaseData(0, 1);
                yield return new TestCaseData(2, 3);
            }
        }
        
        [TestCaseSource(nameof(Instance_CorrectParameters))]
        public void Instance_CorrectParameters_ShouldNotFail(int a, int b)
        {
            Action instantiating = () => new ArchimedeanSpiral(a, b);
            instantiating.Should().NotThrow<ArgumentException>();
        }
        #endregion

        private ArchimedeanSpiral _spiral;
        [SetUp]
        public void Setup()
        {
            _spiral = new ArchimedeanSpiral(0, 1);
        }
        
        public static IEnumerable<TestCaseData> GetPoint
        {
            get
            {
                yield return new TestCaseData(0, Point.Empty).SetName("Start position");
                yield return new TestCaseData(Math.PI / 2, new Point(0, Convert.ToInt32(Math.PI / 2))).SetName("Pi/2 angle");
                yield return new TestCaseData(Math.PI, new Point(-Convert.ToInt32(Math.PI), 0)).SetName("Pi angle");
                yield return new TestCaseData(3 * Math.PI / 2, new Point(0, -Convert.ToInt32(3 * Math.PI / 2))).SetName("3*Pi/2 angle");
                yield return new TestCaseData(2 * Math.PI, new Point(Convert.ToInt32(2 * Math.PI), 0)).SetName("2*Pi angle");
                yield return new TestCaseData(4 * Math.PI, new Point(Convert.ToInt32(4 * Math.PI), 0)).SetName("4*Pi angle");
            }
        }

        [TestCaseSource(nameof(GetPoint))]
        public void GetPoint_ShouldReturnPoint(double angle, Point point)
        {
            _spiral.GetPoint(angle).Should().Be(point);
        }
    }
}