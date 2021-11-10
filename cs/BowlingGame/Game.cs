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

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            new Game()
                .GetScore()
                .Should().Be(0);
        }

        [Test]
        public void DoubleTwoNextKnockedPinsCount_AfterStrike()
        {
            var game = new Game();
            game.Roll(10);
            game.Roll(2);
            game.Roll(3);
            game.GetScore().Should().Be(20);
        }
    }
}
