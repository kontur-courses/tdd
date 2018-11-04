using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudVisualizer_Should
    {
        private CircularCloudLayouter layout;
        private CircularCloudVisualizer visualizer;

        [SetUp]
        public void SetUp()
        {
            layout = new CircularCloudLayouter();    
            visualizer = new CircularCloudVisualizer(layout);
        }

        [Test]
        public void DrawRectangles_WithoutRectangles_ThrowsArgumentNullException()
        {
            Action action = () => visualizer.DrawRectangles(new List<Rectangle>());
            action.ShouldThrow<NullReferenceException>().WithMessage("No Rectangles");
        }

        [Test]
        public void DrawRectangles_AddSingleRectangle_DefaultSizes()
        {
            layout.PutNextRectangle(new Size(100, 100));
            visualizer.DrawRectangles(layout.Rectangles).Size.Should().Be(new Size(500, 500));
        }

        [Test]
        public void GetGetCircumscribedСircleRadius_AddFirstRectangle_CorrectCircleRadius()
        {
            layout.PutNextRectangle(new Size(200, 200));
            visualizer.GetCircumscribedСircleRadius(layout.Rectangles.First()).Should().Be(141);
        }

        [Test]
        public void ShiftRectangleToCenter_ShiftSingleRectangle_CorrectShift()
        {
            layout.PutNextRectangle(new Size(300, 300));
            visualizer.ShiftRectangleToCenter(layout.Rectangles.First()).Should().Be(new Rectangle(-150, -150, 300, 300));
        }
    }
}