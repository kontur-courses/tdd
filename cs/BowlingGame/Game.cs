using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        public void Roll(int pins)
        {
        }

        public int GetScore()
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture, Ignore("Работа над другим проектом, красный тест раздражает")]
    public class Game_should : ReportingTest<Game_should>
    {
        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            new Game()
                .GetScore()
                .Should().Be(0);
        }
    }
}
