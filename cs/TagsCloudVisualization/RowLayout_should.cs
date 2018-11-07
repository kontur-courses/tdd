using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class RowLayout_should
    {
        private RowLayout rowLayout;
        private const int hundred = 100;
        private Random rnd = new Random();
        private const int minHeight = 6;
        private const int maxHeight = 50;
        private const int maxWidthHeightRatio = 10;
        
        [SetUp]
        public void SetUp()
        {
            rowLayout = new RowLayout(new Rectangle(100,50,100,20));
        }
        
        private Size RandomSize()
        {
            var height = rnd.Next(minHeight, maxHeight);
            var width = rnd.Next(height, height * maxWidthHeightRatio);
            return new Size(width, height);
        }
    }
}