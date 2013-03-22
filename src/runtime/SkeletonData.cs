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
		internal string name;
		internal readonly List<BoneData> bones = new List<BoneData> (); // Ordered parents first.
		internal readonly List<SlotData> slots = new List<SlotData> (); // Bind pose draw order.
		internal readonly List<Skin> skins = new List<Skin> ();
		internal Skin defaultSkin;
		
		public void clear ()
		{
			bones.Clear ();
			slots.Clear ();
			skins.Clear ();
			defaultSkin = null;
		}
		
		// --- Bones.
		
		public void addBone (BoneData bone)
		{
			if (bone == null)
			{
				throw new ArgumentException ("bone cannot be null.");
			}
			bones.Add (bone);
		}
		
		public List<BoneData> getBones ()
		{
			return bones;
		}
		
		/** @return May be null. */
		public BoneData findBone (String boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentException ("boneName cannot be null.");
			}

			return this.bones.Find (x => x.name.Equals (boneName));
		}
		
		/** @return -1 if the bone was not found. */
		public int findBoneIndex (String boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentException ("boneName cannot be null.");
			}

			return this.bones.FindIndex (x => x.name.Equals (boneName));
		}
		
		// --- Slots.
		
		public void addSlot (SlotData slot)
		{
			if (slot == null)
			{
				throw new ArgumentException ("slot cannot be null.");
			}
			slots.Add (slot);
		}
		
		public List<SlotData> getSlots ()
		{
			return slots;
		}
		
		/** @return May be null. */
		public SlotData findSlot (String slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentException ("slotName cannot be null.");
			}

			return this.slots.Find (x => x.name.Equals (slotName));
		}
		
		/** @return -1 if the bone was not found. */
		public int findSlotIndex (String slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentException ("slotName cannot be null.");
			}

			return this.slots.FindIndex (x => x.name.Equals (slotName));
		}
		
		// --- Skins.
		
		/** @return May be null. */
		public Skin getDefaultSkin ()
		{
			return defaultSkin;
		}
		
		/** @param defaultSkin May be null. */
		public void setDefaultSkin (Skin defaultSkin)
		{
			this.defaultSkin = defaultSkin;
		}
		
		public void addSkin (Skin skin)
		{
			if (skin == null)
			{
				throw new ArgumentException ("skin cannot be null.");
			}
			skins.Add (skin);
		}
		
		/** @return May be null. */
		public Skin findSkin (String skinName)
		{
			if (skinName == null)
			{
				throw new ArgumentException ("skinName cannot be null.");
			}
			foreach (Skin skin in skins)
				if (skin.name.Equals (skinName))
				{
					return skin;
				}
			return null;
		}
		
		/** Returns all skins, including the default skin. */
		public List<Skin> getSkins ()
		{
			return skins;
		}

		/** @return May be null. */
		public String getName ()
		{
			return name;
		}
		
		/** @param name May be null. */
		public void setName (String name)
		{
			this.name = name;
		}
		
		public String toString ()
		{
			return name != null ? name : base.ToString ();
		}
	}
}