/// <summary>
/// Frame.cs
/// 2013-March
/// </summary>
namespace TexturePacker.Import.JsonObjects
{
	using Newtonsoft.Json;

	internal class ImageDataJson
	{
		internal ImageDataJson ()
		{
		}

		[JsonProperty(PropertyName="filename")]
		internal string Filename
		{
			get;
			set;
		}

		[JsonProperty(PropertyName="frame")]
		internal FrameJson SourceArea
		{
			get;
			set;
		}

	}
}

