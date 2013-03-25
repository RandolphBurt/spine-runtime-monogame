/// <summary>
/// BoneData.cs
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;

	public class BoneData
	{
		public BoneData (String name, BoneData parent)
		{
			if (name == null)
			{
				throw new ArgumentException ("name cannot be null.");
			}

			this.Name = name;
			this.Parent = parent;
			this.ScaleX = 1;
			this.ScaleY = 1;
		}

		public BoneData Parent
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public  float Length
		{
			get;
			set;
		}

		public  float Rotation
		{
			get;
			set;
		}

		public float X{
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

	}	
}

