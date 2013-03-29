/// <summary>
/// RegionAttachment.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Attachments
{
	using System;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework;
	using Spine.Runtime.MonoGame.Graphics;

	/** Attachment that displays a texture region. */
	internal class RegionAttachment : Attachment
	{
		private TextureRegion region;

		internal RegionAttachment (String name, TextureRegion region) : base(name)
		{
			this.region = region;
		}

		
		public float X 
		{
			get;
			set;
		}
		
		public float Y 
		{
			get;
			set;
		}
		
		public float ScaleX 
		{
			get;
			set;
		}
		
		public float ScaleY 
		{
			get;
			set;
		}
		
		public float Rotation 
		{
			get;
			set;
		}
		
		public float Width 
		{
			get;
			set;
		}
		
		public float Height 
		{
			get;
			set;
		}

		public override void Draw (SpriteBatch batch, Slot slot, bool flipX, bool flipY)
		{
			if (region == null)
			{
				throw new Exception ("RegionAttachment is not resolved: " + this);
			}

			// TODO - Need to change - Really all this calculation should be invoked from the Game.Update method - Drawing should just be drawing!
			var imageX = slot.Bone.worldX + this.X * slot.Bone.m00 + this.Y * slot.Bone.m01;
			var imageY = -(slot.Bone.worldY + this.X * slot.Bone.m10 + this.Y * slot.Bone.m11);

			if (flipX)
			{
				imageX *= -1;
			}
			
			if (flipY)
			{
				imageY *= -1;
			}

			Vector2 destination = new Vector2(
				(int)imageX, 
				(int)imageY);

			Vector2 origin = new Vector2(this.region.Area.Width / 2, this.region.Area.Height / 2);
			var imageRotation = -(slot.Bone.worldRotation + this.Rotation);

			if (this.region.Rotated)
			{
				// Image inside our SpriteSheet/TextureAtlas was rotated so we need to unrotate
				imageRotation -= 90;
			}

			var rotationRadians = (float)(imageRotation / 360 * (Math.PI * 2));

			Vector2 scale = new Vector2(
				slot.Bone.worldScaleX + this.ScaleX - 1,
				slot.Bone.worldScaleY + this.ScaleY - 1);

			/*

			image:setFillColor(slot.r, slot.g, slot.b, slot.a)
			*/

			batch.Draw (region.Texture, destination, this.region.Area, Color.White, rotationRadians, origin, scale, SpriteEffects.None, 0f);
		}
	}
}