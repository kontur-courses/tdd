using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class Tests
    {
        private Point _center;
        private CircularCloudLayouter _layouter;

        [SetUp]
        public void Setup()
        {
            _center = new Point(0, 0);
            _layouter = new CircularCloudLayouter(_center);
        }
    }
}