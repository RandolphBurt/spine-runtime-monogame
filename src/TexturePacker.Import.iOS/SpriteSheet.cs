/// <summary>
/// SpriteSheet.cs
/// 2013-March
/// </summary>
namespace TexturePacker.Import
{
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// Sprite sheet.
	/// Contains a reference to the Texture2D object containing the graphics we need to render
	/// and details of the areas within that texturemap that make up the individual graphics
	/// </summary>
	public class SpriteSheet
	{
		/// <summary>
		/// The source areas within the Teture map that define where each individual image is
		/// </summary>
		private readonly Dictionary<string, Rectangle> imageAreas;

		/// <summary>
		/// Initializes a new instance of the <see cref="TexturePacker.Import.SpriteSheet"/> class.
		/// </summary>
		/// <param name="texture">Texture Map</param>
		/// <param name="imageAreas">Mapping from 'graphics image name' to the area within our texture map</param>
		public SpriteSheet (Texture2D texture, Dictionary<string, Rectangle> imageAreas)
		{
			this.TextureMap = texture;
			this.imageAreas = imageAreas;
		}

		/// <summary>
		/// Gets the area within the Texturemap for a specific image.
		/// </summary>
		/// <param name="filename">Graphic/Image name.</param>
		public Rectangle this[string filename] 
		{
			get
			{
				return this.imageAreas [filename];
			}
		}

		/// <summary>
		/// Gets the texture map containing all the individual images
		/// </summary>
		/// <value>The texture map.</value>
		public Texture2D TextureMap
		{
			get;
			private set;
		}
	}
}

