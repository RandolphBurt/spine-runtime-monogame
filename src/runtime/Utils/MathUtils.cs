/// <summary>
/// MathUtils.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Utils
{
	using System;

	public static class MathUtils
	{
		public static float cosDeg (float degrees)
		{
			return (float)Math.Cos ((double)degrees * Math.PI / 180.0);
		}

		public static float sinDeg (float degrees)
		{
			return (float)Math.Sin ((double)degrees * Math.PI / 180.0);
		}
	}
}

