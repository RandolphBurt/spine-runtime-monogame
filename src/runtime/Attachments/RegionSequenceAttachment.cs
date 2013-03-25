/// <summary>
/// RegionSequenceAttachment.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Attachments
{
	using System;
	using Microsoft.Xna.Framework.Graphics;

	/* Not yet implemented.
	 * Spine does not yet support RegionSequenceAttachment therefore this is not yet implemented
	 */
	internal class RegionSequenceAttachment : Attachment
	{
		internal RegionSequenceAttachment (String name) : base(name)
		{
		}

		public override void Draw (SpriteBatch batch, Slot slot)
		{
		}

		internal void SetFrameTime (float fps)
		{
		}

		internal void SetMode (RegionSequenceMode mode)
		{
		}
	}
}

