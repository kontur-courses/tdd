using System;
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
	public class Game_should
	{
		[Test]
		public void HaveEmptyFrame_BeforeAnyRolls()
		{
			//TODO
		}
	}
}
