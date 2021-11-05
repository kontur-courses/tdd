using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    internal class TagsPainterTests
    {
        private TagsPainter _sut = new();
        private CircularCloudLayouter _layouter = new();

        [SetUp]
        public void SetUp()
        {
            _layouter = new CircularCloudLayouter();
        }

        [Test]
        public void Constructor_ThrowArgumentException_WhenOffsetInvalid()
        {
            Action action = () => new TagsPainter(-1, -1);

            action.Should().Throw<ArgumentException>().WithMessage("Offsets must be great or equal to zero");
        }

        [Test]
        public void SaveToFile_ThrowArgumentException_WhenLayoutEmpty()
        {
            var filepath = $"{Environment.CurrentDirectory}\\failedLayout.png";

            Action action = () => _sut.SaveToFile(filepath, _layouter.GetLayout());

            action.Should().Throw<ArgumentException>().WithMessage("Impossible to save an empty layout");
        }

        [Test]
        public void SaveToFile_ShouldCreateFile()
        {
            _layouter.GenerateRandomLayout(1000);
            var filepath = $"{Environment.CurrentDirectory}\\outputTestLayout.png";
            if (File.Exists(filepath))
                File.Delete(filepath);

            _sut.SaveToFile(filepath, _layouter.GetLayout());

            File.Exists(filepath).Should().BeTrue();
        }
    }
}