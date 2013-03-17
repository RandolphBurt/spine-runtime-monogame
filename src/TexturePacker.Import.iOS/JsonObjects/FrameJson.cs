/// <summary>
/// Frame.cs
/// 2013-March
/// </summary>
namespace TexturePacker.Import.JsonObjects
{
	using Newtonsoft.Json;

	internal class FrameJson
	{
		internal FrameJson ()
		{
		}

		[JsonProperty(PropertyName="x")]
		internal int X
		{
			get;
			set;
		}

		[JsonProperty(PropertyName="y")]
		internal int Y
		{
			get;
			set;
		}
	
		[JsonProperty(PropertyName="w")]
		internal int Width
		{
			get;
			set;
		}
	
		[JsonProperty(PropertyName="h")]
		internal int Height
		{
			get;
			set;
		}
	}
}

