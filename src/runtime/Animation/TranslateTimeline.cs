/// <summary>
/// TranslateTimeline.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using Spine.Runtime.MonoGame.Utils;

	public class TranslateTimeline : CurveTimeline, ITimeline {
		protected const int LAST_FRAME_TIME = -3;
		protected const int FRAME_X = 1;
		protected const int FRAME_Y = 2;
		
		internal int boneIndex;
		internal readonly float[] frames; // time, value, value, ...
		
		public TranslateTimeline (int keyframeCount) : base(keyframeCount) {
			frames = new float[keyframeCount * 3];
		}
		
		public float getDuration () {
			return frames[frames.Length - 3];
		}
		
		public int getKeyframeCount () {
			return frames.Length / 3;
		}
		
		public void setBoneIndex (int boneIndex) {
			this.boneIndex = boneIndex;
		}
		
		public int getBoneIndex () {
			return boneIndex;
		}
		
		public float[] getKeyframes () {
			return frames;
		}
		
		/** Sets the time and value of the specified keyframe. */
		public void setKeyframe (int keyframeIndex, float time, float x, float y) {
			keyframeIndex *= 3;
			frames[keyframeIndex] = time;
			frames[keyframeIndex + 1] = x;
			frames[keyframeIndex + 2] = y;
		}
		
		public void apply (Skeleton skeleton, float time, float alpha) {
			float[] frames = this.frames;
			if (time < frames[0]) return; // Time is before first frame.
			
			Bone bone = skeleton.bones[boneIndex];
			
			if (time >= frames[frames.Length - 3]) { // Time is after last frame.
				bone.x += (bone.data.x + frames[frames.Length - 2] - bone.x) * alpha;
				bone.y += (bone.data.y + frames[frames.Length - 1] - bone.y) * alpha;
				return;
			}
			
			// Interpolate between the last frame and the current frame.
			int frameIndex = SearchUtils.BinarySearch(frames, time, 3);
			float lastFrameX = frames[frameIndex - 2];
			float lastFrameY = frames[frameIndex - 1];
			float frameTime = frames[frameIndex];
			float percent = MathUtils.Clamp(1 - (time - frameTime) / (frames[frameIndex + LAST_FRAME_TIME] - frameTime), 0, 1);
			percent = getCurvePercent(frameIndex / 3 - 1, percent);
			
			bone.x += (bone.data.x + lastFrameX + (frames[frameIndex + FRAME_X] - lastFrameX) * percent - bone.x) * alpha;
			bone.y += (bone.data.y + lastFrameY + (frames[frameIndex + FRAME_Y] - lastFrameY) * percent - bone.y) * alpha;
		}
	}
}

