/// <summary>
/// SkeletonData.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;
	using System.Collections.Generic;

	public class SkeletonData
	{
		private readonly List<BoneData> bones = new List<BoneData> (); // Ordered parents first.
		private readonly List<SlotData> slots = new List<SlotData> (); // Bind pose draw order.
		private readonly List<Skin> skins = new List<Skin> ();
		private readonly List<Animation> animations = new List<Animation> ();

		public SkeletonData(String name)
		{
			this.Name = name;
		}

		public string Name
		{
			get;
			private set;
		}

		public List<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		public List<SlotData> Slots
		{
			get
			{
				return this.slots;
			}
		}

		public List<Skin> Skins
		{
			get
			{
				return this.skins;
			}
		}

		public Skin DefaultSkin
		{
			get;
			set;
		}

		public void AddAnimation(Animation animation)
		{
			if (animation == null)
			{
				throw new ArgumentException ("animation cannot be null.");
			}

			this.animations.Add(animation);
		}

		public void AddBone (BoneData bone)
		{
			if (bone == null)
			{
				throw new ArgumentException ("bone cannot be null.");
			}

			this.bones.Add (bone);
		}

		/** @return May be null. */
		public Animation FindAnimation (String animationName)
		{
			if (animationName == null)
			{
				throw new ArgumentException ("animationName cannot be null.");
			}
			
			return this.animations.Find (x => x.Name.Equals (animationName));
		}

		/** @return May be null. */
		public BoneData FindBone (String boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentException ("boneName cannot be null.");
			}

			return this.bones.Find (x => x.Name.Equals (boneName));
		}
		
		/** @return -1 if the bone was not found. */
		public int FindBoneIndex (String boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentException ("boneName cannot be null.");
			}

			return this.bones.FindIndex (x => x.Name.Equals (boneName));
		}

		
		public void AddSlot (SlotData slot)
		{
			if (slot == null)
			{
				throw new ArgumentException ("slot cannot be null.");
			}

			this.slots.Add (slot);
		}

		/** @return May be null. */
		public SlotData FindSlot (String slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentException ("slotName cannot be null.");
			}

			return this.slots.Find (x => x.Name.Equals (slotName));
		}
		
		/** @return -1 if the bone was not found. */
		public int FindSlotIndex (String slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentException ("slotName cannot be null.");
			}

			return this.slots.FindIndex (x => x.Name.Equals (slotName));
		}

				
		public void AddSkin (Skin skin)
		{
			if (skin == null)
			{
				throw new ArgumentException ("skin cannot be null.");
			}

			this.skins.Add (skin);
		}

		/** @return May be null. */
		public Skin FindSkin (String skinName)
		{
			if (skinName == null)
			{
				throw new ArgumentException ("skinName cannot be null.");
			}

			return this.skins.Find (x => x.Name.Equals (skinName));
		}
	}
}







