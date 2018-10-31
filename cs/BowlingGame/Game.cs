using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private int frameScore = 0;
        private int beatsCounter = 0;
        private int score = 0;
        private bool wasSpare = false;
        private bool wasStrike = false;
        private int rollsAfterStrike = 0;
        private int frameCounter = 1;

        public void Roll(int pins)
        {
            if (frameCounter > 10)
                throw new InvalidOperationException();
            if (frameScore + pins > 10 || pins < 0)
                throw new ArgumentException();


            if (wasSpare || wasStrike)
            {
                score += pins;
            }

            score += pins;
            frameScore += pins;
            beatsCounter++;

            UpdateStrikeAndSpare(pins);
            if (2 == beatsCounter)
            {
                UpdateFrame();
            }
        }

        private void UpdateStrikeAndSpare(int pins)
        {
            rollsAfterStrike++;

            if (rollsAfterStrike > 2 || pins == 10)
                wasStrike = pins == 10;

            wasSpare = frameScore == 10 && pins != 10;

            if (pins == 10)
            {
                UpdateFrame();
            }
        }

        private void UpdateFrame()
        {
            if (frameScore == 10 && frameCounter == 10)
                return;
            frameCounter++;
            beatsCounter = 0;
            frameScore = 0;
        }

        public int GetScore()
        {
            return score;
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {

        [TestCase(new int[0], 0, TestName = "HaveZeroScore_BeforeAnyRolls")]
        [TestCase(new[] { 5 }, 5, TestName = "HaveRollScore_AfterOneRolls")]
        [TestCase(new[] { 5, 4 }, 9, TestName = "HaveSumRollsScore_AfterTwoRolls")]
        [TestCase(new[] { 5, 5, 6 }, 22, TestName = "HaveBonusScore_AfterSpareFrame")]
        [TestCase(new[] { 5, 5, 6, 3 }, 25, TestName = "HaveNotBonusScore_AfterTwoPinsInFrameAfterSpare")]
        [TestCase(new[] { 10, 2 }, 14, TestName = "HaveBonus AfterStrike")]
        [TestCase(new[] { 10, 2, 2 }, 18, TestName = "HaveDoubleBonus AfterStrike")]
        [TestCase(new[] { 10, 2, 2, 2 }, 20, TestName = "HaveNotBonusInNextNextFrame AfterStrike")]
        [TestCase(new[] { 10, 2, 8, 4, 1 }, 39, TestName = "HaveBonus AfterStrikeAndFrame")]
        [TestCase(new[] { 10, 10, 10, 2, 3 }, 72, TestName = "CorrectScore AfterMultipleStrike")]
        public void HaveRollsSum_AfterSomeRolls(IEnumerable<int> rolls, int result)
        {
            var game = new Game();
            foreach (var roll in rolls)
            {
                game.Roll(roll);
            }
            game.GetScore()
                .Should().Be(result);
        }

        [TestCase(new[] { 5, 8 })]
        [TestCase(new[] { 11 })]
        [TestCase(new[] { -1 })]
        public void ThrowArgumentException(IEnumerable<int> rolls)
        {
            var game = new Game();
            Action beats = () =>
            {
                foreach (var roll in rolls)
                {
                    game.Roll(roll);
                }
            };
            beats.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ShouldNotThrows_WhenMoreThen10PinsBeatIn2frames()
        {
            var game = new Game();
            Action beats = () =>
            {
                game.Roll(5);
                game.Roll(3);
                game.Roll(3);
            };

            beats.ShouldNotThrow();
        }

        [Test]
        public void ThrowInvalidOperationException_AfterNumberOfFramesMoreThen10()
        {
            var rolls = new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 4, 5, 10 };
            var game = new Game();
            Action beats = () =>
            {
                foreach (var roll in rolls)
                {
                    game.Roll(roll);
                }
            };
            beats.ShouldThrow<InvalidOperationException>();
        }
        [TestCase(new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10})]
        [TestCase(new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 4, 6, 10 })]
        public void ShouldNotThrowInvalidOperationException_AfterMultipleBitInLastFrame(IEnumerable<int> rolls)
        {
            var game = new Game();
            Action beats = () =>
            {
                foreach (var roll in rolls)
                {
                    game.Roll(roll);
                }
            };
            beats.ShouldNotThrow<InvalidOperationException>();
        }

    }
}