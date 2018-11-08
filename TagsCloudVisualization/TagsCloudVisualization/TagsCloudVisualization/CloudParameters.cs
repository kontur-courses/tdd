using System;
using TagsCloudVisualization.Curves;

namespace TagsCloudVisualization
{
	class CloudParameters
	{
		public int maxLengthRect;
		public int count;
		public ICurve curve;

		public CloudParameters(ICurve curve, int count, int maxLengthRect)
		{
			this.curve = curve;
			this.count = count;
			this.maxLengthRect = maxLengthRect;
		}

		public static bool IsCorrect(CloudParameters cloudParameters)
		{
			var isGoodParameters = true;

			if (cloudParameters.curve == null)
			{
				Console.WriteLine("Error in the name of the curve");
				isGoodParameters = false;
			}

			if (cloudParameters.count <= 0)
			{
				Console.WriteLine("Count must be positive");
				isGoodParameters = false;
			}

			if (cloudParameters.maxLengthRect <= 0)
			{
				Console.WriteLine("MaxLength must be positive");
				isGoodParameters = false;
			}

			return isGoodParameters;
		}

		public static CloudParameters Parse(string[] input)
		{
			var parameters = new CloudParameters(null, 0, 0);

			for (var i = 0; i < input.Length; i++)
			{
				switch (input[i])
				{
					case "-count":
						int.TryParse(input[i + 1], out parameters.count);
						break;
					case "-maxLength":
						int.TryParse(input[i + 1], out parameters.maxLengthRect);
						break;
					case "-curve":
						parameters.curve = GetCurve(input, i);
						break;
				}
			}

			return parameters;
		}

		private static ICurve GetCurve(string[] args, int position)
		{
			ICurve curve = null;
			switch (args[position + 1])
			{
				case "spiral":
					curve = new Spiral(0.5, Math.PI / 18);
					break;
				case "heart":
					curve = new Heart(0.2, Math.PI / 36);
					break;
				case "astroid":
					curve = new Astroid(0.2, Math.PI / 36);
					break;
			}

			return curve;
		}
	}
}