using System;
using Spine.Runtime.MonoGame.Utils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Spine.Runtime.MonoGame.Attachments
{
	/** Attachment that displays a texture region. */
	internal class RegionAttachment : Attachment
	{
		private float x, y, scaleX, scaleY, rotation, width, height;
		private readonly float[] vertices = new float[20];
		private readonly float[] offset = new float[8];
			
		// TODO
		//	private TextureRegion region;

		internal RegionAttachment (String name) : base(name)
		{
		}

		// TODO
		internal void updateOffset ()
		{
		}

		public override void draw (SpriteBatch batch, Slot slot)
		{
		}

		// TODO
		/* 
			internal void updateOffset () {
				float width = getWidth();
				float height = getHeight();
				float localX2 = width / 2;
				float localY2 = height / 2;
				float localX = -localX2;
				float localY = -localY2;
				if (region is AtlasRegion) {
					AtlasRegion region = (AtlasRegion)this.region;
					if (region.rotate) {
						localX += region.offsetX / region.originalWidth * height;
						localY += region.offsetY / region.originalHeight * width;
						localX2 -= (region.originalWidth - region.offsetX - region.packedHeight) / region.originalWidth * width;
						localY2 -= (region.originalHeight - region.offsetY - region.packedWidth) / region.originalHeight * height;
					} else {
						localX += region.offsetX / region.originalWidth * width;
						localY += region.offsetY / region.originalHeight * height;
						localX2 -= (region.originalWidth - region.offsetX - region.packedWidth) / region.originalWidth * width;
						localY2 -= (region.originalHeight - region.offsetY - region.packedHeight) / region.originalHeight * height;
					}
				}
				float scaleX = getScaleX();
				float scaleY = getScaleY();
				localX *= scaleX;
				localY *= scaleY;
				localX2 *= scaleX;
				localY2 *= scaleY;
				float rotation = getRotation();
				float cos = MathUtils.CosDeg(rotation);
				float sin = MathUtils.SinDeg(rotation);
				float x = getX();
				float y = getY();
				float localXCos = localX * cos + x;
				float localXSin = localX * sin;
				float localYCos = localY * cos + y;
				float localYSin = localY * sin;
				float localX2Cos = localX2 * cos + x;
				float localX2Sin = localX2 * sin;
				float localY2Cos = localY2 * cos + y;
				float localY2Sin = localY2 * sin;
				float[] offset = this.offset;
				offset[0] = localXCos - localYSin;
				offset[1] = localYCos + localXSin;
				offset[2] = localXCos - localY2Sin;
				offset[3] = localY2Cos + localXSin;
				offset[4] = localX2Cos - localY2Sin;
				offset[5] = localY2Cos + localX2Sin;
				offset[6] = localX2Cos - localYSin;
				offset[7] = localYCos + localX2Sin;
			}
			
			internal void setRegion (TextureRegion region) {
				if (region == null) throw new ArgumentException("region cannot be null.");
				TextureRegion oldRegion = this.region;
				this.region = region;
				float[] vertices = this.vertices;
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
				updateOffset();
			}
			
			internal TextureRegion getRegion () {
				if (region == null) throw new IllegalStateException("RegionAttachment is not resolved: " + this);
				return region;
			}
			
			internal void draw (SpriteBatch batch, Slot slot) {
				if (region == null) throw new IllegalStateException("RegionAttachment is not resolved: " + this);
				
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
				
				updateWorldVertices(slot.getBone());
				
				batch.draw(region.getTexture(), vertices, 0, vertices.length);
			}
			
			internal void updateWorldVertices (Bone bone) {
				float[] vertices = this.vertices;
				float[] offset = this.offset;
				float x = bone.getWorldX();
				float y = bone.getWorldY();
				float m00 = bone.getM00();
				float m01 = bone.getM01();
				float m10 = bone.getM10();
				float m11 = bone.getM11();
				vertices[X1] = offset[0] * m00 + offset[1] * m01 + x;
				vertices[Y1] = offset[0] * m10 + offset[1] * m11 + y;
				vertices[X2] = offset[2] * m00 + offset[3] * m01 + x;
				vertices[Y2] = offset[2] * m10 + offset[3] * m11 + y;
				vertices[X3] = offset[4] * m00 + offset[5] * m01 + x;
				vertices[Y3] = offset[4] * m10 + offset[5] * m11 + y;
				vertices[X4] = offset[6] * m00 + offset[7] * m01 + x;
				vertices[Y4] = offset[6] * m10 + offset[7] * m11 + y;
			}
			*/
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