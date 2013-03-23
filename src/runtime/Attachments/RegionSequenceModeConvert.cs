/// <summary>
/// RegionSequenceModeConvert.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Attachments
{
	using System;

	internal static class RegionSequenceModeConvert
	{
		internal static RegionSequenceMode FromString (string input)
		{
			if (!String.IsNullOrWhiteSpace (input))
			{
				switch (input.ToLower ())
				{
					case "forward":
						return RegionSequenceMode.Forward;
					case "Backward":
						return RegionSequenceMode.Backward;
					case "ForwardLoop":
						return RegionSequenceMode.ForwardLoop;
					case "BackwardLoop":
						return RegionSequenceMode.BackwardLoop;
					case "PingPong":
						return RegionSequenceMode.PingPong;
					case "Random":
						return RegionSequenceMode.Random;

				}
			}

			return RegionSequenceMode.Forward;
		}
	}
}

