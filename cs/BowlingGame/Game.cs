using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Frame
    {
        private int NumberOfRolls = 0;
        private int TotalFrameSum = 0;

        public bool IsStrike()
        {
            return NumberOfRolls == 1 && TotalFrameSum == 10;
        }

        public bool IsSpare()
        {
            return NumberOfRolls == 2 && TotalFrameSum == 10;
        }

        public void Roll(int pins)
        {
            if (pins > 10 || pins < 0)
                throw new ArgumentException();
            if (NumberOfRolls == 1 && TotalFrameSum + pins > 10)
                throw new ArgumentException();

            NumberOfRolls++;
            TotalFrameSum += pins;
        }
    }

    public class Game
    {
        private int totalScore { get; set; }
        private Frame currentFrame = new Frame();
        private Frame previousFrame = new Frame();
        private Queue<int> MultipliyerQueue;
        private bool lastWasSpare = false;
        private bool lastWasStrike = false;

        public Game()
        {
            MultipliyerQueue = new Queue<int>();
            MultipliyerQueue.Enqueue(1);
            MultipliyerQueue.Enqueue(1);
            totalScore = 0;
        }

        public void Roll(int pins)
        {
            lastWasSpare = previousFrame.IsSpare();
            lastWasSpare = previousFrame.IsStrike();
            currentFrame.Roll(pins);

        }

        public int GetScore()
        {
            return totalScore;
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        private Game game;

        [SetUp]
        public void Init()
        {
            game = new Game();
        }


        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            game
                .GetScore()
                .Should().Be(0);
        }

        [Test]
        public void HaveNotZeroScore_AfterSuccessRoll()
        {
            var game = new Game();

            game.Roll(7);

            game.GetScore().Should().Be(7);
        }

        [Test]
        public void DobleRollTest()
        {
            game.Roll(4);
            game.Roll(4);
            game.GetScore().Should().Be(8);
        }

        [TestCase(11)]
        [TestCase(-1)]
        public void ThrowException_RollNotValid(int pin)
        {
            Action act = () => game.Roll(pin);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void OneFrameResultMustBeLessThan11()
        {
            Action act = () =>
            {
                game.Roll(5);
                game.Roll(7);
            };
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void DoubleRollScore_PreviousSpare()
        {
            game.Roll(5);
            game.Roll(5);
            game.Roll(5);

            game.GetScore().Should().Be(20);
        }

        [Test]
        public void StrikeCorrectnessTest()
        {
            game.Roll(5);
            game.Roll(4);
            game.Roll(10);
            game.Roll(3);
            game.Roll(3);
            game.GetScore().Should().Be(31);
        }
    }
}
