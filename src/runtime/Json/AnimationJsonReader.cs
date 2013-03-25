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

			var boneTimelines = this.ReadAnimationBones (skeletonData, data, scale);
			var slotTimelines = this.ReadAnimationSlots (skeletonData, data);

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

			Animation animation = new Animation (timelines, duration);
			animation.Name = Path.GetFileNameWithoutExtension (jsonFile);

			return animation;
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

