using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace HappinessListing
{
	public class HappinessListingEdits : ModSystem
	{
		public override void Load()
		{
#if TML144
			Terraria.GameContent.On_ShopHelper.AddHappinessReportText += On_ShopHelper_AddHappinessReportText;
#endif
			Terraria.On_NPC.GetChat += On_NPC_GetChat;
			MethodInfo method_AddHappinessReportText = typeof(ShopHelper).GetMethod("AddHappinessReportText", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo method_AddHappinessReportTextWithKey = typeof(ShopHelper).GetMethod("AddHappinessReportTextWithKey", BindingFlags.Instance | BindingFlags.NonPublic);
			MonoModHooks.Add(method_AddHappinessReportText, On_ShopHelper_AddHappinessReportText);
			MonoModHooks.Add(method_AddHappinessReportTextWithKey, On_ShopHelper_AddHappinessReportTextWithKey);
			MethodInfo method_PrepareCache = typeof(TextDisplayCache).GetMethod("PrepareCache", BindingFlags.Instance | BindingFlags.Public);
			MonoModHooks.Modify(method_PrepareCache, IL_TextDisplayCache_PrepareCache);
			// IL_TextDisplayCache.PrepareCache += IL_TextDisplayCache_PrepareCache;
#if TML144
			Terraria.IL_Main.TextDisplayCache.PrepareCache += TextDisplayCache_PrepareCache;
#endif
			Terraria.GameContent.Personalities.IL_AllPersonalitiesModifier.ModifyShopPrice_Relationships += IL_AllPersonalitiesModifier_ModifyShopPrice_Relationships;

			// On_TextDisplayCache.PrepareCache += On_TextDisplayCache_PrepareCache;
		}

		/* For detouring TextDisplayCache.PrepareCache
		public static string GetSet_TextDisplayCache__originalText(TextDisplayCache textDisplayCacheInstance, string? newValue = null)
		{
			FieldInfo field = typeof(Terraria.GameContent.UI.TextDisplayCache).GetField("_originalText", BindingFlags.NonPublic | BindingFlags.Instance);

			if (newValue != null)
			{
				field.SetValue(textDisplayCacheInstance, newValue);
			}

			return (string)field.GetValue(textDisplayCacheInstance);
		}

		public static int GetSet_TextDisplayCache__lastScreenWidth(TextDisplayCache textDisplayCacheInstance, int? newValue = null)
		{
			FieldInfo field = typeof(Terraria.GameContent.UI.TextDisplayCache).GetField("_lastScreenWidth", BindingFlags.NonPublic | BindingFlags.Instance);

			if (newValue != null)
			{
				field.SetValue(textDisplayCacheInstance, newValue);
			}

			return (int)field.GetValue(textDisplayCacheInstance);
		}

		public static int GetSet_TextDisplayCache__lastScreenHeight(TextDisplayCache textDisplayCacheInstance, int? newValue = null)
		{
			FieldInfo field = typeof(Terraria.GameContent.UI.TextDisplayCache).GetField("_lastScreenHeight", BindingFlags.NonPublic | BindingFlags.Instance);

			if (newValue != null)
			{
				field.SetValue(textDisplayCacheInstance, newValue);
			}

			return (int)field.GetValue(textDisplayCacheInstance);
		}

		public static InputMode GetSet_TextDisplayCache__lastInputMode(TextDisplayCache textDisplayCacheInstance, InputMode? newValue = null)
		{
			FieldInfo field = typeof(Terraria.GameContent.UI.TextDisplayCache).GetField("_lastInputMode", BindingFlags.NonPublic | BindingFlags.Instance);

			if (newValue != null)
			{
				field.SetValue(textDisplayCacheInstance, newValue);
			}

			return (InputMode)field.GetValue(textDisplayCacheInstance);
		}

		public static string[] GetSet_TextDisplayCache_TextLines(TextDisplayCache textDisplayCacheInstance, string[]? newValue = null)
		{
			PropertyInfo property = typeof(Terraria.GameContent.UI.TextDisplayCache).GetProperty("TextLines", BindingFlags.Public | BindingFlags.Instance);

			if (newValue != null)
			{
				property.SetValue(textDisplayCacheInstance, newValue);
			}

			return (string[])property.GetValue(textDisplayCacheInstance);
		}

		public static int GetSet_TextDisplayCache_AmountOfLines(TextDisplayCache textDisplayCacheInstance, int? newValue = null)
		{
			PropertyInfo property = typeof(Terraria.GameContent.UI.TextDisplayCache).GetProperty("AmountOfLines", BindingFlags.Public | BindingFlags.Instance);

			if (newValue != null)
			{
				property.SetValue(textDisplayCacheInstance, newValue);
			}

			return (int)property.GetValue(textDisplayCacheInstance);
		}

		private void On_TextDisplayCache_PrepareCache(On_TextDisplayCache.orig_PrepareCache orig, TextDisplayCache self, string text)
		{
			// original code. Editing that 10 down there which is the max lines
			// if (false | (Main.screenWidth != _lastScreenWidth) | (Main.screenHeight != _lastScreenHeight) | (_originalText != text) | (PlayerInput.CurrentInputMode != _lastInputMode)) {
			//	_lastScreenWidth = Main.screenWidth;
			//	_lastScreenHeight = Main.screenHeight;
			//	_originalText = text;
			//	_lastInputMode = PlayerInput.CurrentInputMode;
			//	TextLines = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460, 10, out var lineAmount);
			//	AmountOfLines = lineAmount;
			// } 
			
			int _lastScreenWidth = GetSet_TextDisplayCache__lastScreenWidth(self);
			int _lastScreenHeight = GetSet_TextDisplayCache__lastScreenHeight(self);
			string _originalText = GetSet_TextDisplayCache__originalText(self);
			InputMode _lastInputMode = GetSet_TextDisplayCache__lastInputMode(self);

			if (false | (Main.screenWidth != _lastScreenWidth) | (Main.screenHeight != _lastScreenHeight) | (_originalText != text) | (PlayerInput.CurrentInputMode != _lastInputMode))
			{
				GetSet_TextDisplayCache__lastScreenWidth(self, Main.screenWidth);
				GetSet_TextDisplayCache__lastScreenHeight(self, Main.screenHeight);
				GetSet_TextDisplayCache__originalText(self, text);
				GetSet_TextDisplayCache__lastInputMode(self, PlayerInput.CurrentInputMode);
				//TextLines = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460, 10, out var lineAmount);
				int maxLines = ModContent.GetInstance<HappinessListingConfig>().MaxTextLines;
				
				// List<List<TextSnippet>> words = Utils.WordwrapStringSmart(text, Color.White, FontAssets.MouseText.Value, 460, maxLines);
				// string[] linesArray = new string[maxLines];
				// for (int i = 0; i < words.Count; i++)
				// {
				//	ModContent.GetInstance<HappinessListing>().Logger.Debug($"words[{i}] {words[i].Count}");

				//	for (int j = 0; j < words[i].Count; j++)
				//	{
				//		linesArray[i] = words[i][j].TextOriginal;
				//		ModContent.GetInstance<HappinessListing>().Logger.Debug($"words[{i}][{j}] {words[i][j]}");
				//	}
				// }
				// foreach (string line in linesArray)
				// {
				//	ModContent.GetInstance<HappinessListing>().Logger.Debug($"line {line}");
				// }
				// GetSet_TextDisplayCache_TextLines(linesArray);
				// GetSet_TextDisplayCache_AmountOfLines(Math.Min(words[0].Count, maxLines));

				string[] linesArray = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460, maxLines, out var lineAmount);
				foreach (string line in linesArray)
				{
					ModContent.GetInstance<HappinessListing>().Logger.Debug($"line {line}");
				}
				GetSet_TextDisplayCache_TextLines(self, linesArray);
				GetSet_TextDisplayCache_AmountOfLines(self, lineAmount);
			}
		}
		*/

		private void IL_TextDisplayCache_PrepareCache(ILContext il)
		{
			Type type_Main_TextDisplayCache = typeof(Main).GetNestedType("TextDisplayCache", BindingFlags.NonPublic);

			/* original code.
			 * Editing that 10 down there which is the max lines
			if (false | (Main.screenWidth != _lastScreenWidth) | (Main.screenHeight != _lastScreenHeight) | (_originalText != text) | (PlayerInput.CurrentInputMode != _lastInputMode)) {
				_lastScreenWidth = Main.screenWidth;
				_lastScreenHeight = Main.screenHeight;
				_originalText = text;
				_lastInputMode = PlayerInput.CurrentInputMode;
				TextLines = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460, 10, out var lineAmount);
				AmountOfLines = lineAmount;
			} 
			*/

			ILCursor c = new(il);

			// Try to find where 10 is placed onto the stack
			// This 10 is the max lines.

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(10)))
			{
				ModContent.GetInstance<HappinessListing>().Logger.Debug("Patch 2 of IL_TextDisplayCache_PrepareCache unable to be applied! ");
				return; // Patch unable to be applied
			}
			c.Emit(OpCodes.Pop); // Pop the 10.
			c.Emit(OpCodes.Ldc_I4, ModContent.GetInstance<HappinessListingConfig>().MaxTextLines); // Push the config value in its place.
		}

#if Disabled
		/// <summary>
		/// Does 2 IL edits to change the width and the max lines of the dialogue panel.
		/// </summary>
		/// <param name="il"></param>
		private void IL_TextDisplayCache_PrepareCache(ILContext il)
		{
			/* original code.
			 * Editing that 460 down there which is the max width
			 * Editing that 10 down there which is the max lines
			if (false | (Main.screenWidth != _lastScreenWidth) | (Main.screenHeight != _lastScreenHeight) | (_originalText != text) | (PlayerInput.CurrentInputMode != _lastInputMode)) {
				_lastScreenWidth = Main.screenWidth;
				_lastScreenHeight = Main.screenHeight;
				_originalText = text;
				_lastInputMode = PlayerInput.CurrentInputMode;
				TextLines = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460, 10, out var lineAmount);
				AmountOfLines = lineAmount;
			} 
			*/

			ILCursor c = new(il);

			// Try to find where 460 is placed onto the stack
			// This 460 is the width of the wrapping
			if (!c.TryGotoNext(i => i.MatchLdcI4(460)))
			{
				ModContent.GetInstance<HappinessListing>().Logger.Debug("Patch 1 of IL_TextDisplayCache_PrepareCache unable to be applied! ");
				return; // Patch unable to be applied
			}

			// Move the cursor after 460 and onto the ret op.
			c.Index++;
			// Push the TextDisplayCache instance onto the stack
			c.Emit(OpCodes.Ldarg_0);
			// Call a delegate using the int and TextDisplayCache from the stack.
			c.EmitDelegate<Func<int, TextDisplayCache, int>>((returnValue, textDisplayCache) =>
			{
				// Regular c# code
				// Change the 460 to something larger if the Happiness Icons are being used.

				int width = 460;

				if (ModContent.GetInstance<HappinessListingConfig>().EntryIconType == HappinessListingConfig.IconType.HappinessIcon &&
					Main.LocalPlayer.currentShoppingSettings.HappinessReport == Main.npcChatText)
				{
					width = 480;
				}

				return width;
			});

			// Try to find where 10 is placed onto the stack
			// This 10 is the max lines.
			if (!c.TryGotoNext(i => i.MatchLdcI4(10)))
			{
				ModContent.GetInstance<HappinessListing>().Logger.Debug("Patch 2 of IL_TextDisplayCache_PrepareCache unable to be applied! ");
				return; // Patch unable to be applied
			}

			// Move the cursor after 10 and onto the ret op.
			c.Index++;
			// Push the TextDisplayCache instance onto the stack
			c.Emit(OpCodes.Ldarg_0);
			// Call a delegate using the int and TextDisplayCache from the stack.
			c.EmitDelegate<Func<int, TextDisplayCache, int>>((returnValue, textDisplayCache) =>
			{
				// Regular c# code

				// Change the 10 to the MaxTextLines config value.

				return ModContent.GetInstance<HappinessListingConfig>().MaxTextLines;
			});
		}
#endif

		/// <summary>
		/// Does an IL edit to change the limit of 3 in the second for loop for the Princess.
		/// </summary>
		/// <param name="il"></param>
		private void IL_AllPersonalitiesModifier_ModifyShopPrice_Relationships(ILContext il)
		{
			/* original code
			 * Editing the 3 in the second for loop
			bool[] nearbyNPCsByType = info.nearbyNPCsByType;

			// Princess code
			if (info.npc.type == 663) {
				List<int> list = new List<int>();
				for (int i = 0; i < nearbyNPCsByType.Length; i++) {
					if (nearbyNPCsByType[i])
						list.Add(i);
				}

				for (int j = 0; j < 3; j++) {
					if (list.Count <= 0)
						break;

					int index = Main.rand.Next(list.Count);
					int npcType = list[index];
					list.RemoveAt(index);
					// shopHelperInstance.LoveNPCByTypeName(npcType);
					shopHelperInstance.ApplyNpcRelationshipEffect(npcType, AffectionLevel.Love);
				}
			}
			*/

			ILCursor c = new(il);

			// Try to find where 3 is placed onto the stack
			// This 3 is the number of NPCs the Princess will talk about at once.
			if (!c.TryGotoNext(i => i.MatchLdcI4(3)))
			{
				ModContent.GetInstance<HappinessListing>().Logger.Debug("Patch 1 of IL_AllPersonalitiesModifier_ModifyShopPrice_Relationships unable to be applied! ");
				return; // Patch unable to be applied
			}

			// Move the cursor after 3 and onto the ret op.
			c.Index++;
			// DON'T Push the AllPersonalitiesModifier instance onto the stack. The method is static.
			// c.Emit(OpCodes.Ldarg_0);
			// Call a delegate using the int and AllPersonalitiesModifier from the stack.
			c.EmitDelegate<Func<int, int>>((returnValue) =>
			{
				// Regular c# code
				// Change the 3 to the MaxNumberOfPeoplePrincessCanTalkAboutAtOnce value

				return ModContent.GetInstance<HappinessListingConfig>().MaxNumberOfPeoplePrincessCanTalkAboutAtOnce;
			});
		}

		private static readonly FieldInfo Field__currentHappiness = typeof(Terraria.GameContent.ShopHelper).GetField("_currentHappiness", BindingFlags.NonPublic | BindingFlags.Instance);

		/// <summary> Gets or Sets ShopHelper._currentHappiness </summary>
		public static string GetSet_ShopHelper__currentHappiness(string newValue = null)
		{
			if (newValue != null)
			{
				Field__currentHappiness.SetValue(Main.ShopHelper, newValue);
			}

			return (string)Field__currentHappiness.GetValue(Main.ShopHelper);
		}

		private static readonly FieldInfo Field__currentNPCBeingTalkedTo = typeof(Terraria.GameContent.ShopHelper).GetField("_currentNPCBeingTalkedTo", BindingFlags.NonPublic | BindingFlags.Instance);

		/// <summary> Gets ShopHelper._currentNPCBeingTalkedTo </summary>
		public static NPC Get_ShopHelper__currentNPCBeingTalkedTo()
		{
			return (NPC)Field__currentNPCBeingTalkedTo.GetValue(Main.ShopHelper);
		}

		private static readonly FieldInfo Field__database = typeof(Terraria.GameContent.ShopHelper).GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance);

		/// <summary> Gets ShopHelper._database </summary>
		public static PersonalityDatabase Get_ShopHelper__database(ShopHelper shopHelperInstance)
		{
			return (PersonalityDatabase)Field__database.GetValue(shopHelperInstance);
		}

		private static readonly FieldInfo Field__handlers = typeof(Terraria.UI.Chat.ChatManager).GetField("_handlers", BindingFlags.NonPublic | BindingFlags.Static);

		/// <summary> Gets ChatManager._handlers </summary>
		public static ConcurrentDictionary<string, ITagHandler> Get_ChatManager__handlers()
		{

			return (ConcurrentDictionary<string, ITagHandler>)Field__handlers.GetValue(null);
		}

#if TML144
		/// <summary>
		/// Detours the method that adds the dialogue entry to the happiness menu.
		/// </summary>
		private void On_ShopHelper_AddHappinessReportText(On_ShopHelper.orig_AddHappinessReportText orig, ShopHelper self, string textKeyInCategory, object substitutes, int otherNPCType)
		{
			bool skipOrig = LineEntryModifications.ApplyPreEntryModifications(textKeyInCategory, substitutes);
			if (!skipOrig)
			{
				orig(self, textKeyInCategory, substitutes, otherNPCType); // Run the original code
			}
			LineEntryModifications.ApplyPostEntryModifications();
		}
#endif

		private delegate void orig_AddHappinessReportText(ShopHelper self, string textKeyInCategory, object substitutes = null);

		/// <summary>
		/// Detours the method that adds the dialogue entry to the happiness menu.
		/// </summary>
		private void On_ShopHelper_AddHappinessReportText(orig_AddHappinessReportText orig, ShopHelper self, string textKeyInCategory, object substitutes)
		{
			bool skipOrig = LineEntryModifications.ApplyPreEntryModifications(textKeyInCategory, substitutes);
			if (!skipOrig)
			{
				orig(self, textKeyInCategory, substitutes); // Run the original code
			}
			LineEntryModifications.ApplyPostEntryModifications();
		}

		private delegate void orig_AddHappinessReportTextWithKey(ShopHelper self, string textKey, string textKeyInCategory, object substitutes = null, int otherNPCType = 0);

		/// <summary>
		/// Detours the method that adds the dialogue entry to the happiness menu.
		/// </summary>
		private void On_ShopHelper_AddHappinessReportTextWithKey(orig_AddHappinessReportTextWithKey orig, ShopHelper self, string textKey, string textKeyInCategory, object substitutes = null, int otherNPCType = 0)
		{
			bool skipOrig = LineEntryModifications.ApplyPreEntryModifications(textKeyInCategory, substitutes);
			if (!skipOrig)
			{
				orig(self, textKey, textKeyInCategory, substitutes, otherNPCType); // Run the original code
			}
			LineEntryModifications.ApplyPostEntryModifications();
		}

		/// <summary>
		/// Detours the method that gets the regular dialogue chat message.
		/// </summary>
		private string On_NPC_GetChat(On_NPC.orig_GetChat orig, NPC self)
		{
			string regularDialogue = RegularDialogueModifications.ApplyPreEntryModificationsForRegularDialogue();
			regularDialogue += orig(self); // Run the original code
			RegularDialogueModifications.ApplyPostEntryModificationsForRegularDialogue(ref regularDialogue);
			return regularDialogue;
		}
	}
}
