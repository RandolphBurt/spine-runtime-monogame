/// <summary>
/// BaseJsonReader.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{	
	using Microsoft.Xna.Framework;

	using Newtonsoft.Json.Linq;

	public abstract class BaseJsonReader
	{
		protected T Read<T> (JToken jsonObject, string name, T defaultValue)
		{
			JToken token = jsonObject [name];
			if (token == null)
			{
				return defaultValue;
			}
			
			return token.Value<T> ();
		}

		protected Color ReadColor (string input)
		{
			var colorValue = uint.Parse (input, System.Globalization.NumberStyles.HexNumber);
			return new Color (
				(colorValue >> 6) & 0xff,
				(colorValue >> 4) & 0xff,
				(colorValue >> 2) & 0xff,
				colorValue & 0xff
				);
		}
	}
}

