/// <summary>
/// TextureRegion.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Graphics
{
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework;

	public class TextureRegion
	{
		public TextureRegion (Texture2D texture, Rectangle area)
		{
			this.Texture = texture;
			this.Area = area;
		}

		public Rectangle Area
		{
			get;
			private set;
		}
		
		public Texture2D Texture
		{
			get;
			private set;
		}
	}
}

