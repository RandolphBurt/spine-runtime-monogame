/// <summary>
/// TexturePackerReader.cs
/// 2013-March
/// </summary>
namespace TexturePacker.Import
{
	using System.Collections.Generic;
	using System.IO;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Newtonsoft.Json;
	using TexturePacker.Import.JsonObjects;

	/*
	 * Json Atlas file expected in the following format.  Rotation is ignored - we expect all images to be correctly rotated

	{"frames": [	
		{
			"filename": "BackLowerLegLeft.png",
		    "frame": {"x":185,"y":106,"w":36,"h":69},
		},
		{
			"filename": "BackLowerLegRight.png",
			"frame": {"x":147,"y":106,"w":36,"h":69},
		},

    */


	/// <summary>
	/// Texture packer reader - Creates a SpriteSheet object based on a TexturePacker json file and Texture map (Texture2D)
	/// </summary>
	public class TexturePackerReader
	{
		/// <summary>
		/// Creates the sprite sheet.
		/// </summary>
		/// <returns>The sprite sheet.</returns>
		/// <param name="atlasJsonFile">Atlas json file.</param>
		/// <param name="textureFile">Texture file.</param>
		public SpriteSheet CreateSpriteSheet(string atlasJsonFile, Texture2D texture)
		{
			var jsonText = File.ReadAllText (atlasJsonFile);

			var spriteSheetJson = JsonConvert.DeserializeObject<SpriteSheetJson>(jsonText);

			var imageList = new Dictionary<string, Rectangle> ();

			foreach (var frame in spriteSheetJson.ImageList)
			{
				imageList.Add(
					Path.GetFileNameWithoutExtension(frame.Filename), 
					new Rectangle (frame.SourceArea.X, frame.SourceArea.Y, frame.SourceArea.Height, frame.SourceArea.Height));
			}

			return new SpriteSheet (texture, imageList);	
		}
	}
}

