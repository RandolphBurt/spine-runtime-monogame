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
		public Attachment (String name)
		{
			if (name == null)
			{
			
				throw new ArgumentException ("name cannot be null.");
			}

			this.Name = name;
		}
	
		public String Name
		{
			get;
			private set;
		}

		public abstract void Draw (SpriteBatch batch, Slot slot);
	}
}

