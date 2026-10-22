using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HappinessListing
{
	public class LineEntryModifications
	{
		/// <summary>
		/// Contains NPC types who should not have their dialogue modified.
		/// </summary>
		public static List<int> NPCBlackList = [];

		/// <summary>
		/// Calls the other methods that apply changes to the dialogue text before the vanilla code has run.
		/// </summary>
		/// <param name="textKeyInCategory">The dialogue text category</param>
		internal static bool ApplyPreEntryModifications(string textKeyInCategory, object substitutes = null)
		{
			if (NPCBlackList.Contains(HappinessListingEdits.Get_ShopHelper__currentNPCBeingTalkedTo().type))
			{
				return false;
			}
			HappinessListingConfig config = ModContent.GetInstance<HappinessListingConfig>();
			string currentHappiness = HappinessListingEdits.GetSet_ShopHelper__currentHappiness();
			AddNewLine(ref currentHappiness, config);
			AddIconBeforeEntry(ref currentHappiness, config, textKeyInCategory);
			bool skipOrig = ReplaceDialogueWithGeneric(ref currentHappiness, config, textKeyInCategory, substitutes);
			HappinessListingEdits.GetSet_ShopHelper__currentHappiness(currentHappiness);
			return skipOrig;
		}
		/// <summary>
		/// Calls the other methods that apply changes to the dialogue text after the vanilla code has run.
		/// </summary>
		internal static void ApplyPostEntryModifications()
		{
			if (NPCBlackList.Contains(HappinessListingEdits.Get_ShopHelper__currentNPCBeingTalkedTo().type))
			{
				return;
			}
			HappinessListingConfig config = ModContent.GetInstance<HappinessListingConfig>();
			if (config.ColorCodeHappinessText != HappinessListingConfig.ColorCoding.None)
			{
				string currentHappiness = HappinessListingEdits.GetSet_ShopHelper__currentHappiness();
				ColorEnd(ref currentHappiness);
				HappinessListingEdits.GetSet_ShopHelper__currentHappiness(currentHappiness);
			}
		}

		/// <summary>
		/// Adds a new line character if the config is on and the complete dialogue isn't empty (aka hasn't started to be filled, yet).
		/// </summary>
		private static void AddNewLine(ref string currentHappiness, HappinessListingConfig config)
		{
			if (config.BreakEntriesWithNewLine && currentHappiness != "")
			{
				currentHappiness += "\n";
			}
		}

		/// <summary>
		/// Adds the preceding icon to the dialogue entry based on the config.
		/// </summary>
		private static void AddIconBeforeEntry(ref string currentHappiness, HappinessListingConfig config, string textKeyInCategory)
		{
			if (config.EntryIconType == HappinessListingConfig.IconType.None && config.ColorCodeHappinessText == HappinessListingConfig.ColorCoding.None)
			{
				return;
			}

			switch (config.EntryIconType)
			{
				case HappinessListingConfig.IconType.Asterick:
					currentHappiness += $"{CommonIcons.Asterick} ";
					break;
				case HappinessListingConfig.IconType.Bullet:
					currentHappiness += $"{CommonIcons.Bullet} ";
					break;
				case HappinessListingConfig.IconType.RightArrow:
					currentHappiness += $"{CommonIcons.RightArrow} ";
					break;
				case HappinessListingConfig.IconType.GreaterThan:
					currentHappiness += $"{CommonIcons.GreaterThan} ";
					break;
				case HappinessListingConfig.IconType.Tilde:
					currentHappiness += $"{CommonIcons.Tilde} ";
					break;
				default:
					break;
			}

			AffectionLevel affectionLevel = DetermineAffectionType(textKeyInCategory);
			if (config.EntryIconType != HappinessListingConfig.IconType.None)
			{
				IconBasedOnAffection(ref currentHappiness, config.EntryIconType, affectionLevel);
			}
			if (config.ColorCodeHappinessText != HappinessListingConfig.ColorCoding.None)
			{
				ColorStart(ref currentHappiness, config, affectionLevel);
			}
		}

		/// <summary>
		/// Adds the preceding icon that are based on the affection level to the dialogue entry based on the config.
		/// </summary>
		private static void IconBasedOnAffection(ref string currentHappiness, HappinessListingConfig.IconType iconType, AffectionLevel affectionLevel)
		{
			string affectionIcon = "";

			switch (iconType)
			{
				case HappinessListingConfig.IconType.Emoticons:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Emoticons.Love,
						AffectionLevel.Like => CommonIcons.Emoticons.Like,
						AffectionLevel.Dislike => CommonIcons.Emoticons.Dislike,
						AffectionLevel.Hate => CommonIcons.Emoticons.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconType.ChecksAndXs:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.ChecksAndXs.Love,
						AffectionLevel.Like => CommonIcons.ChecksAndXs.Like,
						AffectionLevel.Dislike => CommonIcons.ChecksAndXs.Dislike,
						AffectionLevel.Hate => CommonIcons.ChecksAndXs.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconType.HappinessIcon:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.HappinessIcon.Love,
						AffectionLevel.Like => CommonIcons.HappinessIcon.Like,
						AffectionLevel.Dislike => CommonIcons.HappinessIcon.Dislike,
						AffectionLevel.Hate => CommonIcons.HappinessIcon.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconType.Potions:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Potions.Love,
						AffectionLevel.Like => CommonIcons.Potions.Like,
						AffectionLevel.Dislike => CommonIcons.Potions.Dislike,
						AffectionLevel.Hate => CommonIcons.Potions.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconType.Pickaxes:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Pickaxes.Love,
						AffectionLevel.Like => CommonIcons.Pickaxes.Like,
						AffectionLevel.Dislike => CommonIcons.Pickaxes.Dislike,
						AffectionLevel.Hate => CommonIcons.Pickaxes.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconType.MathSymbols:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.MathSymbols.Love,
						AffectionLevel.Like => CommonIcons.MathSymbols.Like,
						AffectionLevel.Dislike => CommonIcons.MathSymbols.Dislike,
						AffectionLevel.Hate => CommonIcons.MathSymbols.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconType.Statues:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Statues.Love,
						AffectionLevel.Like => CommonIcons.Statues.Like,
						AffectionLevel.Dislike => CommonIcons.Statues.Dislike,
						AffectionLevel.Hate => CommonIcons.Statues.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconType.HeartsAndSkulls:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.HeartsAndSkulls.Love,
						AffectionLevel.Like => CommonIcons.HeartsAndSkulls.Like,
						AffectionLevel.Dislike => CommonIcons.HeartsAndSkulls.Dislike,
						AffectionLevel.Hate => CommonIcons.HeartsAndSkulls.Hate,
						_ => "?",
					};
					break;
				case HappinessListingConfig.IconType.Paintings:
					affectionIcon = affectionLevel switch
					{
						AffectionLevel.Love => CommonIcons.Paintings.Love,
						AffectionLevel.Like => CommonIcons.Paintings.Like,
						AffectionLevel.Dislike => CommonIcons.Paintings.Dislike,
						AffectionLevel.Hate => CommonIcons.Paintings.Hate,
						_ => "?",
					};
					break;
				default:
					break;
			}

			currentHappiness += $"{affectionIcon} ";
		}

		/// <summary>
		/// Determines the affection level of the dialogue based on the localization key.
		/// </summary>
		public static AffectionLevel DetermineAffectionType(string textKeyInCategory)
		{
			string keyType = DetermineDialogueKeyType(textKeyInCategory);
			if (keyType.Contains("NoHome"))
			{
				return AffectionLevel.Hate;
			}
			else if (keyType.Contains("FarFromHome"))
			{
				return AffectionLevel.Hate;
			}
			else if (keyType.Contains("LoveSpace"))
			{
				return AffectionLevel.Like;
			}
			else if (keyType.Contains("Content"))
			{
				return AffectionLevel.Like;
			}
			else if (keyType.Contains("Love"))
			{
				return AffectionLevel.Love;
			}
			else if (keyType.Contains("Like"))
			{
				return AffectionLevel.Like;
			}
			else if (keyType.Contains("Dislike"))
			{
				return AffectionLevel.Dislike;
			}
			else if (keyType.Contains("Hate"))
			{
				return AffectionLevel.Hate;
			}
			return AffectionLevel.Like;
		}

		/// <summary>
		/// Adds a color code at the beginning of the dialogue line (and after the icon).
		/// </summary>
		private static void ColorStart(ref string currentHappiness, HappinessListingConfig config, AffectionLevel affectionLevel)
		{
			string colorCode = "";

			switch (config.ColorCodeHappinessText)
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

			currentHappiness += colorCode;
		}

		/// <summary>
		/// Adds a closing bracket to end the color code.
		/// </summary>
		internal static void ColorEnd(ref string currentHappiness)
		{
			currentHappiness += "]";
		}

		/// <summary>
		/// Replace the happiness dialogue with the simple, generic dialogue.
		/// </summary>
		/// <param name="currentHappiness">The collective happiness dialogue.</param>
		/// <param name="config">The current config value.</param>
		/// <param name="textKeyInCategory">The type of dialogue it is.</param>
		/// <param name="substitutes">Substitutes for NPCName and BiomeName.</param>
		private static bool ReplaceDialogueWithGeneric(ref string currentHappiness, HappinessListingConfig config, string textKeyInCategory, object substitutes = null)
		{
			if (config.SimplifiedHappinessDialogue != HappinessListingConfig.SimplifiedDialogueSetting.Off)
			{
				if (config.SimplifiedHappinessDialogue == HappinessListingConfig.SimplifiedDialogueSetting.VanillaOnly)
				{
					NPC npcBeingTalkedTo = HappinessListingEdits.Get_ShopHelper__currentNPCBeingTalkedTo();
					if (npcBeingTalkedTo.type >= NPCID.Count) // Talking to a modded NPC.
					{
						return false;
					}
				}

				if (config.SimplifiedHappinessDialogue == HappinessListingConfig.SimplifiedDialogueSetting.ModdedOnly)
				{
					NPC npcBeingTalkedTo = HappinessListingEdits.Get_ShopHelper__currentNPCBeingTalkedTo();
					if (npcBeingTalkedTo.type < NPCID.Count) // Talking to a vanilla NPC.
					{
						return false;
					}
				}

				string textValueWith = Language.GetTextValueWith("TownNPCMood." + DetermineDialogueKeyType(textKeyInCategory), substitutes);
				currentHappiness = currentHappiness + textValueWith + " ";
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines what kind of key it is based on the name.
		/// </summary>
		private static string DetermineDialogueKeyType(string textKeyInCategory)
		{
			if (textKeyInCategory.Contains("Content"))
			{
				return "Content";
			}
			else if (textKeyInCategory.Contains("NoHome"))
			{
				return "NoHome";
			}
			else if (textKeyInCategory.Contains("FarFromHome"))
			{
				return "FarFromHome";
			}
			else if (textKeyInCategory.Contains("LoveSpace"))
			{
				return "LoveSpace";
			}
			else if (textKeyInCategory.Contains("HateCrowded"))
			{
				return "HateCrowded";
			}
			else if (textKeyInCategory.Contains("DislikeCrowded"))
			{
				return "DislikeCrowded";
			}
			else if (textKeyInCategory.Contains("NPC"))
			{
				if (textKeyInCategory.Contains("Love"))
				{
					return "LoveNPC";
				}
				else if (textKeyInCategory.Contains("Like"))
				{
					return "LikeNPC";
				}
				else if (textKeyInCategory.Contains("Dislike"))
				{
					return "DislikeNPC";
				}
				else if (textKeyInCategory.Contains("Hate"))
				{
					return "HateNPC";
				}
			}
			else if (textKeyInCategory.Contains("Biome"))
			{
				if (textKeyInCategory.Contains("Love"))
				{
					return "LoveBiome";
				}
				else if (textKeyInCategory.Contains("Like"))
				{
					return "LikeBiome";
				}
				else if (textKeyInCategory.Contains("Dislike"))
				{
					return "DislikeBiome";
				}
				else if (textKeyInCategory.Contains("Hate"))
				{
					return "HateBiome";
				}
			}
			else if (textKeyInCategory.Contains("HateLonely")) // Princess HateLonely
			{
				return "HateLonely";
			}
			return textKeyInCategory;
		}

		/// <summary>
		/// Method for getting the happiness dialogue max lines. Method so it can update "dynamically" in game.
		/// </summary>
		internal static int MaxLinesConfigValue()
		{
			return ModContent.GetInstance<HappinessListingConfig>().MaxTextLines;
		}
	}
}
