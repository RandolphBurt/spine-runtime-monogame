/// <summary>
/// Sprite sheet.cs
/// 2013-March
/// </summary>
namespace TexturePacker.Import.JsonObjects
{
	using Newtonsoft.Json;

	internal class SpriteSheetJson
	{
		internal SpriteSheetJson ()
		{
		}

		[JsonProperty(PropertyName="frames")]
		internal ImageDataJson[] ImageList
		{
			get;
			set;
		}
	}
}

