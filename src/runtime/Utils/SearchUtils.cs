/// <summary>
/// SearchUtil.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Utils
{
	public static class SearchUtils
	{
		/** @param target After the first and before the last entry. */
		public static int BinarySearch (float[] values, float target, int step)
		{
			int low = 0;
			int high = values.Length / step - 2;
			if (high == 0)
			{
				return step;
			}

			int current = high >> 1;
			while (true)
			{
				if (values [(current + 1) * step] <= target)
				{
					low = current + 1;
				}
				else
				{
					high = current;
				}

				if (low == high)
				{
					return (low + 1) * step;
				}

				current = (low + high) >> 1;
			}
		}
		
		public static int LinearSearch (float[] values, float target, int step)
		{
			for (int i = 0, last = values.Length - step; i <= last; i += step)
			{
				if (values [i] <= target)
				{
					continue;
				}

				return i;
			}

			return -1;
		}
	}
}

