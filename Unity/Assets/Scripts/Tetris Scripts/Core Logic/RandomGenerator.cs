using System;

namespace Tetris
{
	/// <summary>
	/// Random generator.
	/// </summary>
	public static class RandomGenerator
	{
		private static Random r;

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static Random Instance 
		{
			get 
			{
				if( r == null )
					r = new Random();
				return r;
			}
		}

		/// <summary>
		/// Sets the seed to generate random numbers off of.
		/// Also creates a new instance if one is not available
		/// </summary>
		/// <param name="seed">Seed.</param>
		public static void SetSeed( int seed )
		{
			r = new Random( seed );
		}
	}
}

