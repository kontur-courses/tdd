using System;
using System.Collections.Generic;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        public int Score { get; private set; } = 0;
        private int[] Frames = new int[10];
        public int CurrentRollsNumber { get; private set; } = 0;
        public int RollsNumber { get; private set; } = 0;
        private int currentFrame = 0;
        private Dictionary<int, List<int>> bonusDictionary = new Dictionary<int, List<int>>();

        public int GetFrame(int number)
        {
            return Frames[number - 1];
        }
        public void Roll(int pins)
        {
            if (bonusDictionary.ContainsKey(RollsNumber))
            {
                foreach (var bonus in bonusDictionary[RollsNumber])
                {
                    Frames[bonus] += pins;
                    Score += pins;
                }
            }
            Score += pins;
            Frames[currentFrame] += pins;
            CurrentRollsNumber++;
            if (pins == 10)
            {
                if (!bonusDictionary.ContainsKey(RollsNumber + 1))
                    bonusDictionary[RollsNumber + 1] = new List<int>() { currentFrame };
                else
                    bonusDictionary[RollsNumber + 1].Add(currentFrame);

                if (!bonusDictionary.ContainsKey(RollsNumber + 2))
                    bonusDictionary[RollsNumber + 2] = new List<int>() { currentFrame };
                else
                    bonusDictionary[RollsNumber + 2].Add(currentFrame);
                CurrentRollsNumber = 0;
                currentFrame++;
            }
            else if (CurrentRollsNumber == 2)
            {
                if (Frames[currentFrame] == 10)
                {
                    if (!bonusDictionary.ContainsKey(RollsNumber + 1))
                        bonusDictionary[RollsNumber + 1] = new List<int>() { currentFrame };
                    else
                        bonusDictionary[RollsNumber + 1].Add(currentFrame);
                }

                CurrentRollsNumber = 0;
                currentFrame++;
            }
            RollsNumber++;
        }

        public int GetScore()
        {
            return Score;
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {


        private Game basicGame;
        private Game CreateGame()
        {
            return new Game();
        }

        [SetUp]
        public void BaseSetUp()
        {
            basicGame = CreateGame();
        }


        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            new Game()
                .GetScore()
                .Should().Be(0);
        }

        [Test]
        public void HaveTrueResultAfterOneRoll()
        {
            basicGame.Roll(1);
            basicGame.GetScore().Should().Be(1);
        }

        [Test]
        public void ReturnScoreForOneFrame()
        {
            basicGame.Roll(3);
            basicGame.Roll(4);
            basicGame.GetFrame(1).Should().Be(7);
        }

        [Test]
        public void ReturnCorretlyScore_AfterSpare()
        {
            basicGame.Roll(7);
            basicGame.Roll(3);
            basicGame.Roll(5);
            basicGame.Score.Should().Be(20);
        }

        [Test]
        public void ReturnCorrectScore_AfterThreeStrikes()
        {
            basicGame.Roll(10);
            basicGame.Roll(10);
            basicGame.Roll(10);
            basicGame.Roll(4);
            basicGame.Roll(3);
            basicGame.Score.Should().Be(78);
        }

        [Test]
        public void ShouldNotChangesScore_When_Rolls0()
        {
            basicGame.Roll(2);
            basicGame.Roll(0);
            basicGame.Score.Should().Be(2);
        }
        [Test]
        public void ShouldNotChangesScoreInCurrentFrame_When_Rolls0()
        {
            basicGame.Roll(2);
            basicGame.Roll(0);
            basicGame.GetFrame(1).Should().Be(2);
        }

        [Test]
        public void ShouldGetCorretlyScore_When_9_11Strikes()
        {
            for(int i=0;i<16;i++)
                basicGame.Roll(0);
            basicGame.Roll(10);
            basicGame.Roll(10);
            basicGame.Roll(10);
        }

    }
}
