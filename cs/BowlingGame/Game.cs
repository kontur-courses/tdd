using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private int score = 0;

        public int FrameNumber { get; private set; } = 0;
        public int AttemptNumber { get; private set; } = 0;
        public int LastRollPinsCount { get; private set; } = 0;
        public int BonusRatio { get; private set; } = 1;
        private bool BonusNextNext = false;

        private Frame[] _frames = new Frame[10];

        public void Roll(int pins)
        {
            if (pins > 10  || pins < 0 || LastRollPinsCount + pins > 10)
                throw new ArgumentException();
            score += pins * BonusRatio;
            BonusRatio -= BonusRatio == 1 ? 0 : 1;
            AttemptNumber += 1;
            LastRollPinsCount = pins;
            BonusRatio += BonusNextNext ? 1 : 0;
            BonusNextNext = false;
            if (AttemptNumber == 2 || pins == 10)
            {
                if (pins == 10)
                {
                    BonusNextNext = true;
                    BonusRatio += 1;
                }
                AttemptNumber = 0;
                FrameNumber++;
                LastRollPinsCount = 0;
            }
        }

        public int GetScore()
        {
            return score;
        }
    }

    public class Frame
    {
        public int Score { get; private set; }
        public int FirstAttempt { get; private set; } = 0;
        public int SecondAttempt { get; private set; } = 0;
        public int ThirdAttempt { get; private set; } = 0;

        public bool IsStrike => FirstAttempt == 10;
        public bool IsSpare => FirstAttempt + SecondAttempt == 10;
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        private Game game;
        [SetUp]
        public void CreteGame()
        {
            game = new Game();
        }
        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            new Game()
                .GetScore()
                .Should().Be(0);
        }
        
        [Test]
        public void HaveScoreEqualToRollPins_AfterOneRoll()
        {
            game.Roll(7);
            
            game
                .GetScore()
                .Should().Be(7);
        }
        
        [Test]
        public void HaveScoreEqualToSumRollsPins_AfterTwoRolls()
        {
            game.Roll(7);
            game.Roll(2);
            
            game
                .GetScore()
                .Should().Be(9);
        }
        
        [Test]
        public void FrameNumberIsOne_BeforeAnyRolls()
        {
            game
                .FrameNumber
                .Should().Be(0);
        }
        
        [Test]
        public void AttemptNumberIsZero_BeforeAnyRolls()
        {
            game
                .AttemptNumber
                .Should().Be(0);
        }
        
        [Test]
        public void AttemptNumberIsOne_AfterOneRollWithPinsLessThanTen()
        {
            game.Roll(5);

            game
                .AttemptNumber
                .Should().Be(1);
        }
        
        [Test]
        public void AttemptNumberIsZero_AfterTwoRollWithPinsLessThanTen()
        {
            game.Roll(5);
            game.Roll(2);
            game
                .AttemptNumber
                .Should().Be(0);
        }
        
        [Test]
        public void FrameNumberIsTwo_AfterTwoRollWithPinsLessThanTen()
        {
            game.Roll(5);
            game.Roll(2);
            game
                .FrameNumber
                .Should().Be(1);
        }
        
        [Test]
        public void RollThrowsArgumentException_IfPinsCountBiggerThanTen()
        {
            Action action = () => game.Roll(11);
            action.ShouldThrow<ArgumentException>();
        }
        
        [Test]
        public void RollThrowsArgumentException_IfPinsCountLessThanZero()
        {
            Action action = () => game.Roll(-1);
            action.ShouldThrow<ArgumentException>();
        }
        
        [Test]
        public void RollThrowsArgumentException_IfSecondRollAndPinsCountBiggerThanTenMinusFirstRollPins()
        {
            Action action = () =>
            {
                game.Roll(2);
                game.Roll(9);
            };
            action.ShouldThrow<ArgumentException>();
        }
        
        [Test]
        public void LastRollPinsCountIsEqualToZero_IfNextFrame()
        {
            game.Roll(2);
            game.Roll(7);

            game.LastRollPinsCount.Should().Be(0);
        }
        
        [Test]
        public void FrameNumberInc_AfterStrike()
        {
            game.Roll(10);
            game.FrameNumber.Should().Be(1);
        }
        
        [Test]
        public void GetScoreContainsStrikeBonus_AfterNextTwoRolls()
        {
            game.Roll(10);
            game.Roll(5);
            game.Roll(2);
            game.GetScore().Should().Be(24);
        }
        
        [Test]
        public void GetScoreContainsDoubleStrikeBonus_AfterTwoStrikesAndRoll()
        {
            game.Roll(10);
            game.Roll(10);
            game.Roll(3);
            game.GetScore().Should().Be(39);
        }
        
        [Test]
        public void GetScoreContainsEqual300_AfterTwelveStrikes()
        {
            for (var i = 0; i < 12; i++) game.Roll(10);
            game.GetScore().Should().Be(300);
        }    
                
    }
}
