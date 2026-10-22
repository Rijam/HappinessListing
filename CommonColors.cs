using Microsoft.Xna.Framework;

namespace HappinessListing
{
	public static class CommonColors
	{
		public static Color ColorFromHex(uint hexColor)
		{
			// Assuming RGB not ARGB
			byte r = (byte)((hexColor >> 16) & 0xFF);
			byte g = (byte)((hexColor >> 8) & 0xFF);
			byte b = (byte)(hexColor & 0xFF);
			return new Color(r, g, b, 255);
		}

		public readonly struct InGameColors
		{
			public readonly struct Love
			{
				public static readonly string HexString = "f2d45a";
				public static readonly uint HexUInt = 0xf2d45a;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Like
			{
				public static readonly string HexString = "9de997";
				public static readonly uint HexUInt = 0x9de997;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Dislike
			{
				public static readonly string HexString = "97dbe4";
				public static readonly uint HexUInt = 0x97dbe4;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Hate
			{
				public static readonly string HexString = "bbb2e3";
				public static readonly uint HexUInt = 0xbbb2e3;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}
		}

		public readonly struct WikiColors
		{
			public readonly struct Love
			{
				public static readonly string HexString = "b3f2b3";
				public static readonly uint HexUInt = 0xb3f2b3;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Like
			{
				public static readonly string HexString = "ddf2b3";
				public static readonly uint HexUInt = 0xddf2b3;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Dislike
			{
				public static readonly string HexString = "f2e0b3";
				public static readonly uint HexUInt = 0xf2e0b3;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Hate
			{
				public static readonly string HexString = "f2b5b3";
				public static readonly uint HexUInt = 0xf2b5b3;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}
		}

		public readonly struct Grayscale
		{
			public readonly struct Love
			{
				public static readonly string HexString = "ffffff";
				public static readonly uint HexUInt = 0xffffff;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Like
			{
				public static readonly string HexString = "dddddd";
				public static readonly uint HexUInt = 0xdddddd;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Dislike
			{
				public static readonly string HexString = "bbbbbb";
				public static readonly uint HexUInt = 0xbbbbbb;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Hate
			{
				public static readonly string HexString = "999999";
				public static readonly uint HexUInt = 0x999999;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}
		}

		public readonly struct ReworkColors
		{
			public readonly struct Love
			{
				public static readonly string HexString = "7CFC00";
				public static readonly uint HexUInt = 0x7CFC00;
				public static readonly Color XnaColor = Color.LawnGreen;
			}

			public readonly struct Like
			{
				public static readonly string HexString = "6495ED";
				public static readonly uint HexUInt = 0x6495ED;
				public static readonly Color XnaColor = Color.CornflowerBlue;
			}

			public readonly struct Dislike
			{
				public static readonly string HexString = "DE8787";
				public static readonly uint HexUInt = 0xDE8787;
				public static readonly Color XnaColor = Color.BurlyWood;
			}

			public readonly struct Hate
			{
				public static readonly string HexString = "C71585";
				public static readonly uint HexUInt = 0xC71585;
				public static readonly Color XnaColor = Color.MediumVioletRed;
			}
		}

		public readonly struct Vibrant
		{
			public readonly struct Love
			{
				public static readonly string HexString = "00FF00";
				public static readonly uint HexUInt = 0x00FF00;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Like
			{
				public static readonly string HexString = "FFFF00";
				public static readonly uint HexUInt = 0xFFFF00;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Dislike
			{
				public static readonly string HexString = "FF7F00";
				public static readonly uint HexUInt = 0xFF7F00;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}

			public readonly struct Hate
			{
				public static readonly string HexString = "FF0000";
				public static readonly uint HexUInt = 0xFF0000;
				public static readonly Color XnaColor = ColorFromHex(HexUInt);
			}
		}
	}
}
