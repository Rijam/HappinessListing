using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Concurrent;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace HappinessListing
{
	public class HappinessListing : Mod
	{
		/// <summary> 1.4.5 Asset for the Happiness icons </summary>
		public Asset<Texture2D> NPCHappinessTexture;

		public override void Load()
		{
			ChatManager.Register<HappinessIconChatTag>(HappinessIconChatTag.HLHappinessIcon);
			NPCHappinessTexture ??= Main.Assets.Request<Texture2D>("Images\\UI\\NPCHappiness", AssetRequestMode.AsyncLoad); // Load the Happiness icons from vanilla 1.4.5
		}

		public override void Unload()
		{
			// From Magic Storage https://github.com/blushiemagic/MagicStorage/blob/1.4.4/TagHandlers/RecipeGroupTagHandler.cs#L71
			ConcurrentDictionary<string, ITagHandler> handler = HappinessListingEdits.Get_ChatManager__handlers();
			handler.TryRemove(HappinessIconChatTag.HLHappinessIcon, out _);

			BestiaryEntryModifications.NPCBlackList = null;
			LineEntryModifications.NPCBlackList = null;
		}

		public override object Call(params object[] args)
		{
			// How to use in your mod:
			// if (ModLoader.TryGetMod("HappinessListing", out Mod happinessListing))
			// {
			//	bool newLineConfig = (bool)happinessListing.Call("BreakEntriesWithNewLine");
			// }

			ArgumentNullException.ThrowIfNull(args);

			if (args[0] is not string function)
			{
				throw new ArgumentException("Expected a function name for the first argument");
			}

			switch (function)
			{
				// Config value for "Break Entries With New Line"
				case "BreakEntriesWithNewLine":
					return ModContent.GetInstance<HappinessListingConfig>().BreakEntriesWithNewLine; // Bool

				// Config value for "Entry Icon Type"
				case "EntryIconType":
					return ModContent.GetInstance<HappinessListingConfig>().EntryIconType; // Enum
				case "EntryIconTypeToString":
					return ModContent.GetInstance<HappinessListingConfig>().EntryIconType.ToString(); // String

				// Config value for "Maximum Text Lines"
				case "MaxTextLines":
				case "MaximumTextLines":
					return ModContent.GetInstance<HappinessListingConfig>().MaxTextLines; // Int

				// Config value for "Color Code Dialogue"
				case "ColorCodeText":
				case "ColorCodeDialogue":
					return ModContent.GetInstance<HappinessListingConfig>().ColorCodeText; // Enum
				case "ColorCodeTextToString":
				case "ColorCodeDialogueToString":
					return ModContent.GetInstance<HappinessListingConfig>().ColorCodeText.ToString(); // String

				// Config value for "Princess Dialogue Maximum"
				case "MaxNumberOfPeoplePrincessCanTalkAboutAtOnce":
				case "PrincessDialogueMaximum":
					return ModContent.GetInstance<HappinessListingConfig>().MaxNumberOfPeoplePrincessCanTalkAboutAtOnce; // Int

				// Config value for "Simplified Happiness Dialogue"
				case "SimplifiedHappinessDialogue":
					return ModContent.GetInstance<HappinessListingConfig>().SimplifiedHappinessDialogue; // Enum
				case "SimplifiedHappinessDialogueToString":
					return ModContent.GetInstance<HappinessListingConfig>().SimplifiedHappinessDialogue.ToString(); // String

				// Config value for "Bestiary Happiness Info Box"
				case "BestiaryFlavorTextEntry":
				case "BestiaryHappinessInfoBox":
					return ModContent.GetInstance<HappinessListingConfig>().BestiaryFlavorTextEntry; // Enum
				case "BestiaryFlavorTextEntryToString":
				case "BestiaryHappinessInfoBoxToString":
					return ModContent.GetInstance<HappinessListingConfig>().BestiaryFlavorTextEntry.ToString(); // String

				// Add your NPC to the a blacklist so its happiness dialogue won't get modified.
				case "LineEntryModificationsBlackListType":
					LineEntryModifications.NPCBlackList.Add((int)args[1]);
					return LineEntryModifications.NPCBlackList.Contains((int)args[1]); // Bool: True if it was added successfully.
				case "LineEntryModificationsBlackListNPC":
					LineEntryModifications.NPCBlackList.Add(((NPC)args[1]).type);
					return LineEntryModifications.NPCBlackList.Contains(((NPC)args[1]).type); // Bool: True if it was added successfully.

				// Add your NPC to the a blacklist so it won't receive the info box with happiness information.
				case "BestiaryFlavorTextEntryBlackListType":
				case "BestiaryHappinessInfoBoxBlackListType":
					BestiaryEntryModifications.NPCBlackList.Add((int)args[1]);
					return BestiaryEntryModifications.NPCBlackList.Contains((int)args[1]); // Bool: True if it was added successfully.
				case "BestiaryFlavorTextEntryBlackListNPC":
				case "BestiaryHappinessInfoBoxBlackListNPC":
					BestiaryEntryModifications.NPCBlackList.Add(((NPC)args[1]).type);
					return BestiaryEntryModifications.NPCBlackList.Contains(((NPC)args[1]).type); // Bool: True if it was added successfully.

				// Unknown call.
				default:
					throw new ArgumentException($"Function \"{function}\" is not defined by Happiness Listing.");
			}
		}
	}
}
