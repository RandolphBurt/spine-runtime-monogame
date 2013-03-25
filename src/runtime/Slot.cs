/// <summary>
/// Slot.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;
	using Microsoft.Xna.Framework;

	using Spine.Runtime.MonoGame.Attachments;

	public class Slot
	{
		private Attachment attachment;
		
		private float attachmentTime;

		public Slot (SlotData data, Skeleton skeleton, Bone bone)
		{
			if (data == null)
			{
				throw new ArgumentException ("data cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentException ("skeleton cannot be null.");
			}
			if (bone == null)
			{
				throw new ArgumentException ("bone cannot be null.");
			}
			this.Data = data;
			this.Skeleton = skeleton;
			this.Bone = bone;
			this.Color = new Color (1, 1, 1, 1);
			this.SetToBindPose ();
		}

		public SlotData Data 
		{
			get;
			set;
		}
		
		public  Bone Bone
		{
			get;
			set;
		}
		
		public Skeleton Skeleton
		{
			get;
			set;
		}
		
		public Color Color
		{
			get;
			set;
		}
		
		public Attachment Attachment
		{
			get
			{
				return this.attachment;
			}
			
			private set
			{
				this.attachment = value;
			}
		}
		
		/** Copy constructor. */
		public Slot (Slot slot, Skeleton skeleton, Bone bone)
		{
			if (slot == null)
			{
				throw new ArgumentException ("slot cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentException ("skeleton cannot be null.");
			}
			if (bone == null)
			{
				throw new ArgumentException ("bone cannot be null.");
			}

			Data = slot.Data;
			this.Skeleton = skeleton;
			this.Bone = bone;
			Color = slot.Color;
			attachment = slot.attachment;
			attachmentTime = slot.attachmentTime;
		}
		
		/** Sets the attachment and resets {@link #getAttachmentTime()}.
	 * @param attachment May be null. */
		public void SetAttachment (Attachment attachment)
		{
			// TODO - compare with corona
			this.Attachment = attachment;
			this.attachmentTime = this.Skeleton.time;
		}
		
		public void SetAttachmentTime (float time)
		{
			this.attachmentTime = this.Skeleton.time - time;
		}
		
		/** Returns the time since the attachment was set. */
		public float GetAttachmentTime ()
		{
			return this.Skeleton.time - this.attachmentTime;
		}
		
		public void SetToBindPose ()
		{
			this.Color = Data.Color;

			this.SetAttachment (Data.AttachmentName == null ? null : this.Skeleton.GetAttachment (this.Data.Name, this.Data.AttachmentName));
		}
	}
}