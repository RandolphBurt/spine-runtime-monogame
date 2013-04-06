/// <summary>
/// BaseJsonReader.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Json
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
			var c = new Color (
				(int)((colorValue >> 24) & 0xff),
				(int)((colorValue >> 16) & 0xff),
				(int)((colorValue >> 8) & 0xff),
				(int)(colorValue & 0xff)
				);

			return c;
		}
	}
}

