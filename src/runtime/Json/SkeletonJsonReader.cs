/// <summary>
/// SkeletonJsonReader.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Json
{
	using System;
	using System.IO;	
	using System.Runtime.Serialization;

	using Newtonsoft.Json.Linq;

	using Spine.Runtime.MonoGame.Attachments;
	using Spine.Runtime.MonoGame.Graphics;

	public class SkeletonJsonReader : BaseJsonReader
	{
		private readonly IAttachmentLoader attachmentLoader;

		public SkeletonJsonReader (TextureAtlas atlas)
		{
			this.attachmentLoader = new TextureAtlasAttachmentLoader (atlas);
		}

		public SkeletonJsonReader (IAttachmentLoader attachmentLoader)
		{
			this.attachmentLoader = attachmentLoader;
		}

		public Skeleton ReadSkeletonJsonFile (string jsonFile, float scale = 1)
		{
			SkeletonData skeletonData = new SkeletonData (Path.GetFileNameWithoutExtension (jsonFile));

			var jsonText = File.ReadAllText (jsonFile);
			JObject data = JObject.Parse (jsonText);

			this.ReadSkeletonBones (skeletonData, data, scale);
			this.ReadSkeletonSlots (skeletonData, data);
			this.ReadSkeletonSkins (skeletonData, data, scale);

			skeletonData.Bones.TrimExcess ();
			skeletonData.Slots.TrimExcess ();
			skeletonData.Skins.TrimExcess ();

			return new Skeleton (skeletonData);
		}

		private void ReadSkeletonSkins (SkeletonData skeletonData, JObject data, float scale)
		{
			foreach (JProperty skin in data["skins"])
			{
				Skin skeletonSkin = new Skin (skin.Name);
				
				foreach (JProperty slot in skin.Value)
				{
					int slotIndex = skeletonData.FindSlotIndex (slot.Name);
					
					foreach (JProperty attachment in slot.Value)
					{
						Attachment skeletonAttachment = this.ReadSkeletonAttachment (attachment.Name, attachment.Value, scale);
						if (attachment != null)
						{
							skeletonSkin.AddAttachment (slotIndex, attachment.Name, skeletonAttachment);
						}
					}
				}
				
				skeletonData.AddSkin (skeletonSkin);
				if (skeletonSkin.Name.Equals ("default"))
				{
					skeletonData.DefaultSkin = skeletonSkin;
				}
			}
		}
		
		private Attachment ReadSkeletonAttachment (String name, JToken map, float scale)
		{
			string attachmentName = this.Read (map, "name", name);

			var attachmentType = AttachmentType.region;

			var attachmentRegionType = (String)map ["type"];
			if (attachmentRegionType != null && String.Compare (attachmentRegionType, "regionSequence", true) == 0)
			{
				attachmentType = AttachmentType.regionSequence;
			}

			Attachment attachment = this.attachmentLoader.NewAttachment (attachmentType, attachmentName);
			
			if (attachment is RegionSequenceAttachment)
			{
				RegionSequenceAttachment regionSequenceAttachment = (RegionSequenceAttachment)attachment;

				var fps = map ["fps"];
				if (fps == null)
				{
					throw new SerializationException ("Region sequence attachment missing fps: " + attachmentName);
				}

				regionSequenceAttachment.SetFrameTime ((float)fps);
				
				regionSequenceAttachment.SetMode (RegionSequenceModeConvert.FromString ((String)map ["mode"]));
			}
			
			if (attachment is RegionAttachment)
			{
				RegionAttachment regionAttachment = (RegionAttachment)attachment;
				regionAttachment.X = this.Read<float> (map, "x", 0) * scale;
				regionAttachment.Y = this.Read<float> (map, "y", 0) * scale;
				regionAttachment.ScaleX = this.Read<float> (map, "scaleX", 1);
				regionAttachment.ScaleY = this.Read<float> (map, "scaleY", 1);
				regionAttachment.Rotation = this.Read<float> (map, "rotation", 0);
				regionAttachment.Width = this.Read<float> (map, "width", 32) * scale;
				regionAttachment.Height = this.Read<float> (map, "height", 32) * scale;
			}
			
			return attachment;
		}

		private void ReadSkeletonSlots (SkeletonData skeletonData, JObject data)
		{
			foreach (var slot in data["slots"].Children())
			{
				String slotName = (String)slot ["name"];
				String boneName = (String)slot ["bone"];
				BoneData boneData = skeletonData.FindBone (boneName);
				if (boneData == null)
				{
					throw new SerializationException ("Slot bone not found: " + boneName);
				}

				SlotData slotData = new SlotData (slotName, boneData);
				
				String color = (String)slot ["color"];

				if (color != null)
				{
					slotData.Color = this.ReadColor (color);
				}

				slotData.AttachmentName = (String)slot ["attachment"];
				
				skeletonData.AddSlot (slotData);
			}
		}

		private void ReadSkeletonBones (SkeletonData skeletonData, JObject data, float scale)
		{
			foreach (var bone in data["bones"].Children())
			{
				BoneData parent = null;
				var parentName = (string)bone ["parent"];
				
				if (parentName != null)
				{
					parent = skeletonData.FindBone (parentName);
					if (parent == null)
					{
						throw new SerializationException ("Parent bone not found: " + parentName);
					}
				}

				BoneData boneData = new BoneData ((String)bone ["name"], parent);
				boneData.Length = this.Read<float> (bone, "length", 0) * scale;
				boneData.X = this.Read<float> (bone, "x", 0) * scale;
				boneData.Y = this.Read<float> (bone, "y", 0) * scale;
				boneData.Rotation = this.Read<float> (bone, "rotation", 0);
				boneData.ScaleX = this.Read<float> (bone, "scaleX", 1);
				boneData.ScaleY = this.Read<float> (bone, "scaleY", 1);
				skeletonData.AddBone (boneData);
			}
		}
	}
}

