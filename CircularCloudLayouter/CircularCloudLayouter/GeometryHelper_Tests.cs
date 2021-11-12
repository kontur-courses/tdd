using FluentAssertions;
using NUnit.Framework;
using System;

namespace TagsCloudVisualizer
{
    class GeometryHelper_Tests
    {
        [TestCase(0, 0, ExpectedResult = new int[] { 0, 0 })]
        [TestCase(1, 0, ExpectedResult = new int[] { 0, 0 })]
        [TestCase(-1, 0, ExpectedResult = new int[] { 0, 0 })]
        [TestCase(0, 1, ExpectedResult = new int[] { 1, 0 })]
        [TestCase(0, 5, ExpectedResult = new int[] { 5, 0 })]
        [TestCase(Math.PI, 1, ExpectedResult = new int[] { -1, 0 })]
        [TestCase(Math.PI / 2, 1, ExpectedResult = new int[] { 0, 1 })]
        [TestCase(3 * Math.PI / 2, 1, ExpectedResult = new int[] { 0, -1 })]
        public int[] ConvertPolarCoorditatesToDecart_OnPolarCoordinates_ShouldReturnCorrectResult(double angle, double radius)
        {
            var p = GeometryHelper.ConvertFromPolarToDecartWithFlooring(angle, radius);
            return new int[] { p.X, p.Y };
        }
    }
}
