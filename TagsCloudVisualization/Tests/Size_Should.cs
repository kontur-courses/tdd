using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Size_Should
    {
        [TestCase(-1,-1,TestName = "BothNegatives",ExpectedResult = false)]
        [TestCase(-1, 1, TestName = "WidthNegative", ExpectedResult = false)]
        [TestCase(1, -1, TestName = "HeightNegative", ExpectedResult = false)]
        public bool ThrowException_WhenCreateWithNegativeDimensions(double width,double height)
        {
            try
            {
                var size = new Size(width, height);
            }
            catch (ArgumentException e)
            {
                return false;
                
            }
            return true;
        }


        [TestCase(0, 0, TestName = "BothZero", ExpectedResult = false)]
        [TestCase(0, 1, TestName = "WidthZero", ExpectedResult = false)]
        [TestCase(1, 0, TestName = "HeightZero", ExpectedResult = false)]
        public bool ThrowException_WhenCreateWithZeroDimensions(double width,double height)
        {
            try
            {
                var size = new Size(width, height);
            }
            catch (ArgumentException e)
            {
                return false;

            }
            return true;
        }
    }
}