using System;
using System.Collections.Generic;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    class Frame
    {
        public int CurrentRoll;
        public int FrameScore;
        public int BonusType;
    }

    public class Game
    {
        private List<Frame> Frames;
        private int Score = 0;
        private bool BonusLevels = false;

        public Game()
        {
            Frames = new List<Frame>() { new Frame() };            
        }

        //public void Roll(int pins)
        //{
        //    if (CurrentFrame == 9 && IsFrameStarted && bonusType == 1)
        //    {
        //        Score += pins;
        //        CurrentFrame = 10;
        //        return;
        //    }

        //    if (CurrentFrame == 10)
        //        throw new ArgumentException("Game over!");

        //    CurrentFrameScore += pins;
        //    if (bonusType > 0 && CurrentFrame < 9)
        //    {
        //        Score += pins;
        //        bonusType--;
        //    }

        //    if (CurrentFrameScore > 10)
        //        throw new ArgumentException();
        //    Score += pins;
        //    if (IsFrameStarted)
        //    {
        //        if (CurrentFrameScore == 10)
        //        {
        //            bonusType = 1;
        //        }
        //        if (CurrentFrame < 9)
        //        {
        //            CurrentFrame++;
        //            CurrentFrameScore = 0;
        //            IsFrameStarted = false;
        //        }
        //    }
        //    else
        //    {
        //        if (pins == 10)
        //        {
        //            bonusType = 2;
        //            CurrentFrameScore = 0;
        //            CurrentFrame++;
        //            return;
        //        }
        //        IsFrameStarted = true;       
        //    }
        //}

        public void Roll(int pins)
        {
            var curFrame = Frames[Frames.Count - 1];
            curFrame.FrameScore += pins;

            if (curFrame.FrameScore > 10)
                throw new ArgumentException();

            var tempBonus = false;
            for (int i = 0; i < Frames.Count - 1; i++)
            {
                if (Frames[i].BonusType > 0)
                {
                    Score += pins;
                    Frames[i].BonusType--;
                    tempBonus = Frames[i].BonusType > 0;
                }
            }

            if (BonusLevels)
            {
                BonusLevels = BonusLevels && tempBonus;
                return;
            }

            if (Frames.Count > 10)
                throw new ArgumentException("Game over!");

            Score += pins;

            if (curFrame.FrameScore == 10)
            {
                if (Frames.Count == 10)
                    BonusLevels = true;

                curFrame.BonusType++; //spare
                if (pins == 10) //strike
                    curFrame.BonusType++;
            }

            curFrame.CurrentRoll++;

            if (curFrame.CurrentRoll == 2 || curFrame.BonusType > 0)
            {
                Frames.Add(new Frame());
            }
        }

        public void Roll(IEnumerable<int> rolls)
        {
            foreach (var e in rolls)
                Roll(e);
        }

        public int GetScore()
        {
            return Score;
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        private Game game;

        [SetUp]
        public void SetUp()
        {
            game = new Game();
        }

        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            game.GetScore().Should().Be(0);
        }

        [Test]
        public void ScoreAdd_AfterRoll_EqualsKeglCount()
        {
            game.Roll(4);
            game.GetScore().Should().Be(4);
        }

        [Test]
        public void Roll_Throws_IfAddMoreThanTenKegles()
        {
            Action action = () =>
            {
                game.Roll(4);
                game.Roll(7);
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void BonusSpare_IfTwoRollsIsTen()
        {
            game.Roll(4);
            game.Roll(6);
            game.Roll(3);
            game.GetScore().Should().Be(16);
        }

        [Test]
        public void BonusStrike_IfTenKegglesInOneRoll()
        {
            game.Roll(new[] { 10, 4, 3 });
            game.GetScore().Should().Be(24);
        }

        [Test]
        public void Roll_Throws_IfMoreThanTenFrames()
        {
            Action act = () =>
            {
                game.Roll(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 });
            };
            act.ShouldThrow<ArgumentException>().WithMessage("Game over!");
        }

        [Test]
        public void BonusRoll_IfSpareInLastFrame()
        {
            game.Roll(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7, 3, 6 });

            game.GetScore().Should().Be(34);
        }

        [Test]
        public void BonusRolls_IfStrikeInLastFrame()
        {
            game.Roll(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 10, 3, 6 });

            game.GetScore().Should().Be(37);
        }

        [Test]
        public void BonusRolls_AfterTwoStrike()
        {
            game.Roll(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 10, 10, 3, 6 });

            game.GetScore().Should().Be(58);
        }


        [Test]
        public void Roll_Throws_IfMoreThanTenFramesWithBonus()
        {
            game.Roll(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 10, 3, 6 });

            game.GetScore().Should().Be(37);
        }
    }
}
