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
			},
			{
				"filename": "BackLowerLegRight.png",
				"frame": {"x":147,"y":106,"w":36,"h":69},
			},

	    */

		public TextureAtlas ReadTextureJsonFile (string jsonFile, Texture2D texture)
		{
			Dictionary<string, TextureRegion> regions = new Dictionary<string, TextureRegion>();

			var jsonText = File.ReadAllText (jsonFile);
			JObject data = JObject.Parse (jsonText);

			foreach (var frame in data["frames"])
			{
				string filename = Path.GetFileNameWithoutExtension((string)frame["filename"]);

				var details = frame["frame"];
				var x = this.Read<int>(details, "x", 0);
				var y = this.Read<int>(details, "y", 0);
				var width = this.Read<int>(details, "w", 0);
				var height = this.Read<int>(details, "h", 0);

				regions.Add(filename, new TextureRegion (texture, new Rectangle(x, y, width, height)));
			}

			return new TextureAtlas(texture, regions);
		}
	}
}

