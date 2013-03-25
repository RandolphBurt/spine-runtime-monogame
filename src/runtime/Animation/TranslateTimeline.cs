/// <summary>
/// TranslateTimeline.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using Spine.Runtime.MonoGame.Utils;

	public class TranslateTimeline : CurveTimeline, ITimeline
	{
		protected const int LAST_FRAME_TIME = -3;
		protected const int FRAME_X = 1;
		protected const int FRAME_Y = 2;
		internal readonly float[] frames; // time, value, value, ...
		
		public TranslateTimeline (int keyframeCount) : base(keyframeCount)
		{
			frames = new float[keyframeCount * 3];
		}

		public int BoneIndex
		{
			get;
			set;
		}

		public float GetDuration ()
		{
			return frames [frames.Length - 3];
		}
		
		public int GetKeyframeCount ()
		{
			return frames.Length / 3;
		}
		
		/** Sets the time and value of the specified keyframe. */
		public void SetKeyframe (int keyframeIndex, float time, float x, float y)
		{
			keyframeIndex *= 3;
			frames [keyframeIndex] = time;
			frames [keyframeIndex + 1] = x;
			frames [keyframeIndex + 2] = y;
		}
		
		public void Apply (Skeleton skeleton, float time, float alpha)
		{
			if (time < this.frames [0])
			{
				return;
			} // Time is before first frame.
			
			Bone bone = skeleton.bones [BoneIndex];
			
			if (time >= frames [frames.Length - 3])
			{ // Time is after last frame.
				bone.X += (bone.Data.X + frames [frames.Length - 2] - bone.X) * alpha;
				bone.Y += (bone.Data.Y + frames [frames.Length - 1] - bone.Y) * alpha;
				return;
			}
			
			// Interpolate between the last frame and the current frame.
			int frameIndex = SearchUtils.BinarySearch (frames, time, 3);
			float lastFrameX = frames [frameIndex - 2];
			float lastFrameY = frames [frameIndex - 1];
			float frameTime = frames [frameIndex];
			float percent = MathUtils.Clamp (1 - (time - frameTime) / (frames [frameIndex + LAST_FRAME_TIME] - frameTime), 0, 1);
			percent = GetCurvePercent (frameIndex / 3 - 1, percent);
			
			bone.X += (bone.Data.X + lastFrameX + (frames [frameIndex + FRAME_X] - lastFrameX) * percent - bone.X) * alpha;
			bone.Y += (bone.Data.Y + lastFrameY + (frames [frameIndex + FRAME_Y] - lastFrameY) * percent - bone.Y) * alpha;
		}
	}
}

