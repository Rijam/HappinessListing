using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace HappinessListing
{
	public class PortraitNameModifications
	{
		/// <summary>
		/// Contains NPC types who should not have their portrait name modified.
		/// </summary>
		public static List<int> NPCBlackList = [];

		/// <summary>
		/// Returns the color the NPC's name should be based on the config value.
		/// </summary>
		internal static Color ModifyPortraitNameColor()
		{
			Color nameColor = Color.White;
			if (Main.LocalPlayer.TalkNPC is not null && NPCBlackList.Contains(Main.LocalPlayer.TalkNPC.type))
			{
				return nameColor;
			}
			HappinessListingConfig config = ModContent.GetInstance<HappinessListingConfig>();
			if (config.ColorCodePortraitName != HappinessListingConfig.ColorCoding.None)
			{
				return NameColor(config, RegularDialogueModifications.DetermineAffectionFromPriceAdjustment());
			}
			return nameColor;
		}

		/// <summary>
		/// Returns the color based on the config and affection level.
		/// </summary>
		private static Color NameColor(HappinessListingConfig config, AffectionLevel affectionLevel)
		{
			Color nameColor = Color.White;

			switch (config.ColorCodePortraitName)
			{
				case HappinessListingConfig.ColorCoding.HappinessGYOR:
					nameColor = affectionLevel switch
					{
						AffectionLevel.Love => CommonColors.WikiColors.Love.XnaColor,
						AffectionLevel.Like => CommonColors.WikiColors.Like.XnaColor,
						AffectionLevel.Dislike => CommonColors.WikiColors.Dislike.XnaColor,
						AffectionLevel.Hate => CommonColors.WikiColors.Hate.XnaColor,
						_ => Color.White,
					};
					break;
				case HappinessListingConfig.ColorCoding.HappinessYGBP:
					nameColor = affectionLevel switch
					{
						AffectionLevel.Love => CommonColors.InGameColors.Love.XnaColor,
						AffectionLevel.Like => CommonColors.InGameColors.Like.XnaColor,
						AffectionLevel.Dislike => CommonColors.InGameColors.Dislike.XnaColor,
						AffectionLevel.Hate => CommonColors.InGameColors.Hate.XnaColor,
						_ => Color.White,
					};
					break;
				case HappinessListingConfig.ColorCoding.HappinessGrayscale:
					nameColor = affectionLevel switch
					{
						AffectionLevel.Love => CommonColors.Grayscale.Love.XnaColor,
						AffectionLevel.Like => CommonColors.Grayscale.Like.XnaColor,
						AffectionLevel.Dislike => CommonColors.Grayscale.Dislike.XnaColor,
						AffectionLevel.Hate => CommonColors.Grayscale.Hate.XnaColor,
						_ => Color.White,
					};
					break;
				case HappinessListingConfig.ColorCoding.HappinessDialogPanelRework:
					nameColor = affectionLevel switch
					{
						AffectionLevel.Love => CommonColors.ReworkColors.Love.XnaColor,
						AffectionLevel.Like => CommonColors.ReworkColors.Like.XnaColor,
						AffectionLevel.Dislike => CommonColors.ReworkColors.Dislike.XnaColor,
						AffectionLevel.Hate => CommonColors.ReworkColors.Hate.XnaColor,
						_ => Color.White,
					};
					break;
				case HappinessListingConfig.ColorCoding.HappinessVibrant:
					nameColor = affectionLevel switch
					{
						AffectionLevel.Love => CommonColors.Vibrant.Love.XnaColor,
						AffectionLevel.Like => CommonColors.Vibrant.Like.XnaColor,
						AffectionLevel.Dislike => CommonColors.Vibrant.Dislike.XnaColor,
						AffectionLevel.Hate => CommonColors.Vibrant.Hate.XnaColor,
						_ => Color.White,
					};
					break;
				default:
					break;
			}

			return nameColor;
		}

		/// <summary>
		/// Draws an icon and price factor under the portrait.
		/// </summary>
		internal static void DrawIconUnderName(Vector2 npcChatTopLeft)
		{
			if (Main.LocalPlayer.TalkNPC is not null && NPCBlackList.Contains(Main.LocalPlayer.TalkNPC.type))
			{
				return;
			}
			HappinessListingConfig config = ModContent.GetInstance<HappinessListingConfig>();

			if (config.PortraitNameIconType == HappinessListingConfig.IconTypeHappinessOnly.None
				&& config.PortraitHappinessPercentage == false)
			{
				return;
			}

			AffectionLevel affectionLevel = RegularDialogueModifications.DetermineAffectionFromPriceAdjustment();
			DynamicSpriteFont font = FontAssets.ItemStack.Value;

			// Portrait animation
			int frameCounter = Main.npcChatPortraitFrameCounter; // Vanilla code: int num = npcChatPortraitFrameCounter++;
			int yOffset = 56;
			Vector2 position = npcChatTopLeft + new Vector2(-yOffset - 6, yOffset + 6);
			int animationMultiple = frameCounter % 120;
			int totalAnimationTime = frameCounter / 120; // integer division. So anything < 0 == 0
			if (totalAnimationTime == 0) // Animate for 2 seconds
			{
				double bounceSpeed = 80.0;
				float bounceReduction = 0.25f;
				float easeOutBounce = (float)Utils.EaseOutBounce(Utils.Clamp(animationMultiple, 0.0, bounceSpeed) / bounceSpeed);
				position += new Vector2(0f, (float)(-yOffset) * bounceReduction * (1f - easeOutBounce));
			}

			if (config.PortraitNameIconType != HappinessListingConfig.IconTypeHappinessOnly.None)
			{
				string icon = IconBasedOnAffection(config.PortraitNameIconType, affectionLevel, out bool applyOffset);
				Vector2 measuredIcon = ChatManager.GetStringSize(font, icon, Vector2.One); // Vanilla code: Vector2 measuredString = font.MeasureString(icon);
				Vector2 iconOffset = applyOffset ? measuredIcon / 2f : Vector2.Zero; // Chat tags get drawn from to the top left instead of the center, so correct that here.
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, icon, position + new Vector2(0f, yOffset + 1 + measuredIcon.Y) - iconOffset, Color.White, 0f, measuredIcon / 2f, Vector2.One);
			}
			if (config.PortraitHappinessPercentage == true)
			{
				string percentage = $"{(int)(Main.LocalPlayer.currentShoppingSettings.PriceAdjustment * 100f)}%";
				Vector2 measuredPercentage = ChatManager.GetStringSize(font, percentage, Vector2.One);
				float precentageYOffset = config.PortraitNameIconType != HappinessListingConfig.IconTypeHappinessOnly.None ? (measuredPercentage.Y * 2f) : measuredPercentage.Y;
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, percentage, position + new Vector2(0f, yOffset + 1 + precentageYOffset), NameColor(config, affectionLevel), 0f, measuredPercentage / 2f, Vector2.One);
			}
		}

		/// <summary>
		/// Returns the icon that are based on the affection level to the dialogue entry based on the config.
		/// </summary>
		private static string IconBasedOnAffection(HappinessListingConfig.IconTypeHappinessOnly iconType, AffectionLevel affectionLevel, out bool applyOffset)
		{
			string affectionIcon = "";
			applyOffset = false;

			switch (iconType)
			{
				case HappinessListingConfig.IconTypeHappinessOnly.Emoticons:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Emoticons.Love,
						AffectionLevel.Like => CommonIcons.Emoticons.Like,
						AffectionLevel.Dislike => CommonIcons.Emoticons.Dislike,
						AffectionLevel.Hate => CommonIcons.Emoticons.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconTypeHappinessOnly.ChecksAndXs:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.ChecksAndXs.Love,
						AffectionLevel.Like => CommonIcons.ChecksAndXs.Like,
						AffectionLevel.Dislike => CommonIcons.ChecksAndXs.Dislike,
						AffectionLevel.Hate => CommonIcons.ChecksAndXs.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconTypeHappinessOnly.HappinessIcon:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.HappinessIcon.Love,
						AffectionLevel.Like => CommonIcons.HappinessIcon.Like,
						AffectionLevel.Dislike => CommonIcons.HappinessIcon.Dislike,
						AffectionLevel.Hate => CommonIcons.HappinessIcon.Hate,
						_ => "?",
					};
					applyOffset = true;
					break;
				case HappinessListingConfig.IconTypeHappinessOnly.Potions:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Potions.Love,
						AffectionLevel.Like => CommonIcons.Potions.Like,
						AffectionLevel.Dislike => CommonIcons.Potions.Dislike,
						AffectionLevel.Hate => CommonIcons.Potions.Hate,
						_ => "?",
					};
					applyOffset = true;
					break;
				case HappinessListingConfig.IconTypeHappinessOnly.Pickaxes:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Pickaxes.Love,
						AffectionLevel.Like => CommonIcons.Pickaxes.Like,
						AffectionLevel.Dislike => CommonIcons.Pickaxes.Dislike,
						AffectionLevel.Hate => CommonIcons.Pickaxes.Hate,
						_ => "?",
					};
					applyOffset = true;
					break;
				case HappinessListingConfig.IconTypeHappinessOnly.MathSymbols:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.MathSymbols.Love,
						AffectionLevel.Like => CommonIcons.MathSymbols.Like,
						AffectionLevel.Dislike => CommonIcons.MathSymbols.Dislike,
						AffectionLevel.Hate => CommonIcons.MathSymbols.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconTypeHappinessOnly.Statues:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Statues.Love,
						AffectionLevel.Like => CommonIcons.Statues.Like,
						AffectionLevel.Dislike => CommonIcons.Statues.Dislike,
						AffectionLevel.Hate => CommonIcons.Statues.Hate,
						_ => "?",
					};
					applyOffset = true;
					break;
				case HappinessListingConfig.IconTypeHappinessOnly.HeartsAndSkulls:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.HeartsAndSkulls.Love,
						AffectionLevel.Like => CommonIcons.HeartsAndSkulls.Like,
						AffectionLevel.Dislike => CommonIcons.HeartsAndSkulls.Dislike,
						AffectionLevel.Hate => CommonIcons.HeartsAndSkulls.Hate,
						_ => "?",
					};
					applyOffset = true;
					break;
				case HappinessListingConfig.IconTypeHappinessOnly.Paintings:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Paintings.Love,
						AffectionLevel.Like => CommonIcons.Paintings.Like,
						AffectionLevel.Dislike => CommonIcons.Paintings.Dislike,
						AffectionLevel.Hate => CommonIcons.Paintings.Hate,
						_ => "?",
					};
					applyOffset = true;
					break;
				default:
					break;
			}

			return affectionIcon;
		}
	}
}
