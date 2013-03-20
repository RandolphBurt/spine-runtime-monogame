/// <summary>
/// MathUtils.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Utils
{
	using System;

	public static class MathUtils
	{
		public static float CosDeg (float degrees)
		{
			return (float)Math.Cos ((double)degrees * Math.PI / 180.0);
		}

		public static float SinDeg (float degrees)
		{
			return (float)Math.Sin ((double)degrees * Math.PI / 180.0);
		}

		public static float Clamp (float value, float min, float max)
		{
			if (value < min)
			{
				return min;
			}

			if (value > max)
			{
				return max;
			}

			return value;
		}
	}
}

