using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class Drawer_Tests
    {
        [Test] 
        public void DrawRectangles_ThrowsArgumentException_WhenRectanglesDoNotFitInImage()
        {
            var drawer = new Drawer();
            var bigRectangle = new Rectangle(Point.Empty, new Size(55, 55));
            var imageSize = new Size(50, 50);

            Action action = () => drawer.DrawRectangles(new[] { bigRectangle }, imageSize);
            
            action.Should().Throw<ArgumentException>();
        }
    }
}