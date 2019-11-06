using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public CircularCloudLayouter(Point center)
        {
            
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return Rectangle.Empty;
        }
    }
    
    public class CircularCloudLayouterTests 
    { 
        [TestCaseSource(nameof(_CoordinateCenter))] 
        public void Constructor_DoesNotThrow_WithСorrectСenter(Point center) 
        { 
            Action action = () => new CircularCloudLayouter(center); 
            action.Should().NotThrow(); 
        }

        private static IEnumerable<TestCaseData> _CoordinateCenter = Enumerable 
            .Range(-1, 3) 
            .SelectMany(i => Enumerable 
                .Range(-1, 3) 
                .Select(j => new TestCaseData(new Point(i, j)).SetName("{m}: " + $"X = {i}, Y = {j}"))); 
    }
}