using Microsoft.Xna.Framework;
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

			if (config.EntryIconType == HappinessListingConfig.IconType.Asterick)
			{
				currentHappiness += "* ";
			}
			else if (config.EntryIconType == HappinessListingConfig.IconType.Bullet)
			{
				currentHappiness += "• ";
			}
			else if (config.EntryIconType == HappinessListingConfig.IconType.RightArrow)
			{
				currentHappiness += "→ ";
			}
			else if (config.EntryIconType == HappinessListingConfig.IconType.GreaterThan)
			{
				currentHappiness += "> ";
			}
			else if (config.EntryIconType == HappinessListingConfig.IconType.Tilde)
			{
				currentHappiness += "~ ";
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
			if (iconType == HappinessListingConfig.IconType.Emoticons)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => ":D",
					AffectionLevel.Like => ":)",
					AffectionLevel.Dislike => ":(",
					AffectionLevel.Hate => ":'(",
					_ => "?",
				};
			}
			else if (iconType == HappinessListingConfig.IconType.ChecksAndXs)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => "✔",
					AffectionLevel.Like => "✓",
					AffectionLevel.Dislike => "✗",
					AffectionLevel.Hate => "✘",
					_ => "?",
				};
			}
			else if (iconType == HappinessListingConfig.IconType.HappinessIcon)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => HappinessIconChatTag.Love,
					AffectionLevel.Like => HappinessIconChatTag.Like,
					AffectionLevel.Dislike => HappinessIconChatTag.Dislike,
					AffectionLevel.Hate => HappinessIconChatTag.Hate,
					_ => "?",
				};
			}
			else if (iconType == HappinessListingConfig.IconType.Potions)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => "[i:2352]", // Love Potion
					AffectionLevel.Like => "[i:293]", // Mana Regen Potion
					AffectionLevel.Dislike => "[i:2353]", // Stink Potion
					AffectionLevel.Hate => "[i:678]", // Red Potion
					_ => "?",
				};
			}
			else if (iconType == HappinessListingConfig.IconType.Pickaxes)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => "[i:2786]", // Solar Flare Pickaxe
					AffectionLevel.Like => "[i:990]", // Pickaxe Axe
					AffectionLevel.Dislike => "[i:103]", // Nightmare Pickaxe
					AffectionLevel.Hate => "[i:3509]", // Copper Pickaxe
					_ => "?",
				};
			}
			else if (iconType == HappinessListingConfig.IconType.MathSymbols)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => "×",
					AffectionLevel.Like => "+",
					AffectionLevel.Dislike => "-",
					AffectionLevel.Hate => "÷",
					_ => "?",
				};
			}
			else if (iconType == HappinessListingConfig.IconType.Statues)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => "[i:HeartStatue]",
					AffectionLevel.Like => "[i:StarStatue]",
					AffectionLevel.Dislike => "[i:ImpStatue]",
					AffectionLevel.Hate => "[i:GloomStatue]",
					_ => "?",
				};
			}
			else if (iconType == HappinessListingConfig.IconType.HeartsAndSkulls)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => "[i:Heart]",
					AffectionLevel.Like => "[i:Star]",
					AffectionLevel.Dislike => "[i:LivingFireBlock]",
					AffectionLevel.Hate => "[i:Skull]",
					_ => "?",
				};
			}
			else if (iconType == HappinessListingConfig.IconType.Paintings)
			{
				affectionIcon = affectionLevel switch
				{
					AffectionLevel.Love => "[i:1482]", // Good Morning
					AffectionLevel.Like => "[i:5248]", // Happy Little Tree
					AffectionLevel.Dislike => "[i:4723]", // Nevermore
					AffectionLevel.Hate => "[i:1475]", // Darkness
					_ => "?",
				};
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
			if (config.ColorCodeHappinessText == HappinessListingConfig.ColorCoding.HappinessGYOR)
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
			else if (config.ColorCodeHappinessText == HappinessListingConfig.ColorCoding.HappinessYGBP)
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
			else if (config.ColorCodeHappinessText == HappinessListingConfig.ColorCoding.HappinessGrayscale)
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
			else if (config.ColorCodeHappinessText == HappinessListingConfig.ColorCoding.HappinessDialogPanelRework)
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
			else if (config.ColorCodeHappinessText == HappinessListingConfig.ColorCoding.HappinessVibrant)
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
	}
}
