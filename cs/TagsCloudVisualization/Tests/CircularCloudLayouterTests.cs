using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests
{
    class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layourter;
        [SetUp]
        public void SetUp()
        {
            layourter = new CircularCloudLayouter(Point.Empty);
        }

        [Test]
        public void PutNextRectangle_Correct_WhenPutFirstRectangle()
        {
            var expectedRectangle = new Rectangle(new Point(-5, -5), new Size(10, 10));
            layourter.PutNextRectangle(new Size(10, 10)).Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_ShouldNoIntersections_WhenPutRectangles()
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
                rectangles.Add(layourter.PutNextRectangle(new Size(2, 2)));
            foreach(var i in rectangles)
                Console.WriteLine(i);
            ContainsAnyIntersections().Should().BeFalse();
            /*
             * Надо ли тестировать методы, использующиеся для теста.
             * Думаю да.
             * Тогда вопрос где и как.
             */


            bool ContainsAnyIntersections() 
            {
                for (var i = 0; i < rectangles.Count; i++)
                {
                    if (rectangles[i].IntersectsWith(rectangles.Take(i).Skip(1)))
                        return true;
                }

                return false;
            }
        }

        [TestCase(-1, 5, TestName = "When Size.Width is negative number")]
        [TestCase(5, -1, TestName = "When Size.Height is negative number")]
        [TestCase(5, 0, TestName = "When Size.Height is zero")]
        [TestCase(0, 5, TestName = "When Size.Width is zero")]
        public void PutNextRectangle_ThrowException(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            Action act = () => layourter.PutNextRectangle(rectangleSize);
            act.Should().Throw<ArgumentException>().WithMessage("Width and height of the rectangle must be non-negative");
        }
    }
}
