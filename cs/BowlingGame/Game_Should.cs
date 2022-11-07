using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    [TestFixture]
    public class Game_Should : ReportingTest<Game_Should>
    {
        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            new Game()
                .GetScore()
                .Should().Be(0);
        }

        [Test]
        public void HaveScore_AfterFirsRoll()
        {
            var game = new Game();
            game.Roll(4);
            game.GetScore().Should().Be(4);
        }

        [Test]
        public void ThrowArgumentException_WhenShootDownMoreTenPins()
        {
            Action act = () => new Game().Roll(11);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ThrowArgumentException_WhenShootDownNegativeAmount()
        {
            Action act = () => new Game().Roll(-1);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(1, 4, 5)]
        [TestCase(6, 2, 8)]
        public void GetScore_HaveCorrectScore_AfterTwoRolls(
            int firstRoll, int secondRoll, int result)
        {
            var game = new Game();
            game.Roll(firstRoll);
            game.Roll(secondRoll);
            game.GetScore().Should().Be(result);
        }
    }
}
