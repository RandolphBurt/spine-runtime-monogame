/// <summary>
/// AnimationJsonReader.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Json
{
	using System;
	using System.Collections.Generic;
	using System.IO;	
	using System.Runtime.Serialization;
	
	using Microsoft.Xna.Framework;
	
	using Newtonsoft.Json.Linq;

	public class AnimationJsonReader : BaseJsonReader
	{
		private const String TIMELINE_SCALE = "scale";
		private const String TIMELINE_ROTATE = "rotate";
		private const String TIMELINE_TRANSLATE = "translate";
		private const String TIMELINE_ATTACHMENT = "attachment";
		private const String TIMELINE_COLOR = "color";

		public Animation ReadAnimationJsonFile (string jsonFile, SkeletonData skeletonData, float scale = 1)
		{
			if (skeletonData == null)
			{
				throw new ArgumentException ("skeletonData cannot be null.");
			}

			var timelines = new List<ITimeline> ();

			var jsonText = File.ReadAllText (jsonFile);
			JObject data = JObject.Parse (jsonText);

			var boneTimelines = this.ReadBones (skeletonData, data, scale);
			var slotTimelines = this.ReadSlots (skeletonData, data);

			timelines.AddRange (boneTimelines);
			timelines.AddRange (slotTimelines);

			timelines.TrimExcess ();

			float duration = 0;

			foreach (var timeline in timelines)
			{
				var timelineDuration = timeline.getDuration ();
				if (timelineDuration > duration)
				{
					duration = timelineDuration;
				}
			}

			Animation animation = new Animation (timelines, duration);
			animation.setName (Path.GetFileNameWithoutExtension (jsonFile));

			return animation;
		}

		private List<ITimeline> ReadBones (SkeletonData skeletonData, JObject data, float scale)
		{
			List<ITimeline> timelines = new List<ITimeline> ();

			foreach (JProperty bone in data["bones"])
			{
				int boneIndex = skeletonData.findBoneIndex (bone.Name);
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
							timeline = this.ReadRotationTimeline (jsonTimelineList, boneIndex);
							break;
							
						case TIMELINE_TRANSLATE:
							timeline = this.ReadTranslationTimeline (jsonTimelineList, boneIndex, scale);
							break;
							
						case TIMELINE_SCALE:
							timeline = this.ReadScaleTimeline (jsonTimelineList, boneIndex);
							break;
							
						default:
							throw new Exception ("Invalid timeline type for a bone: " + timelineType.Name + " (" + bone.Name + ")");
					}
					
					timelines.Add (timeline);
				}
			}

			return timelines;
		}

		private List<ITimeline> ReadSlots (SkeletonData skeletonData, JObject data)
		{
			List<ITimeline> timelines = new List<ITimeline> ();

			var dataSlots = data["slots"];

			if (dataSlots != null)
			{
				foreach (JProperty jsonSlot in dataSlots)
				{
					int slotIndex = skeletonData.findSlotIndex (jsonSlot.Name);
					
					foreach (JProperty jsonTimelineType in jsonSlot.Value)
					{
						JArray jsonTimelineList = (JArray)jsonTimelineType.Value;

						ITimeline timeline;
						
						switch (jsonTimelineType.Name)
						{
							case TIMELINE_COLOR:
								
								timeline = this.ReadColorTimeline (jsonTimelineList, slotIndex);
								break;
								
							case TIMELINE_ATTACHMENT:
								timeline = this.ReadAttachmentTimeline (jsonTimelineList, slotIndex);
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

		private ITimeline ReadTranslationTimeline (JArray jsonTimelineList, int boneIndex, float scale)
		{
			var translateTimeline = new TranslateTimeline (jsonTimelineList.Count);

			this.PopulateTranslationScaleTimeline (translateTimeline, jsonTimelineList, boneIndex, scale);

			return translateTimeline;
		}

		private ITimeline ReadScaleTimeline (JArray jsonTimelineList, int boneIndex)
		{
			var scaleTimeline = new ScaleTimeline (jsonTimelineList.Count);

			this.PopulateTranslationScaleTimeline (scaleTimeline, jsonTimelineList, boneIndex, 1);

			return scaleTimeline;
		}

		private void PopulateTranslationScaleTimeline (TranslateTimeline translateScaleTimeline, JArray jsonTimelineList, int boneIndex, float timelineScale)
		{
			translateScaleTimeline.setBoneIndex (boneIndex);
			
			int keyframeIndex = 0;
			foreach (JToken jsonTimeline in jsonTimelineList)
			{
				float time = (float)jsonTimeline ["time"];
				float x = this.Read<float> (jsonTimeline, "x", 0);
				float y = this.Read<float> (jsonTimeline, "y", 0);
				translateScaleTimeline.setKeyframe (keyframeIndex, time, x * timelineScale, y * timelineScale);
				this.ReadCurve (translateScaleTimeline, keyframeIndex, jsonTimeline);
				keyframeIndex++;
			}
		}

		private RotateTimeline ReadRotationTimeline (JArray jsonTimelineList, int boneIndex)
		{
			var rotateTimeline = new RotateTimeline (jsonTimelineList.Count);
			rotateTimeline.setBoneIndex (boneIndex);
			
			int keyframeIndex = 0;
			foreach (JToken jsonTimeline in jsonTimelineList)
			{
				float time = (float)jsonTimeline ["time"];
				rotateTimeline.setKeyframe (keyframeIndex, time, (float)jsonTimeline ["angle"]);
				this.ReadCurve (rotateTimeline, keyframeIndex, jsonTimeline);
				keyframeIndex++;
			}

			return rotateTimeline;
		}

		private ColorTimeline ReadColorTimeline (JArray jsonTimelineList, int slotIndex)
		{
			var colorTimeline = new ColorTimeline (jsonTimelineList.Count);
			colorTimeline.setSlotIndex (slotIndex);
			
			int keyframeIndex = 0;
			foreach (JToken timeline in jsonTimelineList)
			{
				float time = (float)timeline ["time"];
				Color color = this.ReadColor ((String)timeline ["color"]);
				colorTimeline.setKeyframe (keyframeIndex, time, color.R, color.G, color.B, color.A);
				this.ReadCurve (colorTimeline, keyframeIndex, timeline);
				keyframeIndex++;
			}

			return colorTimeline;
		}

		private AttachmentTimeline ReadAttachmentTimeline (JArray jsonTimelineList, int slotIndex)
		{
			AttachmentTimeline attachmentTimeline = new AttachmentTimeline (jsonTimelineList.Count);
			attachmentTimeline.setSlotIndex (slotIndex);
			
			int keyframeIndex = 0;
			foreach (JToken timeline in jsonTimelineList)
			{
				float time = (float)timeline ["time"];
				attachmentTimeline.setKeyframe (keyframeIndex++, time, (String)timeline ["name"]);
			}

			return attachmentTimeline;
		}
		
		private void ReadCurve (CurveTimeline timeline, int keyframeIndex, JToken valueMap)
		{
			JToken curveObject = valueMap ["curve"];
			if (curveObject == null)
			{
				return;
			}
			if (curveObject is JArray)
			{
				JArray curve = (JArray)curveObject;
				timeline.setCurve (keyframeIndex, (float)curve [0], (float)curve [1], (float)curve [2], (float)curve [3]);
			}
			else if (curveObject.Value<string> ().Equals ("stepped"))
			{
				timeline.setStepped (keyframeIndex);
			}
		}
	}
}

