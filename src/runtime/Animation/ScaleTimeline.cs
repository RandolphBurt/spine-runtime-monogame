/// <summary>
/// ScaleTimeline.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using Spine.Runtime.MonoGame.Utils;

	public class ScaleTimeline : TranslateTimeline, ITimeline
	{
		public ScaleTimeline (int keyframeCount) :base(keyframeCount)
		{
		}
		
		public new void Apply (Skeleton skeleton, float time, float alpha)
		{
			if (time < frames [0])
			{
				return;
			} // Time is before first frame.
			
			Bone bone = skeleton.bones [BoneIndex];

			if (time >= frames [frames.Length - 3])
			{ // Time is after last frame.
				bone.scaleX += (bone.Data.ScaleX - 1 + frames [frames.Length - 2] - bone.scaleX) * alpha;
				bone.scaleY += (bone.Data.ScaleY - 1 + frames [frames.Length - 1] - bone.scaleY) * alpha;
				return;
			}
			
			// Interpolate between the last frame and the current frame.
			int frameIndex = SearchUtils.BinarySearch (frames, time, 3);
			float lastFrameX = frames [frameIndex - 2];
			float lastFrameY = frames [frameIndex - 1];
			float frameTime = frames [frameIndex];
			float percent = MathUtils.Clamp (1 - (time - frameTime) / (frames [frameIndex + LAST_FRAME_TIME] - frameTime), 0, 1);
			percent = GetCurvePercent (frameIndex / 3 - 1, percent);
			
			bone.scaleX += (bone.Data.ScaleX - 1 + lastFrameX + (frames [frameIndex + FRAME_X] - lastFrameX) * percent - bone.scaleX)
				* alpha;
			bone.scaleY += (bone.Data.ScaleY - 1 + lastFrameY + (frames [frameIndex + FRAME_Y] - lastFrameY) * percent - bone.scaleY)
				* alpha;
		}
	}

}

