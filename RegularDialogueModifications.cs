using Microsoft.Xna.Framework;
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

		private static AffectionLevel DetermineAffectionFromPriceAdjustment()
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
			if (config.ColorCodeRegularDialogue == HappinessListingConfig.ColorCoding.HappinessGYOR)
			{
				colorCode = affectionLevel switch
				{
					AffectionLevel.Love => "[c/b3f2b3:",
					AffectionLevel.Like => "[c/ddf2b3:",
					AffectionLevel.Dislike => "[c/f2e0b3:",
					AffectionLevel.Hate => "[c/f2b5b3:",
					_ => "?",
				};
			}
			else if (config.ColorCodeRegularDialogue == HappinessListingConfig.ColorCoding.HappinessYGBP)
			{
				colorCode = affectionLevel switch
				{
					AffectionLevel.Love => "[c/f2d45a:",
					AffectionLevel.Like => "[c/9de997:",
					AffectionLevel.Dislike => "[c/97dbe4:",
					AffectionLevel.Hate => "[c/bbb2e3:",
					_ => "?",
				};
			}
			else if (config.ColorCodeRegularDialogue == HappinessListingConfig.ColorCoding.HappinessGrayscale)
			{
				colorCode = affectionLevel switch
				{
					AffectionLevel.Love => "[c/ffffff:",
					AffectionLevel.Like => "[c/dddddd:",
					AffectionLevel.Dislike => "[c/bbbbbb:",
					AffectionLevel.Hate => "[c/999999:",
					_ => "?",
				};
			}
			else if (config.ColorCodeRegularDialogue == HappinessListingConfig.ColorCoding.HappinessDialogPanelRework)
			{
				colorCode = affectionLevel switch
				{
					AffectionLevel.Love => $"[c/{Color.LawnGreen.Hex3()}:",
					AffectionLevel.Like => $"[c/{Color.CornflowerBlue.Hex3()}:",
					AffectionLevel.Dislike => $"[c/{Color.BurlyWood.Hex3()}:",
					AffectionLevel.Hate => $"[c/{Color.MediumVioletRed.Hex3()}:",
					_ => "?",
				};
			}
			else if (config.ColorCodeRegularDialogue == HappinessListingConfig.ColorCoding.HappinessVibrant)
			{
				colorCode = affectionLevel switch
				{
					AffectionLevel.Love => "[c/00FF00:",
					AffectionLevel.Like => "[c/FFFF00:",
					AffectionLevel.Dislike => "[c/FF7F00:",
					AffectionLevel.Hate => "[c/FF0000:",
					_ => "?",
				};
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
