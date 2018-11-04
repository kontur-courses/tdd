using System;
using System.Collections.Generic;
using System.Xml;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private List<int> rolls = new List<int>();

        public void Roll(int pins)
        {
            if (pins < 0 || pins > 10)
            {
                throw new ArgumentException("pins must be positive and less than 10 or equal");
            }
            rolls.Add(pins);
        }

        public int GetScore()
        {
            int score = 0;
            int result = 0;
            int frameCount = 1;
            int frameScore = 0;
            Bonus bonus = Bonus.None;
            foreach (var r in rolls)
            {
                if (frameCount > 10)
                {
                    return result;
                }
                if (frameScore == 0)
                {
                    frameScore += r;

                    if (bonus != Bonus.None)
                        score += r;

                    if (frameScore == 10)
                    {
                        score += frameScore;
                        frameScore = 0;
                        bonus = Bonus.Strike;
                        
                    }
                }
                else
                {
                    frameScore += r;

                    if (bonus == Bonus.Strike)
                        score += r;


                    //frameNumber++;
                    if (frameScore == 10)
                        bonus = Bonus.Spare;
                    else
                        bonus = Bonus.None;

                    frameCount++;
                    result += frameScore;
                    frameScore = 0;
                }
            }
            score += result + frameScore;
            return score;
        }
    }

    enum Bonus { None, Spare, Strike}

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        [TestCase(11)]
        [TestCase(-1)]
        public void Roll_IncorrectValue_ThrowsException(int pins)
        {
            Assert.Throws<ArgumentException>(() => new Game().Roll(pins));
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(7, ExpectedResult = 7)]
        public int HaveItselfNumber_AfterOneRollTip(int pins)
        {
            var game = new Game();
            game.Roll(pins);
            return game.GetScore();
        }

        [TestCase(new int[] {7, 1}, ExpectedResult = 8)]
        [TestCase(new int[] { 0, 1 }, ExpectedResult = 1)]
        public int HaveSumNumbers_AfterOneRollWithTwoTip(int[] rolls)
        {

            var game = new Game();
            foreach (var roll in rolls)
            {
                game.Roll(roll);
            }
            return game.GetScore();
        }

        [Test]
        public void HaveSumNumbersWithBonus_AfterOneRollWithSpare()
        {

            var game = new Game();
            game.Roll(7);
            game.Roll(3);
            game.Roll(6);
            game.GetScore().Should().Be(22);
        }

        [TestCase(new int[] {10, 5, 4}, ExpectedResult = 28)]
        [TestCase(new int[] {10, 10}, ExpectedResult = 30)]
        public int GetScore_WithStrike_Correct(int[] rolls)
        {
            var game = new Game();
            foreach (var roll in rolls)
            {
                game.Roll(roll);
            }

            return game.GetScore();
        }

        [TestCase(new int[] {1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 5, 5, 7}, ExpectedResult = 27+10+7+7)]
        [TestCase(new int[] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 5, 5, 7, 3 }, ExpectedResult = 27 + 10 + 7 + 7)]
        public int GetScore_TenFrameSpare_Correct(int[] rolls)
        {
            var game = new Game();
            foreach (var roll in rolls)
            {
                game.Roll(roll);
            }

            return game.GetScore();
        }

    }
}
