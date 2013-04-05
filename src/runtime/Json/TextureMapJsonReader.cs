/// <summary>
/// TextureMapJsonReader.cs
/// </summary>
namespace Spine.Runtime.MonoGame.Json
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	
	using Newtonsoft.Json.Linq;
	
	using Spine.Runtime.MonoGame.Graphics;
	
	public class TextureMapJsonReader : BaseJsonReader
	{
		/*
		 * Json Atlas file expected in the following format.  Rotation is ignored - we expect all images to be correctly rotated

		{"frames": [	
			{
				"filename": "BackLowerLegLeft.png",
			    "frame": {"x":185,"y":106,"w":36,"h":69},
				"rotated": false,
			},
			{
				"filename": "BackLowerLegRight.png",
				"frame": {"x":147,"y":106,"w":36,"h":69},
				"rotated": false,
			},

	    */
		
		public TextureAtlas ReadTextureJsonFile (string jsonFile, Texture2D texture)
		{
			Dictionary<string, TextureRegion> regions = new Dictionary<string, TextureRegion>();
			
			var jsonText = File.ReadAllText (jsonFile);
			JObject data = JObject.Parse (jsonText);
			
			foreach (var frame in data["frames"])
			{
				Rectangle textureAtlasArea;
				
				string filename = Path.GetFileNameWithoutExtension((string)frame["filename"]);
				
				var rotated = this.Read<bool>(frame, "rotated", false);
				
				var details = frame["frame"];
				var x = this.Read<int>(details, "x", 0);
				var y = this.Read<int>(details, "y", 0);
				var width = this.Read<int>(details, "w", 0);
				var height = this.Read<int>(details, "h", 0);
				
				if (rotated)
				{
					// The image inside our texture map is rotated so swap width and height
					textureAtlasArea =new Rectangle(x, y, height, width);
				}
				else
				{
					textureAtlasArea =new Rectangle(x, y, width, height);
				}
				
				regions.Add(filename, new TextureRegion (texture, rotated, textureAtlasArea));
			}
			
			return new TextureAtlas(texture, regions);
		}
	}
}