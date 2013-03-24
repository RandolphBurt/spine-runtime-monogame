/// <summary>
/// TextureAtlas.cs
/// </summary>
namespace Spine.Runtime.MonoGame.Graphics
{
	using System.Collections.Generic;

	using Microsoft.Xna.Framework.Graphics;

	public class TextureAtlas
	{
		private Dictionary<string, TextureRegion> regions;

		public TextureAtlas (Texture2D texture, Dictionary<string, TextureRegion> regions)
		{
			this.regions = regions;
		}

		public Texture2D Texture
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the area within the Texturemap for a specific image.
		/// </summary>
		/// <param name="filename">Graphic/Image name.</param>
		public TextureRegion this[string filename] 
		{
			get
			{
				return this.regions [filename];
			}
		}
	}
}

