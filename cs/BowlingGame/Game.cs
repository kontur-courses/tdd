using System;
using System.Collections.Generic;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        public int Score;
        private bool isSpare;
        private List<int> strikeCounters = new List<int>();
        private int currentFrame = 1;
        private int frameRolls;
        private int framePins = 10;

        public void Roll(int pins)
        {
            if (currentFrame == 11 && frameRolls == 1)
                throw new ArgumentException();
            if (currentFrame == 11 && !isSpare && !strikeCounters.Contains(2))
                throw new ArgumentException();
            if (pins < 0 || pins > framePins)
                throw new ArgumentException();

            frameRolls++;
            framePins -= pins;
            BonusScoring(pins);
            Score += pins;

            if (framePins == 0)
            {
                switch(frameRolls)
                {
                    case 2:
                        isSpare = true;
                        break;
                    case 1:
                        strikeCounters.Add(2);
                        break;
                }
                NewFrame();
                return;
            }

            if (frameRolls == 2)
            {
                NewFrame();
            }
        }

        private void BonusScoring(int pins)
        {
            if (isSpare)
            {
                Score += pins;
                isSpare = false;
            }
            foreach (var strikeCounter in strikeCounters)
            {

                Score += pins;
                var newCounter = strikeCounter - 1;
                var newCounters = new List<int>();
                if (newCounter > 0)
                    newCounters.Add(newCounter);
                strikeCounters = newCounters;
            }
        }

        private void NewFrame()
        {
            currentFrame++;
            frameRolls = 0;
            framePins = 10;
        }

        public int GetScore()
        {
            return Score;
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
        public void HaveOneScore_AfterRollOnePin()
        {
            var game = new Game();
            game.Roll(1);
            game.GetScore().Should().Be(1);
        }

        [Test]
        public void HaveMaxPins()
        {
            var game = new Game();
            Assert.Throws<ArgumentException>(() => game.Roll(11));
        }

        [Test]
        public void ShouldThrows_WhenNegativeArgument()
        {
            var game = new Game();
            Assert.Throws<ArgumentException>(() => game.Roll(-11));
        }

        [Test]
        public void HaveCorrectFramePins()
        {
            var game = new Game();
            game.Roll(6);
            Assert.Throws<ArgumentException>(() => game.Roll(5));
        }

        [Test]
        public void ShouldCorrectWork_AfterFirstFrame()
        {
            var game = new Game();
            game.Roll(5);
            game.Roll(5);
            Assert.DoesNotThrow(() => game.Roll(2));

        }

        [Test]
        public void ShouldBeThirdRoll_InLastFrame()
        {
            var game = new Game();
            for (var i = 0; i < 9; i++)
            {
                game.Roll(5);
                game.Roll(4);
            }
            game.Roll(10);
            game.Roll(5);
            Assert.Throws<ArgumentException>(() => game.Roll(4));
        }

        [Test]
        public void ShouldAcceptSpare()
        {
            var game = new Game();
            game.Roll(6);
            game.Roll(4);
            game.Roll(5);
            game.Roll(1);
            game.GetScore().Should().Be(21);
        }

        [Test]
        public void ShouldAcceptStrike()
        {
            var game = new Game();
            game.Roll(10);
            game.Roll(5);
            game.Roll(1);
            game.GetScore().Should().Be(22);
        }

        [Test]
        public void ShouldAcceptStrikeAfterStrike()
        {
            var game = new Game();
            game.Roll(10);
            game.Roll(10);
            game.Roll(1);
            game.Roll(2);
            game.GetScore().Should().Be(37);
        }

        [Test]
        public void ShouldBeCorrectMaxScore()
        {
            var game = new Game();
            for (var i = 1; i <= 11; i++)
            {
                game.Roll(10);
            }
            game.GetScore().Should().Be(300);
        }
    }
}
