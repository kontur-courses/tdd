using System;
using System.Drawing;
using FluentAssertions;
using HomeExerciseTDD;
using NUnit.Framework;

namespace TestProject1
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [Test]
        public void PutNextRectangle_AlmostCircleFormAndArea_WhenPutManyRectangle()
        {
            var totalRectanglesArea = 0.0;
            var controlCircleArea = 0.0;
            var mostDistant = Int32.MinValue;
            var center = new Point(22,22);
            var layouter = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(2,2);
            for (var i = 0; i < 100; i++)
            {
                var newRectangle = layouter.PutNextRectangle(sizeRectangle);
                totalRectanglesArea += newRectangle.Width * newRectangle.Height;
                var horizontalMax = Math.Max(Math.Abs(center.X - newRectangle.Left), Math.Abs(center.X - newRectangle.Right));
                var verticalMax = Math.Max(Math.Abs(center.Y - newRectangle.Top), Math.Abs(center.Y - newRectangle.Bottom));
                if(Math.Max(horizontalMax, verticalMax)>mostDistant)
                    mostDistant = Math.Max(horizontalMax, verticalMax);
            }
            controlCircleArea = Math.PI * Math.Pow(mostDistant, 2);
            var fillSpacePercent = (totalRectanglesArea / controlCircleArea)*100;
            
            fillSpacePercent.Should().BeGreaterThan(80);
        }
        
        [Test]
        public void PutNextRectangle_ReturnNotNull_WhenPutSize()
        {
            var center = new Point(22,22);
            var layouter = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(1,1);

            var newRectangle = layouter.PutNextRectangle(sizeRectangle);

            newRectangle.Should().NotBe(null);
        }
        
        [Test]
        public void PutNextRectangle_ReturnRectangleInCenter_WhenAddedFirstRectangle()
        {
            var center = new Point(22,22);
            var layouter = new CircularCloudLayouter(center);
            var sizeRectangle = new Size(1,1);

            var firstRectangle = layouter.PutNextRectangle(sizeRectangle);

            firstRectangle.Location.Should().BeEquivalentTo(center);
        }
    }
}