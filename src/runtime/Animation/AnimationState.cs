using System;
using Spine.Runtime.MonoGame.Utils;
using System.Collections.Generic;

namespace Spine.Runtime.MonoGame
{
	public class AnimationState
	{
		internal Animation current, previous;
		internal float currentTime, previousTime;
		internal bool currentLoop, previousLoop;
		internal float mixTime, mixDuration;
		private readonly Dictionary<Key, float> animationToMixTime = new Dictionary<Key, float> ();
		private readonly Key tempKey = new Key ();
		
		public void apply (Skeleton skeleton)
		{
			if (current == null)
			{
				return;
			}
			if (previous != null)
			{
				previous.apply (skeleton, previousTime, previousLoop);
				float alpha = MathUtils.Clamp (mixTime / mixDuration, 0, 1);
				current.mix (skeleton, currentTime, currentLoop, alpha);
				if (alpha == 1)
				{
					previous = null;
				}
			}
			else
			{
				current.apply (skeleton, currentTime, currentLoop);
			}
		}
		
		public void update (float delta)
		{
			currentTime += delta;
			previousTime += delta;
			mixTime += delta;
		}
		
		/** Set the mixing duration between two animations. */
		public void setMixing (Animation from, Animation to, float duration)
		{
			if (from == null)
			{
				throw new ArgumentException ("from cannot be null.");
			}
			if (to == null)
			{
				throw new ArgumentException ("to cannot be null.");
			}
			Key key = new Key ();
			key.a1 = from;
			key.a2 = to;
			animationToMixTime [key] = duration;
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
				tempKey.a1 = current;
				tempKey.a2 = animation;
				if (animationToMixTime.ContainsKey (tempKey))
				{
					mixDuration = animationToMixTime [tempKey];
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
		
		private class Key
		{
			internal Animation a1, a2;
			
			public int hashCode ()
			{
				return 31 * (31 + a1.GetHashCode ()) + a2.GetHashCode ();
			}
			
			public bool equals (Object obj)
			{
				if (this == obj)
				{
					return true;
				}

				if (obj == null)
				{
					return false;
				}

				Key other = (Key)obj;
				if (a1 == null)
				{
					if (other.a1 != null)
					{
						return false;
					}
				}
				else
				if (!a1.Equals (other.a1))
					{
						return false;
					}

				if (a2 == null)
				{
					if (other.a2 != null)
					{
						return false;
					}
				}
				else
				if (!a2.Equals (other.a2))
					{
						return false;
					}
				return true;
			}
		}
	}
}

