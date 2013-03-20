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
		
		public float getDuration ()
		{
			return frames [frames.Length - 1];
		}
		
		public int getKeyframeCount ()
		{
			return frames.Length;
		}
		
		public int getSlotIndex ()
		{
			return slotIndex;
		}
		
		public void setSlotIndex (int slotIndex)
		{
			this.slotIndex = slotIndex;
		}
		
		public float[] getKeyframes ()
		{
			return frames;
		}
		
		public String[] getAttachmentNames ()
		{
			return attachmentNames;
		}
		
		/** Sets the time and value of the specified keyframe. */
		public void setKeyframe (int keyframeIndex, float time, String attachmentName)
		{
			frames [keyframeIndex] = time;
			attachmentNames [keyframeIndex] = attachmentName;
		}
		
		public void apply (Skeleton skeleton, float time, float alpha)
		{
			float[] frames = this.frames;
			if (time < frames [0])
			{
				return;
			} // Time is before first frame.
			
			int frameIndex;
			if (time >= frames [frames.Length - 1])
			{ // Time is after last frame.
				frameIndex = frames.Length - 1;
			}
			else
			{
				frameIndex = SearchUtils.BinarySearch (frames, time, 1) - 1;
			}
			
			String attachmentName = attachmentNames [frameIndex];
			skeleton.slots [slotIndex].setAttachment (
				attachmentName == null ? null : skeleton.getAttachment (slotIndex, attachmentName));
		}
	}
}

