using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    class ArchimedeanSpiralTests
    {

        [Test]
        public void GetNextPoint_ShouldReturnCenterPoint_WhenFirstCall()
        {
            var center = new Point(5,5);
            var spiral = new ArchimedeanSpiral(center);
            spiral.GetNextPoint().Should().BeEquivalentTo(center);
        }

        /*
         * Не могу понять как корректно тестировать
         * Сделать НОРМАЛЬНЫЕ тесты, которые реально что-то проверяют.
         * А то класс слишком простой.
         */
    }
}
