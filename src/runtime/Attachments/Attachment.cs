/// <summary>
/// Attachment.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Attachments
{
	using Microsoft.Xna.Framework.Graphics;

	using System;

	public abstract class Attachment
	{
		internal readonly String name;
		
		public Attachment (String name)
		{
			if (name == null)
			{
			
				throw new ArgumentException ("name cannot be null.");
			}

			this.name = name;
		}
		
		public abstract void draw (SpriteBatch batch, Slot slot);
		
		public String getName ()
		{
			return name;
		}
		
		public String toString ()
		{
			return name;
		}
	}
}

