/// <summary>
/// Skeleton.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class Skeleton
	{
		internal readonly SkeletonData data;
		internal readonly List<Bone> bones;
		internal readonly List<Slot> slots;
		internal readonly List<Slot> drawOrder;
		internal Skin skin;
		internal readonly Color color;
		internal float time;
		internal bool flipX, flipY;
		
		public Skeleton (SkeletonData data)
		{
			if (data == null)
			{
				throw new ArgumentException ("data cannot be null.");
			}
			this.data = data;

			bones = new List<Bone> (data.bones.Count);
			foreach (BoneData boneData in data.bones)
			{
				Bone parent = boneData.parent == null ? null : bones [data.bones.IndexOf (boneData.parent)];
				bones.Add (new Bone (boneData, parent));
			}
			
			slots = new List<Slot> (data.slots.Count);
			drawOrder = new List<Slot> (data.slots.Count);
			foreach (SlotData slotData in data.slots)
			{
				Bone bone = bones [data.bones.IndexOf (slotData.boneData)];
				Slot slot = new Slot (slotData, this, bone);
				slots.Add (slot);
				drawOrder.Add (slot);
			}
			
			color = new Color (1, 1, 1, 1);
		}
		
		/** Copy constructor. */
		public Skeleton (Skeleton skeleton)
		{
			if (skeleton == null)
			{
				throw new ArgumentException ("skeleton cannot be null.");
			}
			data = skeleton.data;
			
			bones = new List<Bone> (skeleton.bones.Count);
			foreach (Bone bone in skeleton.bones)
			{
				Bone parent = bones [skeleton.bones.IndexOf (bone.parent)];
				bones.Add (new Bone (bone, parent));
			}
			
			slots = new List<Slot> (skeleton.slots.Count);
			foreach (Slot slot in skeleton.slots)
			{
				Bone bone = bones [skeleton.bones.IndexOf (slot.bone)];
				Slot newSlot = new Slot (slot, this, bone);
				slots.Add (newSlot);
			}
			
			drawOrder = new List<Slot> (slots.Count);
			foreach (Slot slot in skeleton.drawOrder)
				drawOrder.Add (slots [skeleton.slots.IndexOf (slot)]);
			
			skin = skeleton.skin;
			color = skeleton.color;
			time = skeleton.time;
		}
		
		/** Updates the world transform for each bone. */
		public void updateWorldTransform ()
		{
			foreach (var bone in this.bones)
			{
				bone.updateWorldTransform (this.flipX, this.flipY);
			}
		}
		
		/** Sets the bones and slots to their bind pose values. */
		public void setToBindPose ()
		{
			setBonesToBindPose ();
			setSlotsToBindPose ();
		}
		
		public void setBonesToBindPose ()
		{
			foreach (var bone in this.bones)
			{
				bone.setToBindPose ();
			}
		}
		
		public void setSlotsToBindPose ()
		{
			int index = 0;
			foreach (var slot in this.slots)
			{
				slot.setToBindPose (index++);
			}
		}
		
		public void draw (SpriteBatch batch)
		{
			foreach (var drawOrder in this.drawOrder)
			{
				if (drawOrder.attachment != null)
				{
					drawOrder.attachment.draw (batch, drawOrder);
				}
			}
		}

		/*
		public void drawDebug (ShapeRenderer renderer) {
			renderer.setColor(Color.RED);
			renderer.begin(ShapeType.Line);
			for (int i = 0, n = bones.Count; i < n; i++) {
				Bone bone = bones.get(i);
				if (bone.parent == null) continue;
				float x = bone.data.length * bone.m00 + bone.worldX;
				float y = bone.data.length * bone.m10 + bone.worldY;
				renderer.line(bone.worldX, bone.worldY, x, y);
			}
			renderer.end();
			
			renderer.setColor(Color.GREEN);
			renderer.begin(ShapeType.Filled);
			for (int i = 0, n = bones.Count; i < n; i++) {
				Bone bone = bones.get(i);
				renderer.setColor(Color.GREEN);
				renderer.circle(bone.worldX, bone.worldY, 3);
			}
			renderer.end();
		}*/

		
		public SkeletonData getData ()
		{
			return data;
		}
		
		public List<Bone> getBones ()
		{
			return bones;
		}
		
		/** @return May return null. */
		public Bone getRootBone ()
		{
			if (bones.Count == 0)
			{
				return null;
			}
			return bones [0];
		}
		
		/** @return May be null. */
		public Bone findBone (String boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentException ("boneName cannot be null.");
			}

			return this.bones.Find (x => x.data.name.Equals (boneName));
		}
		
		/** @return -1 if the bone was not found. */
		public int findBoneIndex (String boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentException ("boneName cannot be null.");
			}
		
			return this.bones.FindIndex (x => x.data.name.Equals (boneName));
		}
		
		public List<Slot> getSlots ()
		{
			return slots;
		}
		
		/** @return May be null. */
		public Slot findSlot (String slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentException ("slotName cannot be null.");
			}

			return this.slots.Find (x => x.data.name.Equals (slotName));
		}
		
		/** @return -1 if the bone was not found. */
		public int findSlotIndex (String slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentException ("slotName cannot be null.");
			}

			return this.slots.FindIndex (x => x.data.name.Equals (slotName));
		}
		
		/** Returns the slots in the order they will be drawn. The returned array may be modified to change the draw order. */
		public List<Slot> getDrawOrder ()
		{
			return drawOrder;
		}
		
		/** @return May be null. */
		public Skin getSkin ()
		{
			return skin;
		}
		
		/** Sets a skin by name.
	 * @see #setSkin(Skin) */
		public void setSkin (String skinName)
		{
			Skin skin = data.findSkin (skinName);
			if (skin == null)
			{
				throw new ArgumentException ("Skin not found: " + skinName);
			}
			setSkin (skin);
		}
		
		/** Sets the skin used to look up attachments not found in the {@link SkeletonData#getDefaultSkin() default skin}. Attachments
	 * from the new skin are attached if the corresponding attachment from the old skin is currently attached.
	 * @param newSkin May be null. */
		public void setSkin (Skin newSkin)
		{
			if (skin != null && newSkin != null)
			{
				newSkin.attachAll (this, skin);
			}
			skin = newSkin;
		}
		
		/** @return May be null. */
		public Attachment getAttachment (String slotName, String attachmentName)
		{
			return getAttachment (data.findSlotIndex (slotName), attachmentName);
		}
		
		/** @return May be null. */
		public Attachment getAttachment (int slotIndex, String attachmentName)
		{
			if (attachmentName == null)
			{
				throw new ArgumentException ("attachmentName cannot be null.");
			}
			if (skin != null)
			{
				return skin.getAttachment (slotIndex, attachmentName);
			}
			if (data.defaultSkin != null)
			{
				Attachment attachment = data.defaultSkin.getAttachment (slotIndex, attachmentName);
				if (attachment != null)
				{
					return attachment;
				}
			}
			return null;
		}
		
		/** @param attachmentName May be null. */
		public void setAttachment (String slotName, String attachmentName)
		{
			if (slotName == null)
			{
				throw new ArgumentException ("slotName cannot be null.");
			}

			int index = 0;
			foreach (var slot in this.slots)
			{
				if (slot.data.name.Equals (slotName))
				{
					Attachment attachment = null;
					if (attachmentName != null)
					{
						attachment = getAttachment (index, attachmentName);
						if (attachment == null)
						{
							throw new ArgumentException ("Attachment not found: " + attachmentName + ", for slot: " + slotName);
						}
					}
					slot.setAttachment (attachment);
					return;
				}

				index++;
			}
			throw new ArgumentException ("Slot not found: " + slotName);
		}
		
		public Color getColor ()
		{
			return color;
		}
		
		public bool getFlipX ()
		{
			return flipX;
		}
		
		public void setFlipX (bool flipX)
		{
			this.flipX = flipX;
		}
		
		public bool getFlipY ()
		{
			return flipY;
		}
		
		public void setFlipY (bool flipY)
		{
			this.flipY = flipY;
		}
		
		public float getTime ()
		{
			return time;
		}
		
		public void setTime (float time)
		{
			this.time = time;
		}
		
		public void update (float delta)
		{
			time += delta;
		}

		public String toString () 
		{
			return data.name != null ? data.name : base.ToString();
		}
	}
}