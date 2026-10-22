using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ModLoader;

namespace HappinessListing
{
	public class RegularDialogueModifications
	{
		/// <summary>
		/// Contains NPC types who should not have their dialogue modified.
		/// </summary>
		public static List<int> NPCBlackList = [];

		internal static string ApplyPreEntryModificationsForRegularDialogue()
		{
			if (NPCBlackList.Contains(HappinessListingEdits.Get_ShopHelper__currentNPCBeingTalkedTo().type))
			{
				return "";
			}
			HappinessListingConfig config = ModContent.GetInstance<HappinessListingConfig>();
			if (config.ColorCodeRegularDialogue != HappinessListingConfig.ColorCoding.None)
			{
				string regularDialogue = "";
				ColorStart(ref regularDialogue, config, DetermineAffectionFromPriceAdjustment());
				return regularDialogue;
			}
			return "";
		}

		/// <summary>
		/// Calls the other methods that apply changes to the dialogue text after the vanilla code has run.
		/// </summary>
		internal static string ApplyPostEntryModificationsForRegularDialogue(ref string regularDialogue)
		{
			if (NPCBlackList.Contains(HappinessListingEdits.Get_ShopHelper__currentNPCBeingTalkedTo().type))
			{
				return "";
			}
			HappinessListingConfig config = ModContent.GetInstance<HappinessListingConfig>();
			if (config.ColorCodeRegularDialogue != HappinessListingConfig.ColorCoding.None)
			{
				ColorEnd(ref regularDialogue);
			}
			return "";
		}

		internal static AffectionLevel DetermineAffectionFromPriceAdjustment()
		{
			double priceAdjustment = Main.LocalPlayer.currentShoppingSettings.PriceAdjustment;
			if (priceAdjustment <= 0.82)
			{
				return AffectionLevel.Love;
			}
			else if (priceAdjustment <= 1)
			{
				return AffectionLevel.Like;
			}
			else if (priceAdjustment <= 1.1)
			{
				return AffectionLevel.Dislike;
			}
			else
			{
				return AffectionLevel.Hate;
			}
		}

		/// <inheritdoc cref="LineEntryModifications.ColorStart(ref string, HappinessListingConfig, AffectionLevel)"/>
		private static void ColorStart(ref string regularDialogue, HappinessListingConfig config, AffectionLevel affectionLevel)
		{
			string colorCode = "";

			switch (config.ColorCodeRegularDialogue)
			{
				case HappinessListingConfig.ColorCoding.HappinessGYOR:
					colorCode = affectionLevel switch
					{
						AffectionLevel.Love => $"[c/{CommonColors.WikiColors.Love.HexString}:",
						AffectionLevel.Like => $"[c/{CommonColors.WikiColors.Like.HexString}:",
						AffectionLevel.Dislike => $"[c/{CommonColors.WikiColors.Dislike.HexString}:",
						AffectionLevel.Hate => $"[c/{CommonColors.WikiColors.Hate.HexString}:",
						_ => "?",
					};
					break;
				case HappinessListingConfig.ColorCoding.HappinessYGBP:
					colorCode = affectionLevel switch
					{
						AffectionLevel.Love => $"[c/{CommonColors.InGameColors.Love.HexString}:",
						AffectionLevel.Like => $"[c/{CommonColors.InGameColors.Like.HexString}:",
						AffectionLevel.Dislike => $"[c/{CommonColors.InGameColors.Dislike.HexString}:",
						AffectionLevel.Hate => $"[c/{CommonColors.InGameColors.Hate.HexString}:",
						_ => "?",
					};
					break;
				case HappinessListingConfig.ColorCoding.HappinessGrayscale:
					colorCode = affectionLevel switch
					{
						AffectionLevel.Love => $"[c/{CommonColors.Grayscale.Love.HexString}:",
						AffectionLevel.Like => $"[c/{CommonColors.Grayscale.Like.HexString}:",
						AffectionLevel.Dislike => $"[c/{CommonColors.Grayscale.Dislike.HexString}:",
						AffectionLevel.Hate => $"[c/{CommonColors.Grayscale.Hate.HexString}:",
						_ => "?",
					};
					break;
				case HappinessListingConfig.ColorCoding.HappinessDialogPanelRework:
					colorCode = affectionLevel switch
					{
						AffectionLevel.Love => $"[c/{CommonColors.ReworkColors.Love.HexString}:",
						AffectionLevel.Like => $"[c/{CommonColors.ReworkColors.Like.HexString}:",
						AffectionLevel.Dislike => $"[c/{CommonColors.ReworkColors.Dislike.HexString}:",
						AffectionLevel.Hate => $"[c/{CommonColors.ReworkColors.Hate.HexString}:",
						_ => "?",
					};
					break;
				case HappinessListingConfig.ColorCoding.HappinessVibrant:
					colorCode = affectionLevel switch
					{
						AffectionLevel.Love => $"[c/{CommonColors.Vibrant.Love.HexString}:",
						AffectionLevel.Like => $"[c/{CommonColors.Vibrant.Like.HexString}:",
						AffectionLevel.Dislike => $"[c/{CommonColors.Vibrant.Dislike.HexString}:",
						AffectionLevel.Hate => $"[c/{CommonColors.Vibrant.Hate.HexString}:",
						_ => "?",
					};
					break;
				default:
					break;
			}
			regularDialogue += colorCode;
		}

		/// <inheritdoc cref="LineEntryModifications.ColorEnd(ref string)"/>
		private static void ColorEnd(ref string regularDialogue)
		{
			LineEntryModifications.ColorEnd(ref regularDialogue);
		}
	}
}
