using System;
using System.Collections.Generic;
using System.Linq;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private int score;
        public Frame[] Frames = new Frame[10];
        private int currentFrameIndex;
        private Frame spareFrame;
        private Queue<Frame> strikeFrames = new Queue<Frame>();

        public Game()
        {
            for (var i = 0; i < 10; i++)
                Frames[i] = new Frame();
        }
        
        public void Roll(int pins)
        {
            score += pins;
            var currentFrame = Frames[currentFrameIndex];
            currentFrame.ScoreReceivedDuringFrame += pins;
            var framePins = strikeFrames.Any() || spareFrame != null ? pins * 2 : pins;
            currentFrame.Score = Frames[currentFrameIndex > 0 ? currentFrameIndex - 1 : 0].Score + framePins;
            currentFrame.CurrentRoll++;
            if (strikeFrames.Any())
            {
                foreach (var frame in strikeFrames.Where(frame => frame.StrikeBonusCount != 2))
                {
                    frame.ScoreReceivedDuringFrame += pins;
                    frame.Score += pins;
                    score += pins;
                    frame.StrikeBonusCount++;
                }

                if (strikeFrames.Peek().StrikeBonusCount == 2)
                    strikeFrames.Dequeue();
            }
            if (spareFrame != null)
            {
                spareFrame.Score += currentFrame.ScoreReceivedDuringFrame;
                score += pins;
                spareFrame = null;
            }

            if (IsSpare(currentFrame))
                spareFrame = currentFrame;
            if (IsStrike(currentFrame))
                strikeFrames.Enqueue(currentFrame);
            
            if (currentFrame.CurrentRoll == 2 || currentFrame.ScoreReceivedDuringFrame == 10)
                currentFrameIndex++;
        }

        private bool IsSpare(Frame frame)
        {
            return frame.CurrentRoll > 1 && frame.ScoreReceivedDuringFrame == 10;
        }
        
        private bool IsStrike(Frame frame)
        {
            return frame.CurrentRoll == 1 && frame.ScoreReceivedDuringFrame == 10;
        }

        public int GetScore()
        {
            return score;
        }
    }

    public class Frame
    {
        public int Score { get; set; }
        public int ScoreReceivedDuringFrame { get; set; }
        public int CurrentRoll { get; set; }
        public int StrikeBonusCount { get; set; }
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
        public void Have5Score_WhenHit5Pins()
        {
            var game = new Game();
            game.Roll(5);
            game.GetScore().Should().Be(5);
        }

        [Test]
        public void FirstFrameScoreShouldBeGreaterThan20_WhenSpare()
        {
            game.Roll(3);
            game.Roll(7);
            game.Roll(5);
            game.Frames[0].Score.Should().Be(15);
            
        }

        private Game game;
        [SetUp]
        public void SetUp()
        {
            game = new Game();
        }

        [Test]
        public void FirstFrameScoreShouldBeGreaterThan10_WhenStrike()
        {
            game.Roll(10);
            game.Roll(5);
            game.Roll(5);
            game.Frames[0].Score.Should().Be(20);
        }

        [Test]
        public void NextStrikeDoesNotReplacePreviousOne()
        {
            game.Roll(10);
            game.Roll(10);
            game.Roll(5);
            game.Roll(3);
            game.Frames[0].Score.Should().Be(25);
        }

        [Test]
        public void ScoreZero_WhenRollsZeroPins()
        {
            game.Roll(0);
            game.GetScore().Should().Be(0);
        }

        [Test]
        public void CorrectScore_AfterStrike()
        {
            game.Roll(10);
            game.Roll(5);
            game.Roll(3);
            game.GetScore().Should().Be(26);
        }
        
        [Test]
        public void CorrectScore_AfterSpare()
        {
            game.Roll(4);
            game.Roll(6);
            game.Roll(3);
            game.GetScore().Should().Be(16);
        }

        [Test]
        public void SecondFrameScoreCorrect_WhenDoubleStrike()
        {
            game.Roll(10);
            game.Roll(10);
            game.Roll(5);
            game.Roll(3);
            game.Frames[1].Score.Should().Be(28);
        }

        [Test]
        public void ScoreCorrect_AfterStrikeAfterSpare()
        {
            game.Roll(10);//10 (5)(5)
            game.Roll(5);//10 + 10
            game.Roll(5);//20 + 10 (5)
            game.Roll(5);//30 + 10
            game.GetScore().Should().Be(40);
        }

        [Test]
        public void TotalScoreShouldBeEqualToLastFrameScore()
        {
            game.Roll(10);
            game.Roll(5);
            game.Roll(4);
            game.GetScore().Should().Be(game.Frames[1].Score);
        }
    }
}
