using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        public void Roll(int pins)
        {
        }

        public int GetScore()
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {
        
    }
}
