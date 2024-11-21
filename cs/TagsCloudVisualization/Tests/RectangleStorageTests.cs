using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class RectangleStorageTests
    {
        private RectangleStorage storage;
        private Rectangle defaulRectangle;
        [SetUp]
        public void SetUp()
        {
            storage = new RectangleStorage();
            defaulRectangle = new(new(2, 2), new(2, 2));
        }

        [Test]
        public void GetRectangles_ShouldGetAllRectangle()
        {
            storage.Add(defaulRectangle);
            storage.Add(defaulRectangle);

            storage.GetAll().Should().HaveCount(2);
        }

        [Test]
        public void AddRectangle_ShouldGetIdForRectangle()
        {
            var id = storage.Add(defaulRectangle);

            storage.GetById(id).Should().Be(defaulRectangle);
        }

        [Test]
        public void ChangeRectangle_ShouldChangeRectangleByIndex()
        {
            var id = storage.Add(defaulRectangle);
            var rectangleForChange = storage.GetById(id);

            rectangleForChange.Size = new Size(1, 1);

            storage.GetById(id).Size.Should().Be(rectangleForChange.Size);
        }
    }
}
