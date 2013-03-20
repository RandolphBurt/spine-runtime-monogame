/// <summary>
/// RotateTimeline.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using Spine.Runtime.MonoGame.Utils;

	public class RotateTimeline : CurveTimeline, ITimeline
	{
		private const int LAST_FRAME_TIME = -2;
		private const int FRAME_VALUE = 1;
		private int boneIndex;
		private readonly float[] frames; // time, value, ...
		
		public RotateTimeline (int keyframeCount) : base(keyframeCount)
		{
			this.frames = new float[keyframeCount * 2];
		}
		
		public float getDuration ()
		{
			return frames [frames.Length - 2];
		}
		
		public int getKeyframeCount ()
		{
			return frames.Length / 2;
		}
		
		public void setBoneIndex (int boneIndex)
		{
			this.boneIndex = boneIndex;
		}
		
		public int getBoneIndex ()
		{
			return boneIndex;
		}
		
		public float[] getKeyframes ()
		{
			return frames;
		}
		
		/** Sets the time and value of the specified keyframe. */
		public void setKeyframe (int keyframeIndex, float time, float value)
		{
			keyframeIndex *= 2;
			frames [keyframeIndex] = time;
			frames [keyframeIndex + 1] = value;
		}
		
		public void apply (Skeleton skeleton, float time, float alpha)
		{
			float[] frames = this.frames;
			if (time < frames [0])
			{
				return;
			} // Time is before first frame.
			
			Bone bone = skeleton.bones [boneIndex];
			
			if (time >= frames [frames.Length - 2])
			{ // Time is after last frame.
				float rotationAmount = bone.data.rotation + frames [frames.Length - 1] - bone.rotation;
				while (rotationAmount > 180)
				{
					rotationAmount -= 360;
				}

				while (rotationAmount < -180)
				{
					rotationAmount += 360;
				}

				bone.rotation += rotationAmount * alpha;
				return;
			}
			
			// Interpolate between the last frame and the current frame.
			int frameIndex = SearchUtils.BinarySearch (frames, time, 2);
			float lastFrameValue = frames [frameIndex - 1];
			float frameTime = frames [frameIndex];
			float percent = MathUtils.Clamp (1 - (time - frameTime) / (frames [frameIndex + LAST_FRAME_TIME] - frameTime), 0, 1);
			percent = getCurvePercent (frameIndex / 2 - 1, percent);
			
			float amount = frames [frameIndex + FRAME_VALUE] - lastFrameValue;
			while (amount > 180)
			{
				amount -= 360;
			}

			while (amount < -180)
			{
				amount += 360;
			}

			amount = bone.data.rotation + (lastFrameValue + amount * percent) - bone.rotation;
			while (amount > 180)
			{
				amount -= 360;
			}

			while (amount < -180)
			{
				amount += 360;
			}

			bone.rotation += amount * alpha;
		}
	}
}