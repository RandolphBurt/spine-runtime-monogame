using System;
using System.Collections.Generic;

namespace Spine.Runtime.MonoGame
{
	/** Stores mixing times between animations. */
	public class AnimationStateData
	{
		internal readonly Dictionary<Key, float> animationToMixTime = new Dictionary<Key, float> ();

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

			Key key = new Key (from, to);
			this.animationToMixTime [key] = duration;
		}
		
		public float getMixing (Animation from, Animation to)
		{
			Key key = new Key (from, to);

			if (this.animationToMixTime.ContainsKey (key))
			{
				return this.animationToMixTime [key];
			}
			else
			{
				return 0;
			}
		}
		
		internal class Key  : IEquatable<Key>
		{
			internal Key (Animation a, Animation b)
			{
				this.from = a;
				this.to = b;
			}

			private Animation from, to;
			
			public override int GetHashCode ()
			{
				int fromHashcode = from == null ? 0 : 31 + from.GetHashCode ();
				int toHashcode = to == null ? 0 : to.GetHashCode ();

				return 31 * fromHashcode + toHashcode;
			}
						
			public bool Equals (Key other)
			{
				if (from == null)
				{
					if (other.from != null)
					{
						return false;
					}
				}
				else
				if (!from.Equals (other.from))
					{
						return false;
					}
				if (to == null)
				{
					if (other.to != null)
					{
						return false;
					}
				}
				else
				if (!to.Equals (other.to))
					{
						return false;
					}

				return true;
			}

			public override bool Equals (Object obj)
			{
				if (obj == null)
				{
					return false;
				}

				Key other = (Key)obj;

				if (other == null)
				{
					return false;
				}

				return this.Equals (other);
			}
		}
	}
}
