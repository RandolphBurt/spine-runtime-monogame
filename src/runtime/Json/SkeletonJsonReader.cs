/// <summary>
/// SkeletonJsonReader.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Json
{
	using System;
	using System.Collections.Generic;
	using System.IO;	
	using System.Runtime.Serialization;

	using Newtonsoft.Json.Linq;

	using Microsoft.Xna.Framework;

	using Spine.Runtime.MonoGame.Attachments;
	using Spine.Runtime.MonoGame.Graphics;

	public class SkeletonJsonReader : BaseJsonReader
	{
		private const String TIMELINE_SCALE = "scale";
		private const String TIMELINE_ROTATE = "rotate";
		private const String TIMELINE_TRANSLATE = "translate";
		private const String TIMELINE_ATTACHMENT = "attachment";
		private const String TIMELINE_COLOR = "color";

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
			this.ReadAnimations (skeletonData, data, scale);

			skeletonData.Bones.TrimExcess ();
			skeletonData.Slots.TrimExcess ();
			skeletonData.Skins.TrimExcess ();

			return new Skeleton (skeletonData);
		}

		private void ReadAnimations (SkeletonData skeletonData, JObject data, float scale)
		{
			foreach (JProperty animation in data["animations"])
			{
				Animation skeletonAnimation = this.ReadAnimation(skeletonData, (JObject) animation.Value, scale);
				skeletonAnimation.Name = animation.Name;
				skeletonData.AddAnimation(skeletonAnimation);
			}
		}

		private Animation ReadAnimation (SkeletonData skeletonData, JObject animationData, float scale)
		{
			var timelines = new List<ITimeline> ();
			
			var boneTimelines = this.ReadAnimationBones (skeletonData, animationData, scale);
			var slotTimelines = this.ReadAnimationSlots (skeletonData, animationData);
			
			timelines.AddRange (boneTimelines);
			timelines.AddRange (slotTimelines);
			
			timelines.TrimExcess ();
			
			float duration = 0;
			
			foreach (var timeline in timelines)
			{
				var timelineDuration = timeline.GetDuration ();
				if (timelineDuration > duration)
				{
					duration = timelineDuration;
				}
			}
			
			return new Animation (timelines, duration);
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

		
		private List<ITimeline> ReadAnimationBones (SkeletonData skeletonData, JObject data, float scale)
		{
			List<ITimeline> timelines = new List<ITimeline> ();
			
			foreach (JProperty bone in data["bones"])
			{
				int boneIndex = skeletonData.FindBoneIndex (bone.Name);
				if (boneIndex == -1)
				{
					throw new SerializationException ("Bone not found: " + bone.Name);
				}
				
				foreach (JProperty timelineType in bone.Value)
				{
					JArray jsonTimelineList = (JArray)timelineType.Value;
					
					ITimeline timeline;
					
					switch (timelineType.Name)
					{
						case TIMELINE_ROTATE:
							timeline = this.ReadAnimationRotationTimeline (jsonTimelineList, boneIndex);
							break;
							
						case TIMELINE_TRANSLATE:
							timeline = this.ReadAnimationTranslationTimeline (jsonTimelineList, boneIndex, scale);
							break;
							
						case TIMELINE_SCALE:
							timeline = this.ReadAnimationScaleTimeline (jsonTimelineList, boneIndex);
							break;
							
						default:
							throw new Exception ("Invalid timeline type for a bone: " + timelineType.Name + " (" + bone.Name + ")");
					}
					
					timelines.Add (timeline);
				}
			}
			
			return timelines;
		}
		
		private List<ITimeline> ReadAnimationSlots (SkeletonData skeletonData, JObject data)
		{
			List<ITimeline> timelines = new List<ITimeline> ();
			
			var dataSlots = data ["slots"];
			
			if (dataSlots != null)
			{
				foreach (JProperty jsonSlot in dataSlots)
				{
					int slotIndex = skeletonData.FindSlotIndex (jsonSlot.Name);
					
					foreach (JProperty jsonTimelineType in jsonSlot.Value)
					{
						JArray jsonTimelineList = (JArray)jsonTimelineType.Value;
						
						ITimeline timeline;
						
						switch (jsonTimelineType.Name)
						{
							case TIMELINE_COLOR:
								
								timeline = this.ReadAnimationColorTimeline (jsonTimelineList, slotIndex);
								break;
								
							case TIMELINE_ATTACHMENT:
								timeline = this.ReadAnimationAttachmentTimeline (jsonTimelineList, slotIndex);
								break;
								
							default:
								throw new Exception ("Invalid timeline type for a slot: " + jsonTimelineType.Name + " (" + jsonSlot.Name + ")");
						}
						
						timelines.Add (timeline);
					}
				}	
			}
			
			return timelines;
		}
		
		private ITimeline ReadAnimationTranslationTimeline (JArray jsonTimelineList, int boneIndex, float scale)
		{
			var translateTimeline = new TranslateTimeline (jsonTimelineList.Count);
			
			this.PopulateAnimationTranslationScaleTimeline (translateTimeline, jsonTimelineList, boneIndex, scale);
			
			return translateTimeline;
		}
		
		private ITimeline ReadAnimationScaleTimeline (JArray jsonTimelineList, int boneIndex)
		{
			var scaleTimeline = new ScaleTimeline (jsonTimelineList.Count);
			
			this.PopulateAnimationTranslationScaleTimeline (scaleTimeline, jsonTimelineList, boneIndex, 1);
			
			return scaleTimeline;
		}
		
		private void PopulateAnimationTranslationScaleTimeline (TranslateTimeline translateScaleTimeline, JArray jsonTimelineList, int boneIndex, float timelineScale)
		{
			translateScaleTimeline.BoneIndex = boneIndex;
			
			int keyframeIndex = 0;
			foreach (JToken jsonTimeline in jsonTimelineList)
			{
				float time = (float)jsonTimeline ["time"];
				float x = this.Read<float> (jsonTimeline, "x", 0);
				float y = this.Read<float> (jsonTimeline, "y", 0);
				translateScaleTimeline.SetKeyframe (keyframeIndex, time, x * timelineScale, y * timelineScale);
				this.ReadAnimationCurve (translateScaleTimeline, keyframeIndex, jsonTimeline);
				keyframeIndex++;
			}
		}
		
		private RotateTimeline ReadAnimationRotationTimeline (JArray jsonTimelineList, int boneIndex)
		{
			var rotateTimeline = new RotateTimeline (jsonTimelineList.Count);
			rotateTimeline.SetBoneIndex (boneIndex);
			
			int keyframeIndex = 0;
			foreach (JToken jsonTimeline in jsonTimelineList)
			{
				float time = (float)jsonTimeline ["time"];
				rotateTimeline.SetKeyframe (keyframeIndex, time, (float)jsonTimeline ["angle"]);
				this.ReadAnimationCurve (rotateTimeline, keyframeIndex, jsonTimeline);
				keyframeIndex++;
			}
			
			return rotateTimeline;
		}
		
		private ColorTimeline ReadAnimationColorTimeline (JArray jsonTimelineList, int slotIndex)
		{
			var colorTimeline = new ColorTimeline (jsonTimelineList.Count);
			colorTimeline.SetSlotIndex (slotIndex);
			
			int keyframeIndex = 0;
			foreach (JToken timeline in jsonTimelineList)
			{
				float time = (float)timeline ["time"];
				Color color = this.ReadColor ((String)timeline ["color"]);
				colorTimeline.SetKeyframe (keyframeIndex, time, color.R, color.G, color.B, color.A);
				this.ReadAnimationCurve (colorTimeline, keyframeIndex, timeline);
				keyframeIndex++;
			}
			
			return colorTimeline;
		}
		
		private AttachmentTimeline ReadAnimationAttachmentTimeline (JArray jsonTimelineList, int slotIndex)
		{
			AttachmentTimeline attachmentTimeline = new AttachmentTimeline (jsonTimelineList.Count);
			attachmentTimeline.SetSlotIndex (slotIndex);
			
			int keyframeIndex = 0;
			foreach (JToken timeline in jsonTimelineList)
			{
				float time = (float)timeline ["time"];
				attachmentTimeline.SetKeyframe (keyframeIndex++, time, (String)timeline ["name"]);
			}
			
			return attachmentTimeline;
		}
		
		private void ReadAnimationCurve (CurveTimeline timeline, int keyframeIndex, JToken valueMap)
		{
			JToken curveObject = valueMap ["curve"];
			if (curveObject == null)
			{
				return;
			}
			if (curveObject is JArray)
			{
				JArray curve = (JArray)curveObject;
				timeline.SetCurve (keyframeIndex, (float)curve [0], (float)curve [1], (float)curve [2], (float)curve [3]);
			}
			else
				if (curveObject.Value<string> ().Equals ("stepped"))
			{
				timeline.SetStepped (keyframeIndex);
			}
		}
	}
}

