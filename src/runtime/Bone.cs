/// <summary>
/// Bone.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;

	using Microsoft.Xna.Framework;

	using Spine.Runtime.MonoGame.Utils;

	public class Bone
	{
		internal float rotation;
		internal float scaleX = 1, scaleY = 1;
		internal float m00, m01, worldX; // a b x
		internal float m10, m11, worldY; // c d y
		internal float worldRotation;
		internal float worldScaleX, worldScaleY;

		public Bone (BoneData data, Bone parent)
		{
			if (data == null)
			{
				throw new ArgumentException ("data cannot be null.");
			}
			
			this.Data = data;
			this.Parent = parent;
		}
				
		/** Copy constructor.
	 * @param parent May be null. */
		public Bone (Bone bone, Bone parent)
		{
			if (bone == null)
			{
				throw new ArgumentException ("bone cannot be null.");
			}
			
			this.Parent = parent;
			Data = bone.Data;
			X = bone.X;
			Y = bone.Y;
			rotation = bone.rotation;
			scaleX = bone.scaleX;
			scaleY = bone.scaleY;
		}

		public Bone Parent
		{
			get;
			private set;
		}

		public BoneData Data
		{
			get;
			private set;
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

		/** Computes the world SRT using the parent bone and the local SRT. */
		public void UpdateWorldTransform (bool flipX, bool flipY)
		{
			if (this.Parent != null)
			{
				this.worldX = this.X * this.Parent.m00 + this.Y * this.Parent.m01 + this.Parent.worldX;
				this.worldY = this.X * this.Parent.m10 + this.Y * this.Parent.m11 + this.Parent.worldY;
				this.worldScaleX = this.Parent.worldScaleX * this.scaleX;
				this.worldScaleY = this.Parent.worldScaleY * this.scaleY;
				this.worldRotation = this.Parent.worldRotation + this.rotation;
			}
			else
			{
				this.worldX = this.X;
				this.worldY = this.Y;
				this.worldScaleX = this.scaleX;
				this.worldScaleY = this.scaleY;
				this.worldRotation = this.rotation;
			}

			float cos = MathUtils.CosDeg (worldRotation);
			float sin = MathUtils.SinDeg (worldRotation);

			this.m00 = cos * worldScaleX;
			this.m10 = sin * worldScaleX;
			this.m01 = -sin * worldScaleY;
			this.m11 = cos * worldScaleY;

			if (flipX)
			{
				this.m00 = -this.m00;
				this.m01 = -this.m01;
			}
			if (flipY)
			{
				this.m10 = -this.m10;
				this.m11 = -this.m11;
			}
		}
			
		public void SetToBindPose ()
		{
			X = this.Data.X;
			Y = this.Data.Y;
			rotation = this.Data.Rotation;
			scaleX = this.Data.ScaleX;
			scaleY = this.Data.ScaleY;
		}
	}
}