using System;
using Spine.Runtime.MonoGame.Utils;
using System.Collections.Generic;

namespace Spine.Runtime.MonoGame
{
	public class AnimationState
	{
		private readonly AnimationStateData data;
		internal Animation current, previous;
		internal float currentTime, previousTime;
		internal bool currentLoop, previousLoop;
		internal float mixTime, mixDuration;

		public AnimationState (AnimationStateData data) {
			this.data = data;
		}
		
		public void update (float delta) {
			currentTime += delta;
			previousTime += delta;
			mixTime += delta;
		}

		public void apply (Skeleton skeleton)
		{
			if (current == null)
			{
				return;
			}
			if (previous != null)
			{
				previous.apply (skeleton, previousTime, previousLoop);
				float alpha = mixTime / mixDuration;
				if (alpha >= 1) {
					alpha = 1;
					previous = null;
				}
				current.mix(skeleton, currentTime, currentLoop, alpha);
			} else
				current.apply(skeleton, currentTime, currentLoop);
		}

		
		/** Set the current animation. */
		public void setAnimation (Animation animation, bool loop)
		{
			setAnimation (animation, loop, 0);
		}
		
		/** Set the current animation.
	 * @param time The time within the animation to start. */
		public void setAnimation (Animation animation, bool loop, float time)
		{
			previous = null;
			if (animation != null && current != null)
			{
				mixDuration = data.getMixing(current, animation);
				if (mixDuration > 0) {
					mixTime = 0;
					previous = current;
				}
			}
			current = animation;
			currentLoop = loop;
			currentTime = time;

		}
		
		/** @return May be null. */
		public Animation getAnimation ()
		{
			return current;
		}
		
		/** Returns the time within the current animation. */
		public float getTime ()
		{
			return currentTime;
		}
		
		public void setTime (float time) {
			currentTime = time;
		}
		
		public AnimationStateData getData () {
			return data;
		}
		
		public String toString () {
			return (current != null && current.getName() != null) ? current.getName() : base.ToString();
		}
	}
}

