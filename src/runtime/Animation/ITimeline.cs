/// <summary>
/// ITimeline.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	/** The keyframes for a single animation timeline. */
	public interface ITimeline
	{
		/** Returns the time in seconds of the last keyframe. */
		float GetDuration ();
		
		int GetKeyframeCount ();
		
		/** Sets the value(s) for the specified time. */
		void Apply (Skeleton skeleton, float time, float alpha);
	}
}

