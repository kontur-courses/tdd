using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.IO;
using System.Threading;

namespace TagsCloudVisualization_Should
{
    [TestFixture]
    public class NotThrowExceptions
    {
        [TestCase(0, 0, TestName = "NotThrowsError_CreationZeroZero")]
        [TestCase(-1, -2, TestName = "NotThrowsError_CreationBothNegative")]
        [TestCase(1, 2, TestName = "NotThrowsError_CreationBothPositive")]
        public void Creation(int x, int y)
        {
            var point = new Point(x,y);

            Action act = () => new CircularCloudLayouter(point);
            
            act.ShouldNotThrow();
        }
    }


    [TestFixture]
    public class Rectangles
    {
        [TestCase(10,20,10,20,TestName = "ReturnOneRectangle_AddOneRectangle")]
        public void AddRectangle(int x, int y, int expectedX, int expectedY, int startPoseX = 0, int startPoseY = 0)
        {
            var expectedSize = new Size(expectedX, expectedY);
            var expectedRectangle = new Rectangle(new Point(startPoseX,startPoseY), expectedSize);
            var cloud = new CircularCloudLayouter(new Point(startPoseX,startPoseY));

            var actual = cloud.PutNextRectangle(new Size(x, y));

            actual.Should().Equals(expectedRectangle);

        }

        
    }


}