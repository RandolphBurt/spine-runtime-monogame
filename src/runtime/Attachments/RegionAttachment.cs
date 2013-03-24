/// <summary>
/// RegionAttachment.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Attachments
{
	using System;
	using Spine.Runtime.MonoGame.Utils;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework;
	using Spine.Runtime.MonoGame.Graphics;

	/** Attachment that displays a texture region. */
	internal class RegionAttachment : Attachment
	{
		private const int X1 = 0;
		private const int Y1 = 1;
		private const int C1 = 2;
		private const int U1 = 3;
		private const int V1 = 4;
		private const int X2 = 5;
		private const int Y2 = 6;
		private const int C2 = 7;
		private const int U2 = 8;
		private const int V2 = 9;
		private const int X3 = 10;
		private const int Y3 = 11;
		private const int C3 = 12;
		private const int U3 = 13;
		private const int V3 = 14;
		private const int X4 = 15;
		private const int Y4 = 16;
		private const int C4 = 17;
		private const int U4 = 18;
		private const int V4 = 19;

		private TextureRegion region;
		private float x, y, scaleX, scaleY, rotation, width, height;
		private readonly float[] vertices = new float[20];
		private readonly float[] offset = new float[8];
			
		// TODO
		//	private TextureRegion region;

		internal RegionAttachment (String name) : base(name)
		{
		}

		internal void updateOffset ()
		{
			float localX2 = this.width / 2;
			float localY2 = this.height / 2;
			float localX = -localX2;
			float localY = -localY2;

			/* TODO ????
				if (region is AtlasRegion) {
					AtlasRegion region = (AtlasRegion)this.region;
					if (region.rotate) {
						localX += region.offsetX / region.originalWidth * this.height;
						localY += region.offsetY / region.originalHeight * this.width;
						localX2 -= (region.originalWidth - region.offsetX - region.packedHeight) / region.originalWidth * this.width;
						localY2 -= (region.originalHeight - region.offsetY - region.packedWidth) / region.originalHeight * this.height;
					} else {
						localX += region.offsetX / region.originalWidth * this.width;
						localY += region.offsetY / region.originalHeight * this.height;
						localX2 -= (region.originalWidth - region.offsetX - region.packedWidth) / region.originalWidth * this.width;
						localY2 -= (region.originalHeight - region.offsetY - region.packedHeight) / region.originalHeight * this.height;
					}
				}
				*/

			localX *= this.scaleX;
			localY *= this.scaleY;
			localX2 *= this.scaleX;
			localY2 *= this.scaleY;
			float cos = MathUtils.CosDeg (this.rotation);
			float sin = MathUtils.SinDeg (this.rotation);
			float localXCos = localX * cos + this.x;
			float localXSin = localX * sin;
			float localYCos = localY * cos + this.y;
			float localYSin = localY * sin;
			float localX2Cos = localX2 * cos + this.x;
			float localX2Sin = localX2 * sin;
			float localY2Cos = localY2 * cos + this.y;
			float localY2Sin = localY2 * sin;
			this.offset [0] = localXCos - localYSin;
			this.offset [1] = localYCos + localXSin;
			this.offset [2] = localXCos - localY2Sin;
			this.offset [3] = localY2Cos + localXSin;
			this.offset [4] = localX2Cos - localY2Sin;
			this.offset [5] = localY2Cos + localX2Sin;
			this.offset [6] = localX2Cos - localYSin;
			this.offset [7] = localYCos + localX2Sin;
		}
			
		internal void setRegion (TextureRegion region)
		{
			if (region == null)
			{
				throw new ArgumentException ("region cannot be null.");
			}

			this.region = region;

			/* TODO ???
				if (region is AtlasRegion && ((AtlasRegion)region).rotate) {
					vertices[U2] = region.getU();
					vertices[V2] = region.getV2();
					vertices[U3] = region.getU();
					vertices[V3] = region.getV();
					vertices[U4] = region.getU2();
					vertices[V4] = region.getV();
					vertices[U1] = region.getU2();
					vertices[V1] = region.getV2();
				} else {
					vertices[U1] = region.getU();
					vertices[V1] = region.getV2();
					vertices[U2] = region.getU();
					vertices[V2] = region.getV();
					vertices[U3] = region.getU2();
					vertices[V3] = region.getV();
					vertices[U4] = region.getU2();
					vertices[V4] = region.getV2();
				}
				*/

			Rectangle area = region.Area;

			float u = area.X;
			float u2 = area.X + area.Width;
			float v = area.Y;
			float v2 = area.Y + area.Height;

			this.vertices [U1] = u;
			this.vertices [V1] = v2;
			this.vertices [U2] = u;
			this.vertices [V2] = v;
			this.vertices [U3] = u2;
			this.vertices [V3] = v;
			this.vertices [U4] = u2;
			this.vertices [V4] = v2;

			updateOffset ();
		}
			
		internal TextureRegion getRegion ()
		{
			if (region == null)
			{
				throw new Exception ("RegionAttachment is not resolved: " + this);
			}
			return region;
		}
			
		public override void draw (SpriteBatch batch, Slot slot)
		{
			if (region == null)
			{
				throw new Exception ("RegionAttachment is not resolved: " + this);
			}
				
			/* TODO
				Color skeletonColor = slot.getSkeleton().getColor();
				Color slotColor = slot.getColor();
				float color = NumberUtils.intToFloatColor( //
				                                          ((int)(255 * skeletonColor.a * slotColor.a) << 24) //
				                                          | ((int)(255 * skeletonColor.b * slotColor.b) << 16) //
				                                          | ((int)(255 * skeletonColor.g * slotColor.g) << 8) //
				                                          | ((int)(255 * skeletonColor.r * slotColor.r)));
				float[] vertices = this.vertices;
				vertices[C1] = color;
				vertices[C2] = color;
				vertices[C3] = color;
				vertices[C4] = color;
				*/

			Bone bone = slot.getBone ();
			updateWorldVertices (bone);

			Rectangle destination = new Rectangle(
				(int)this.vertices[X2], 
				800 - (int)this.vertices[Y2], 
				(int)this.width, 
				(int)this.height);

			Vector2 origin = new Vector2(this.region.Area.Width / 2, this.region.Area.Height / 2);
			/*
			if(slot.toString () == "BackLowerLegLeft")
			{
				System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15} {16} {17} {18} {19} ", 
				                                   this.vertices[X1], this.vertices[Y1], this.vertices[C1], this.vertices[U1], this.vertices[V1],
				                                   this.vertices[X2], this.vertices[Y2], this.vertices[C2], this.vertices[U2], this.vertices[V2],
				                                   this.vertices[X3], this.vertices[Y3], this.vertices[C3], this.vertices[U3], this.vertices[V3],
				                                   this.vertices[X4], this.vertices[Y4], this.vertices[C4], this.vertices[U4], this.vertices[V4]);
				                                   }
			*/

			float rot = (float)((rotation  + bone.rotation) / 360 * (Math.PI * 2));
			//batch.Draw (region.Texture, destination, this.region.Area, Color.White, rot, origin, SpriteEffects.None, 0f);
			batch.Draw (region.Texture, destination, this.region.Area, Color.White);

			// TODO
			// batch.Draw (region.Texture, vertices, 0, vertices.length);
		}
			
		internal void updateWorldVertices (Bone bone)
		{
			float worldX = bone.getWorldX ();
			float worldY = bone.getWorldY ();
			float m00 = bone.getM00 ();
			float m01 = bone.getM01 ();
			float m10 = bone.getM10 ();
			float m11 = bone.getM11 ();
			this.vertices [X1] = this.offset [0] * m00 + offset [1] * m01 + worldX;
			this.vertices [Y1] = this.offset [0] * m10 + offset [1] * m11 + worldY;
			this.vertices [X2] = this.offset [2] * m00 + offset [3] * m01 + worldX;
			this.vertices [Y2] = this.offset [2] * m10 + offset [3] * m11 + worldY;
			this.vertices [X3] = this.offset [4] * m00 + offset [5] * m01 + worldX;
			this.vertices [Y3] = this.offset [4] * m10 + offset [5] * m11 + worldY;
			this.vertices [X4] = this.offset [6] * m00 + offset [7] * m01 + worldX;
			this.vertices [Y4] = this.offset [6] * m10 + offset [7] * m11 + worldY;
		}
			
		internal float[] getWorldVertices ()
		{
			return vertices;
		}
			
		internal float getX ()
		{
			return x;
		}
			
		internal void setX (float x)
		{
			this.x = x;
		}
			
		internal float getY ()
		{
			return y;
		}
			
		internal void setY (float y)
		{
			this.y = y;
		}
			
		internal float getScaleX ()
		{
			return scaleX;
		}
			
		internal void setScaleX (float scaleX)
		{
			this.scaleX = scaleX;
		}
			
		internal float getScaleY ()
		{
			return scaleY;
		}
			
		internal void setScaleY (float scaleY)
		{
			this.scaleY = scaleY;
		}
			
		internal float getRotation ()
		{
			return rotation;
		}
			
		internal void setRotation (float rotation)
		{
			this.rotation = rotation;
		}
			
		internal float getWidth ()
		{
			return width;
		}
			
		internal void setWidth (float width)
		{
			this.width = width;
		}
			
		internal float getHeight ()
		{
			return height;
		}
			
		internal void setHeight (float height)
		{
			this.height = height;
		}
	}
}