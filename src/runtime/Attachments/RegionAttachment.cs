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

		public override void Draw (SpriteBatch batch, Slot slot)
		{
			if (region == null)
			{
				throw new Exception ("RegionAttachment is not resolved: " + this);
			}

			var imageX = slot.Bone.worldX + this.X * slot.Bone.m00 + this.Y * slot.Bone.m01;
			var imageY = -(slot.Bone.worldY + this.X * slot.Bone.m10 + this.Y * slot.Bone.m11);
			var imageRotation = -(slot.Bone.worldRotation + this.Rotation);
			var imageWidth = this.Width * (slot.Bone.worldScaleX + this.ScaleX - 1);
			var imageHeight = this.Height * (slot.Bone.worldScaleY + this.ScaleY - 1);

			/*

			image:setFillColor(slot.r, slot.g, slot.b, slot.a)
			*/

			// TODO - check that imageWidth / imageHeight is working for scaling

			Rectangle destination = new Rectangle(
				(int)imageX, 
				(int)imageY, 
				(int)imageWidth, 
				(int)imageHeight);

			Vector2 origin = new Vector2(this.region.Area.Width / 2, this.region.Area.Height / 2);

			float rot = (float)(imageRotation / 360 * (Math.PI * 2));
			batch.Draw (region.Texture, destination, this.region.Area, Color.White, rot, origin, SpriteEffects.None, 0f);
		}
	}
}