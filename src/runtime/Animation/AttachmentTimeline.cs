/// <summary>
/// AttachmentTimeline.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;
	using Spine.Runtime.MonoGame.Utils;

	public class AttachmentTimeline : ITimeline
	{
		private int slotIndex;
		private readonly float[] frames; // time, ...
		private readonly String[] attachmentNames;
		
		public AttachmentTimeline (int keyframeCount)
		{
			frames = new float[keyframeCount];
			attachmentNames = new String[keyframeCount];
		}
		
		public float GetDuration ()
		{
			return this.frames [this.frames.Length - 1];
		}
		
		public int GetKeyframeCount ()
		{
			return frames.Length;
		}

		public void SetSlotIndex(int index)
		{
			this.slotIndex = index;
		}
		
		/** Sets the time and value of the specified keyframe. */
		public void SetKeyframe (int keyframeIndex, float time, String attachmentName)
		{
			this.frames [keyframeIndex] = time;
			this.attachmentNames [keyframeIndex] = attachmentName;
		}
		
		public void Apply (Skeleton skeleton, float time, float alpha)
		{
			if (time < this.frames [0])
			{
				return;
			} // Time is before first frame.
			
			int frameIndex;
			if (time >= this.frames [this.frames.Length - 1])
			{ // Time is after last frame.
				frameIndex = this.frames.Length - 1;
			}
			else
			{
				frameIndex = SearchUtils.BinarySearch (this.frames, time, 1) - 1;
			}
			
			String attachmentName = this.attachmentNames [frameIndex];
			skeleton.slots [slotIndex].SetAttachment (
				attachmentName == null ? null : skeleton.GetAttachment (slotIndex, attachmentName));
		}
	}
}

