using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HappinessListing
{
	public class BestiaryEntryModifications : GlobalNPC
	{
		/// <summary>
		/// Contains NPC types who should not receive an additional info box.
		/// </summary>
		public static List<int> NPCBlackList = [];

		// Adds the additional flavor text info box to the all Town NPCs unless they are Town Pets or don't have happiness.
		public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			if (NPCBlackList.Contains(npc.type))
			{
				return;
			}
			if (npc.townNPC
				&& !NPCID.Sets.IsTownPet[npc.type]
				&& !NPCID.Sets.NoTownNPCHappiness[npc.type] 
				&& ModContent.GetInstance<HappinessListingConfig>().BestiaryFlavorTextEntry != HappinessListingConfig.BestiaryPreferencesEntry.None)
			{
				bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement(FormatNPCPreferences(npc)));
			}
		}

		/// <summary>
		/// Looks up the NPC's preferences and organizes them in lists. Biomes will be added to the list first.
		/// </summary>
		/// <param name="npc">The NPC instance.</param>
		/// <param name="loveList">The list of loved biomes and NPCs.</param>
		/// <param name="likeList">The list of liked biomes and NPCs.</param>
		/// <param name="dislikeList">he list of diliked biomes and NPCs.</param>
		/// <param name="hateList">he list of hated biomes and NPCs.</param>
		/// <returns>True if preferences of any kind were found or it is the Princess.</returns>
		public static bool TryGetNPCPreferences(NPC npc, out List<string> loveList, out List<string> likeList, out List<string> dislikeList, out List<string> hateList)
		{
			// Code adapted from Dialogue Panel Rework by Cyrilly https://github.com/Cyrillya/DialogueTweak/blob/1.4.4/Interfaces/ChatMethods.cs#L24
			loveList = [];
			likeList = [];
			dislikeList = [];
			hateList = [];

			PersonalityDatabase personalityDatabase = HappinessListingEdits.Get_ShopHelper__database(Main.ShopHelper);
			if (personalityDatabase.TryGetProfileByNPCID(npc.type, out PersonalityProfile profile))
			{
				List<IShopPersonalityTrait> allShopModifiers = profile.ShopModifiers;
				List<IShopPersonalityTrait> iNPCPreferences = allShopModifiers.Where(trait => trait is NPCPreferenceTrait).ToList();
				List<IShopPersonalityTrait> iBiomePreferences = allShopModifiers.Where(trait => trait is BiomePreferenceListTrait).ToList();

				List<NPCPreferenceTrait> allNPCPreferences = null;
				List<BiomePreferenceListTrait> allBiomePreferences = null;

				if (iNPCPreferences != null)
				{
					allNPCPreferences = iNPCPreferences.ConvertAll(t => t as NPCPreferenceTrait);
					//allNPCPreferences.Sort(NPCLevelComparison);
				}
				if (iBiomePreferences != null)
				{
					allBiomePreferences = iBiomePreferences.ConvertAll(t => t as BiomePreferenceListTrait);
				}

				// Get the biome preferences
				foreach (BiomePreferenceListTrait biomePreferenceList in allBiomePreferences)
				{
					foreach (BiomePreferenceListTrait.BiomePreference biomePreference in biomePreferenceList)
					{
						string internalName = biomePreference.Biome.NameKey;
						string langName = "";
						string displayName = "";
						if (internalName.StartsWith("Mods.")) // Modded Biomes
						{
							// They are already their full localization key, so just use that to get the name.
							langName = Language.GetTextValue(internalName);
						}
						else // Vanilla biomes
						{
							// Get the vanilla localization for the biome.
							langName = Language.GetTextValue($"TownNPCMoodBiomes.{internalName}");
						}
						// The text will be like "the Forest". Try to split by the space and take the last bit.
						string[] splitName = langName.Split(' ');
						if (splitName.Length > 1) // If the split split the string into at least 2 elements:
						{
							for (int i = 1; i < splitName.Length; i++) // Re-assemble the strings but excluding the first element.
							{
								displayName += $"{splitName[i]}";
								if (i != splitName.Length - 1)
								{
									displayName += " "; // Add a space between the words unless it is the last word.
								}
							}
						}
						else
						{
							displayName = langName; // If the localized biome name was just one word, then use that.
						}

						// Add the biome to the lists.
						if (biomePreference.Affection == AffectionLevel.Love)
						{
							loveList.Add(displayName);
						}
						else if (biomePreference.Affection == AffectionLevel.Like)
						{
							likeList.Add(displayName);
						}
						else if (biomePreference.Affection == AffectionLevel.Dislike)
						{
							dislikeList.Add(displayName);
						}
						else if (biomePreference.Affection == AffectionLevel.Hate)
						{
							hateList.Add(displayName);
						}
					}
				}

				// Get the NPC preferences
				foreach (NPCPreferenceTrait npcPreference in allNPCPreferences)
				{
					string internalName = NPCID.Search.GetName(npcPreference.NpcId); // Get the internal name of the NPC
					string displayName = "";
					if (npcPreference.NpcId >= NPCID.Count) // Modded NPC
					{
						// If Modded NPC, their name will be ExampleMod/ExamplePerson
						string[] split = internalName.Split('/');
						displayName = Language.GetTextValue($"Mods.{split[0]}.NPCs.{split[^1]}.DisplayName");
					}
					else // Vanilla NPC
					{
						displayName = Language.GetTextValue($"NPCName.{internalName}");
					}

					// Add the NPC to the lists.
					if (npcPreference.Level == AffectionLevel.Love)
					{
						loveList.Add(displayName);
					}
					else if (npcPreference.Level == AffectionLevel.Like)
					{
						likeList.Add(displayName);
					}
					else if (npcPreference.Level == AffectionLevel.Dislike)
					{
						dislikeList.Add(displayName);
					}
					else if (npcPreference.Level == AffectionLevel.Hate)
					{
						hateList.Add(displayName);
					}
				}
				return true; // Found something.
			}
			else
			{
				// The Princess does not register preferences in the same way, so it ends up being empty.
				if (npc.type == NPCID.Princess)
				{
					return true; // Return true for Princess anyway.
				}
				ModContent.GetInstance<HappinessListing>().Logger.Warn($"Failed to get personality profile for {npc.type} {npc.FullName}");
				return false;
			}
		}

		/// <summary>
		/// Formats the entire string for the Bestiary info box based on the config.
		/// </summary>
		/// <param name="npc">The NPC instance.</param>
		/// <returns>The info box string.</returns>
		public static string FormatNPCPreferences(NPC npc)
		{
			// Get the preferences formatted in lists by affection type.
			if (TryGetNPCPreferences(npc, out List<string> loveList, out List<string> likeList, out List<string> dislikeList, out List<string> hateList))
			{
				HappinessListingConfig.BestiaryPreferencesEntry config = ModContent.GetInstance<HappinessListingConfig>().BestiaryFlavorTextEntry;
				StringBuilder stringBuilder = new();
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.TextGYOR)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						$"[c/b3f2b3:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Love")}]", 
						$"[c/ddf2b3:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Like")}]", 
						$"[c/f2e0b3:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Dislike")}]", 
						$"[c/f2b5b3:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Hate")}]");
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.TextYGBP)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						$"[c/f2d45a:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Love")}]",
						$"[c/9de997:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Like")}]",
						$"[c/97dbe4:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Dislike")}]",
						$"[c/bbb2e3:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Hate")}]");
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.TextGrayscale)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						$"[c/ffffff:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Love")}]",
						$"[c/dddddd:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Like")}]",
						$"[c/bbbbbb:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Dislike")}]",
						$"[c/999999:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Hate")}]");
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.TextDialogPanelRework)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						$"[c/{Color.LawnGreen.Hex3()}:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Love")}]",
						$"[c/{Color.CornflowerBlue.Hex3()}:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Like")}]",
						$"[c/{Color.BurlyWood.Hex3()}:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Dislike")}]",
						$"[c/{Color.MediumVioletRed.Hex3()}:{Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.Hate")}]");
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.Emoticons)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						":D",
						":)",
						":(",
						":'(");
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.ChecksAndXs)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						"✔",
						"✓",
						"✗",
						"✘");
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.HappinessIcon)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						HappinessIconChatTag.Love,
						HappinessIconChatTag.Like,
						HappinessIconChatTag.Dislike,
						HappinessIconChatTag.Hate);
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.Potions)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						"[i:2352]",
						"[i:293]",
						"[i:2353]",
						"[i:678]");
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.Pickaxes)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						"[i:2786]",
						"[i:990]",
						"[i:103]",
						"[i:3509]");
				}
				if (config == HappinessListingConfig.BestiaryPreferencesEntry.MathSymbols)
				{
					FormatAllAffectionLevels(npc, ref stringBuilder, loveList, likeList, dislikeList, hateList,
						"×",
						"+",
						"-",
						"÷");
				}
				return stringBuilder.ToString(); // Return the completed string.
			}
			else
			{
				ModContent.GetInstance<HappinessListing>().Logger.Warn($"Failed to format happiness preferences {npc.type} {npc.FullName}");
				return Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.RetreivalError");
			}
		}

		/// <summary>
		/// Take all of the lists and prefixes and create the string builder.
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="stringBuilder"></param>
		/// <param name="loveList"></param>
		/// <param name="likeList"></param>
		/// <param name="dislikeList"></param>
		/// <param name="hateList"></param>
		/// <param name="lovePrefix"></param>
		/// <param name="likePrefix"></param>
		/// <param name="dislikePrefix"></param>
		/// <param name="hatePrefix"></param>
		public static void FormatAllAffectionLevels(NPC npc, ref StringBuilder stringBuilder,
			List<string> loveList, List<string> likeList, List<string> dislikeList, List<string> hateList,
			string lovePrefix, string likePrefix, string dislikePrefix, string hatePrefix)
		{
			if (npc.type == NPCID.Princess)
			{
				FormatForPrincess(lovePrefix, ref stringBuilder);
				return;
			}
			FormatAffectionLevel(loveList, lovePrefix, ref stringBuilder);
			FormatAffectionLevel(likeList, likePrefix, ref stringBuilder);
			FormatAffectionLevel(dislikeList, dislikePrefix, ref stringBuilder);
			FormatAffectionLevel(hateList, hatePrefix, ref stringBuilder);
		}

		/// <summary>
		/// Take the lists of preferences and format them with the prefix.
		/// </summary>
		/// <param name="affectionList"></param>
		/// <param name="prefix"></param>
		/// <param name="stringBuilder"></param>
		public static void FormatAffectionLevel(List<string> affectionList, string prefix, ref StringBuilder stringBuilder)
		{
			stringBuilder.Append($"{prefix}:"); // Add the prefix
			if (affectionList.Count == 0)
			{
				stringBuilder.Append($" {Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.None")}."); // If the list was empty, add the word None.
				// if (ModContent.GetInstance<HappinessListingConfig>().BreakEntriesWithNewLine)
				// {
					stringBuilder.Append('\n'); // New line.
				// }
			}
			else
			{
				for (int i = 0; i < affectionList.Count; i++) // Add each item in the list
				{
					if (i == affectionList.Count - 1)
					{
						stringBuilder.Append($" {affectionList[i]}."); // If it is the last element in the list, end it with a period and a new line.
						// if (ModContent.GetInstance<HappinessListingConfig>().BreakEntriesWithNewLine)
						// {
							stringBuilder.Append('\n');
						// }
					}
					else
					{
						stringBuilder.Append($" {affectionList[i]},"); // If it is not the last element in the list, end it with a comma.
					}
				}
			}
		}

		/// <summary>
		/// Special case for the Princess that simply says she loves all Town NPCs.
		/// </summary>
		/// <param name="prefix">The love prefix to show.</param>
		/// <param name="stringBuilder"></param>
		public static void FormatForPrincess(string prefix, ref StringBuilder stringBuilder)
		{
			stringBuilder.Append($"{prefix}: {Language.GetTextValue("Mods.HappinessListing.UI.HappinessWords.PrincessLovesAll")}");
		}
	}
}
