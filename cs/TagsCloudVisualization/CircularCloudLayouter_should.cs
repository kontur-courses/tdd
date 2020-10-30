using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private CircularCloudLayouter layouter;
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(10, 10));
        }
    }
}