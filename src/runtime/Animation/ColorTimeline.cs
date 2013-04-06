/// <summary>
/// ColorTimeline.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using Microsoft.Xna.Framework;
	using Spine.Runtime.MonoGame.Utils;

	public class ColorTimeline : CurveTimeline, ITimeline
	{
		private const int LAST_FRAME_TIME = -5;
		private const int FRAME_R = 1;
		private const int FRAME_G = 2;
		private const int FRAME_B = 3;
		private const int FRAME_A = 4;
		private int slotIndex = -1;
		private readonly float[] frames; // time, r, g, b, a, ...
		
		public ColorTimeline (int keyframeCount) :base (keyframeCount)
		{
			this.frames = new float[keyframeCount * 5];
		}
		
		public float GetDuration ()
		{
			return this.frames [frames.Length - 5];
		}
		
		public int GetKeyframeCount ()
		{
			return this.frames.Length / 5;
		}

		public void SetSlotIndex(int index)
		{
			this.slotIndex = index;
		}

		/** Sets the time and value of the specified keyframe. */
		public void SetKeyframe (int keyframeIndex, float time, float r, float g, float b, float a)
		{
			keyframeIndex *= 5;
			this.frames [keyframeIndex] = time;
			this.frames [keyframeIndex + 1] = r;
			this.frames [keyframeIndex + 2] = g;
			this.frames [keyframeIndex + 3] = b;
			this.frames [keyframeIndex + 4] = a;
		}
		
		public void Apply (Skeleton skeleton, float time, float alpha)
		{
			if (time < this.frames [0])
			{
				return;
			} // Time is before first frame.

			var slot = skeleton.slots [slotIndex];
			
			if (time >= frames [frames.Length - 5])
			{ // Time is after last frame.
				int i = frames.Length - 1;
				float colorRed = frames [i - 3];
				float colorGreen = frames [i - 2];
				float colorBlue = frames [i - 1];
				float colorAlpha = frames [i];
				slot.Color = new Color ((int)colorRed, (int)colorGreen, (int)colorBlue, (int)colorAlpha);
				return;
			}
			
			// Interpolate between the last frame and the current frame.
			int frameIndex = SearchUtils.BinarySearch (frames, time, 5);
			float lastFrameR = this.frames [frameIndex - 4];
			float lastFrameG = this.frames [frameIndex - 3];
			float lastFrameB = this.frames [frameIndex - 2];
			float lastFrameA = this.frames [frameIndex - 1];
			float frameTime = this.frames [frameIndex];
			float percent = MathUtils.Clamp (1 - (time - frameTime) / (frames [frameIndex + LAST_FRAME_TIME] - frameTime), 0, 1);
			percent = GetCurvePercent (frameIndex / 5 - 1, percent);
			
			float r = lastFrameR + (frames [frameIndex + FRAME_R] - lastFrameR) * percent;
			float g = lastFrameG + (frames [frameIndex + FRAME_G] - lastFrameG) * percent;
			float b = lastFrameB + (frames [frameIndex + FRAME_B] - lastFrameB) * percent;
			float a = lastFrameA + (frames [frameIndex + FRAME_A] - lastFrameA) * percent;

			if (alpha < 1)
			{
				slot.Color = new Color (
					MathUtils.Clamp ((int)(slot.Color.R + ((r - slot.Color.R) * alpha)), 0, 255),
	                MathUtils.Clamp ((int)(slot.Color.G + ((g - slot.Color.G) * alpha)), 0, 255),
    	            MathUtils.Clamp ((int)(slot.Color.B + ((b - slot.Color.B) * alpha)), 0, 255),
        	        MathUtils.Clamp ((int)(slot.Color.A + ((a - slot.Color.A) * alpha)), 0, 255));
			}
			else
			{
				slot.Color = new Color ((int)r, (int)g, (int)b, (int)a);
			}
		}
	}
}

