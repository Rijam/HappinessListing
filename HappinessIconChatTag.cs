using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace HappinessListing
{
	public class HappinessIconChatTag : ITagHandler
	{
		// This chat tag is registered in HappinessList.cs

		// Tags can only be 10 characters long
		public static readonly string HLHappinessIcon = "hlhi";
		public static readonly string Love = $"[{HLHappinessIcon}:love]";
		public static readonly string Like = $"[{HLHappinessIcon}:like]";
		public static readonly string Dislike = $"[{HLHappinessIcon}:disl]";
		public static readonly string Hate = $"[{HLHappinessIcon}:hate]";

		// Tags are set up as prefix/options:text
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			int frameIndex = 0;

			if (text == "love")
			{
				frameIndex = 0;
			}
			else if (text == "like")
			{
				frameIndex = 1;
			}
			else if (text == "disl")
			{
				frameIndex = 2;
			}
			else if (text == "hate")
			{
				frameIndex = 3;
			}

			if (int.TryParse(options, out int index))
			{
				frameIndex = index;
			}

			// ModContent.GetInstance<HappinessListing>().Logger.Debug($"text {text} options {options} frameIndex {frameIndex}");

			return new HappinessIconSnippet(frameIndex, baseColor)
			{
				DeleteWhole = true,
				// Text = $"[hlhi:{text}]"
				Text = $"   " // Can't be an empty string or it doesn't show up at all.
			};
		}

		public class HappinessIconSnippet : TextSnippet
		{
			private int iconIndex;

			public HappinessIconSnippet(int frameIndex, Color baseColor)
			{
				iconIndex = frameIndex;
				Color = baseColor;
			}

			public override bool UniqueDraw(bool justCheckingSize, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default, Color color = default, float scale = 1)
			{
				if (spriteBatch is null || justCheckingSize)
				{
					// ModContent.GetInstance<HappinessListing>().Logger.Debug($"Not drawing; justCheckingSize {justCheckingSize} spriteBatch {spriteBatch}");
					size = default;
					return false;
				}

				int frameX = iconIndex;

				Texture2D happinessIcons = TextureAssets.NPCHappiness.Value;
#if TML144
				Texture2D happinessIcons = ModContent.GetInstance<HappinessListing>().NPCHappinessTexture.Value;
#endif
				spriteBatch.Draw(happinessIcons, position, happinessIcons.Frame(4, 1, frameX, 0), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

				// ModContent.GetInstance<HappinessListing>().Logger.Debug($"DRAWING!! frameX {frameX} color {color}");

				size = new Vector2(24f) * scale; // I don't understand what size does.
				// size = new Vector2(2f) * scale;
				//size = Vector2.One;
				return true;
			}
		}
	}
}
