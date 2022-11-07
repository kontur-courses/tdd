using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace BowlingGame
{
    public struct Frame
    {
        public int score;
        public bool isSpare;
        public bool isStrike;

        public Frame(int score, bool isSpare, bool isStrike)
        {
            this.score = score;
            this.isSpare = isSpare;
            this.isStrike = isStrike;
        }
    }
    
    public class Game
    {
        private int _score = 0;
        private int _countFrame = 0;
        private Frame frame;

        public int CountFrame => _countFrame;

        public void PlayGame(int countFrame, List<(int FirstRoll, int SecondRoll)> pinsShotDownInFrame)
        {
            for (int i = 0; i < countFrame; i++)
            {
                frame = new Frame(0, false, false);
                Roll(pinsShotDownInFrame[i].SecondRoll);
                Roll(pinsShotDownInFrame[i].FirstRoll);
                if(pinsShotDownInFrame[i].SecondRoll < 10)
                    Roll(pinsShotDownInFrame[i].SecondRoll);
            }
        }
        
        public void Roll(int pins)
        {
            if (pins > 10)
                throw new ArgumentException();
            if (pins == 10)
                frame.score += 10;
            _score += pins;
        }

        public int GetScore()
        {
            return _score;
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _game = new Game();
        }
        
        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            _game
                .GetScore()
                .Should().Be(0);
        }

        [TestCase(5, 5,  TestName = ": Check score after first roll")]
        [TestCase(10, 10)]
        public void CheckScore(int pins, int result)
        {
            _game.Roll(pins);
            _game.GetScore().Should().Be(result);
        }
        
        [TestCase(11, TestName = ": Shot down more than 10")]
        public void CheckScore_ShouldThrowArgumentException(int pins)
        {
            Action action = () => _game.Roll(pins);
            action.Should().Throw<ArgumentException>();
        }

        // [TestCase(6, 1,TestName = ": Check count roll after shot down less than 10 pins")]
        // public void CountRoll(int pins, int result)
        // {
        //     _game.Roll(pins);
        //     _game.CountRoll.Should().Be(result);
        // }
        
        [TestCase(10, 1, TestName = ": Check count frame after shot down 10 pins")]
        [TestCase(6, 0, TestName = ": Check count frame after shot down less than 10 pins")]
        public void CountFrame(int pins, int resultFrame)
        {
            _game.Roll(pins);
            _game.CountFrame.Should().Be(resultFrame);
        }
    }
}
