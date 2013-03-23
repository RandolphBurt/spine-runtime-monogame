/// <summary>
/// SlotData.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;
	using Microsoft.Xna.Framework;

	public class SlotData
	{
		internal readonly String name;
		internal readonly BoneData boneData;
		internal Color color = new Color (1, 1, 1, 1);
		internal String attachmentName;
		
		SlotData ()
		{
			name = null;
			boneData = null;
		}
		
		public SlotData (String name, BoneData boneData)
		{
			if (name == null)
			{
				throw new ArgumentException ("name cannot be null.");
			}

			if (boneData == null)
			{
				throw new ArgumentException ("boneData cannot be null.");
			}

			this.name = name;
			this.boneData = boneData;
		}
		
		public String getName ()
		{
			return name;
		}
		
		public BoneData getBoneData ()
		{
			return boneData;
		}
		
		public Color getColor ()
		{
			return color;
		}
		
		/** @param attachmentName May be null. */
		public void setAttachmentName (String attachmentName)
		{
			this.attachmentName = attachmentName;
		}
		
		/** @return May be null. */
		public String getAttachmentName ()
		{
			return attachmentName;
		}
		
		public String toString ()
		{
			return name;
		}
	}
}