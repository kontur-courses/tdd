using System;
using TagsCloudVisualization.Curves;

namespace TagsCloudVisualization
{
	public class CloudParameters
	{
		public static CloudParametersDTO Parse(string[] input)
		{
			var parameters = new CloudParametersDTO();
			var count = 0;
			var maxLengthRect = 0;

			for (var i = 0; i < input.Length; i++)
			{
				switch (input[i])
				{
					case "-count":
						int.TryParse(input[i + 1], out count);
						break;
					case "-maxLength":
						int.TryParse(input[i + 1], out maxLengthRect);
						break;
					case "-curve":
						parameters.Curve = GetCurve(input, i);
						break;
				}
			}

			parameters.Count = count;
			parameters.MaxLengthRect = maxLengthRect;
			return parameters;
		}

		private static ICurve GetCurve(string[] args, int position)
		{
			ICurve curve = null;
			switch (args[position + 1])
			{
				case "spiral":
					curve = new Spiral(0.2, Math.PI / 36);
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