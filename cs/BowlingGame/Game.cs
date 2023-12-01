using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private int[] score = new int[10];
        public int frame = 1;
        private int count;
        private bool isSpare;
        private bool isStrike;

        public void Roll(int pins)
        {
            if (pins < 0 || pins > 10)
                throw new ArgumentException(
                    $"pins count should be non-negative and less or equal ten, but was {pins}"
                );
            
            count++;

            if (isStrike)
            {
                score[frame - 2] += pins;
                if (count == 2)
                    isStrike = false;
            }
            else if (isSpare)
            {
                score[frame - 2] += pins;
                isSpare = false;
            }
            else score[frame - 1] += pins;

            if (count == 1 && score[frame - 1] == 10)
            {
                isStrike = true;
                ChangeFrame();
                return;
            }

            
            if (count == 2 && score[frame - 1] == 10)
                isSpare = true;
            if (score[frame - 1] > 10)
                throw new ArgumentException(
                    $"score in one frame should be less or equal ten, but was {score[frame - 1]}"
                );

            if (count == 2)
                ChangeFrame();
        }

        private void ChangeFrame()
        {
            frame++;
            count = 0;
        }
        
        public int GetScore()
        {
            return score[frame - 1];
        }
        
        public int GetScore(int frame)
        {
            return score[frame - 1];
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
        public void HaveScore_AfterAnyRoll()
        {
            var game = new Game();
            game.Roll(5);

            game.GetScore().Should().Be(5);
        }

        [Test]
        public void Spare()
        {
            var game = new Game();

            int frame = 1;
            
            game.Roll(5);
            game.Roll(5);
            
            game.Roll(5);

            game.GetScore(frame).Should().Be(15);
        }

        [Test]
        public void NewFrame_AfterStrike()
        {
            var game = new Game();
            
            game.Roll(10);

            game.frame.Should().Be(2);
        }
        
        [Test]
        public void BonusScore_InNextFrame_AfterStrike()
        {
            var game = new Game();
            
            game.Roll(10);
            game.Roll(2);
            game.Roll(2);

            game.GetScore(1).Should().Be(14);
        }
        
        [TestCase(-5)]
        [TestCase(99)]
        public void Fails_OnNegativeOrGreaterThan10(int pin)
        {
            var game = new Game();
            Action action = () => game.Roll(pin);

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Fails_OnPinsGreaterThanTenInOneFrame()
        {
            var game = new Game();
            Action action = () =>
            {
                game.Roll(5);
                game.Roll(7);
            };

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(2, new[] { 0, 0 }, 2)]
        public void FrameValue_AfterRolls(int rollsCount, int[] rollsValues, int expected)
        {
            var game = new Game();
            for (var i = 0; i < rollsCount; i++)
            {
                game.Roll(rollsValues[i]);
            }

            game.frame.Should().Be(expected);
        }
        
        // [TestCase(7, new[] { 10, 2, 2, 5, 5, 3, 0 }, 34)]
        // public void ScoreValue_AfterRolls(int rollsCount, int[] rollsValues, int expected)
        // {
        //     var game = new Game();
        //     for (var i = 0; i < rollsCount; i++)
        //     {
        //         game.Roll(rollsValues[i]);
        //     }
        //
        //     game.GetScore().Should().Be(expected);
        // }
    }
}