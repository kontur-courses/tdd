using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Tdd
{
	public class Frame
	{
	}

	public class Game
	{
		public void Roll(int pins)
		{
			
		}

		public IList<Frame> GetFrames()
		{
			throw new NotImplementedException();
		}

		public int Score {  get { throw new NotImplementedException(); } }
	}


	[TestFixture]
	public class BowlingGame_should
	{
		[Test]
		public void HaveEmptyFrame_BeforeAnyRolls()
		{
			//TODO
		}
	}
}
