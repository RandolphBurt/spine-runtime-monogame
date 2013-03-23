using System;
using System.Collections.Generic;

namespace Spine.Runtime.MonoGame
{
	/** Stores mixing times between animations. */
	public class AnimationStateData {
		internal readonly Dictionary<Key, float> animationToMixTime = new Dictionary<Key, float>();
		internal readonly Key tempKey = new Key();
		
		/** Set the mixing duration between two animations. */
		public void setMixing (Animation from, Animation to, float duration) {
			if (from == null) throw new ArgumentException("from cannot be null.");
			if (to == null) throw new ArgumentException("to cannot be null.");
			Key key = new Key();
			key.a1 = from;
			key.a2 = to;
			animationToMixTime[key] = duration;
		}
		
		public float getMixing (Animation from, Animation to) {
			tempKey.a1 = from;
			tempKey.a2 = to;

			if (animationToMixTime.ContainsKey(tempKey))
			{
				return animationToMixTime[tempKey];
			}
			else
			{
				return 0;
			}
		}
		
		internal class Key {
			internal Animation a1, a2;
			
			public int hashCode () {
				return 31 * (31 + a1.GetHashCode()) + a2.GetHashCode();
			}
			
			public bool equals (Object obj) {
				if (this == obj) return true;
				if (obj == null) return false;
				Key other = (Key)obj;
				if (a1 == null) {
					if (other.a1 != null) return false;
				} else if (!a1.Equals(other.a1)) return false;
				if (a2 == null) {
					if (other.a2 != null) return false;
				} else if (!a2.Equals(other.a2)) return false;
				return true;
			}
		}
	}
}
