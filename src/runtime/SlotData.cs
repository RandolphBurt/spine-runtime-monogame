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
		private Color color = new Color (1f, 1f, 1f, 1f);

		public String Name
		{
			get;
			set;
		}

		public BoneData BoneData {
			get;
			set;
		}

		public Color Color 
		{
			get
			{
				return this.color;
			}

			set
			{
				this.color = value;
			}
		}

		public String AttachmentName
		{
			get;
			set;
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

			this.Name = name;
			this.BoneData = boneData;
		}
	}
}