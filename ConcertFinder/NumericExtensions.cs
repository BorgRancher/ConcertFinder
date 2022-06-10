using System;
namespace StubHub
{
	public static class NumericExtensions
	{
		// Converts degrees to radians for Haversine
		public static double ToRadians(this double val)
		{
			return (Math.PI / 180) * val;
		}
	}
}

