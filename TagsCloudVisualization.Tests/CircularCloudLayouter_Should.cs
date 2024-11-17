using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using System.Reflection;
using System.Drawing;
using TagsCloudVisualization.Tests;
using System.IO;
using System.Runtime.InteropServices;
using FluentAssertions.Execution;
using NUnit.Framework.Interfaces;


namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layout;
        [TestCase(1, 2, TestName = "Odd coordinate value results in an even size value")]
        [TestCase(2, 5, TestName = "Even coordinate value results in an odd size value")]
        public void CircularCloudLayouter_MakeRightSizeLayout(int coordinateValue, int sizeValue)
        {
            var center = new Point(coordinateValue, coordinateValue);
            var size = new Size(sizeValue, sizeValue);

            layout = new CircularCloudLayouter(center, new ArchemedianSpiral());

            layout.Size.Should().BeEquivalentTo(size);
        }

        [TestCase(-1, 1, TestName = "Negative X")]
        [TestCase(1, -1, TestName = "Negative Y")]
        [TestCase(0, 1, TestName = "Zero X")]
        [TestCase(1, 0, TestName = "Zero Y")]
        public void CircularCloudLayouter_GetsOnlyPositiveCenterCoordinates(int x, int y)
        {
            Action makeLayout = () => new CircularCloudLayouter(new Point(x, y), new ArchemedianSpiral());

            makeLayout.Should().Throw<ArgumentException>()
                .WithMessage("Center coordinates values have to be greater than Zero");
        }

        [Test]
        public void PutNextRectangle_ShouldKeepEnteredSize()
        {
            layout = new CircularCloudLayouter(new Point(5, 5), new ArchemedianSpiral());
            var enteredSize = new Size(3, 4);
            var returnedSize = layout.PutNextRectangle(enteredSize).Size;

            returnedSize.Should().BeEquivalentTo(enteredSize);
        }

        [Test]
        public void PutNextRectangle_HasNotEnoughPoints()
        {
            
            layout = new CircularCloudLayouter(new Point(100, 100), new TestPointGenerator());
            var rectangleSizes = new RandomSizeRectangle().GenerateRectangles(5);

            var makeRectangles = () => {
                foreach (var size in rectangleSizes)
                    layout.PutNextRectangle(size);
            };

            makeRectangles.Should().Throw<ArgumentException>()
                .WithMessage("Not Enough Points Generated");
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
                return;

            var image = new Bitmap(layout.Size.Width, layout.Size.Height);
            foreach (var rectangle in layout.Rectangles)
                Drawings.DrawRectangle(image, rectangle);
            
            var fileName = string.Format("{0}.bmp", TestContext.CurrentContext.Test.Name);
            var path = TestContext.CurrentContext.WorkDirectory;
            var fullPath = Path.Combine(path, fileName);
            var message = string.Format("Tag cloud visualization saved to file {0}", fullPath);

            image.Save(fileName);
            Console.WriteLine(message);
        }
    }
}
