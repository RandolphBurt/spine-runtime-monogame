/// <summary>
/// Animation.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;
	using System.Collections.Generic;

	public class Animation : IEquatable<Animation>
	{
		private readonly List<ITimeline> timelines;

		public  float Duration
		{
			get;
			set;
		}
	
		public String Name
		{
			get;
			set;
		}

		public List<ITimeline> Timelines 
		{
			get
			{
				return this.timelines;
			}
		}

		public Animation (List<ITimeline> timelines, float duration)
		{
			if (timelines == null)
			{
				throw new ArgumentException ("timelines cannot be null.");
			}

			this.timelines = timelines;
			this.Duration = duration;
		}

		/** Poses the skeleton at the specified time for this animation. */
		public void Apply (Skeleton skeleton, float time, bool loop)
		{
			if (skeleton == null)
			{
				throw new ArgumentException ("skeleton cannot be null.");
			}
			
			if (loop && this.Duration != 0)
			{
				time %= this.Duration;
			}

			foreach (var timeline in this.timelines)
			{
				timeline.Apply (skeleton, time, 1);
			}
		}
		
		/** Poses the skeleton at the specified time for this animation mixed with the current pose.
	 * @param alpha The amount of this animation that affects the current pose. */
		public void Mix (Skeleton skeleton, float time, bool loop, float alpha)
		{
			if (skeleton == null)
			{
				throw new ArgumentException ("skeleton cannot be null.");
			}

			if (loop && this.Duration != 0)
			{
				time %= this.Duration;
			}
			
			foreach (var timeline in this.timelines)
			{
				timeline.Apply (skeleton, time, alpha);
			}
		}

		public override int GetHashCode ()
		{
			if (this.Name != null)
			{
				return this.Name.GetHashCode () + this.Duration.GetHashCode();
			}
			else
			{
				return this.Duration.GetHashCode();
			}
		}

		public bool Equals (Animation other)
		{
			if (this.Name == null && other.Name == null)
			{
				return true;
			}

			if (this.Name == null || other.Name == null)
			{
				return false;
			}

			return String.Compare (this.Name, other.Name) == 0;
		}

		public override bool Equals (Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			Animation other = obj as Animation;
			if (other == null)
			{
				return false;
			}

			return this.Equals (other);
		}
	}
}

