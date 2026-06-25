using System.ComponentModel;
using Newtonsoft.Json;
using Terraria.ModLoader.Config;

namespace HappinessListing
{
	public class HappinessListingConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("DialogueOptions")]

		[DefaultValue(true)]
		public bool BreakEntriesWithNewLine { get; set; }

		[DefaultValue(IconType.HappinessIcon)]
		public IconType EntryIconType { get; set; }

#if TML145
		[Range(1, 50)]
		[DefaultValue(20)]
#endif
		// 1.4.4: The GUI looks weird with more than 16 lines and only 20 lines can render.
		[Range(1, 20)]
		[DefaultValue(16)]
		[Slider]
		public int MaxTextLines { get; set; }

		[DefaultValue(ColorCoding.HappinessYGBP)]
		// [JsonIgnore]
		public ColorCoding ColorCodeText { get; set; } // 1.4.5: Disabled for now because the color chat tags break on new lines.

		[Range(1, 20)]
		[DefaultValue(5)]
		[Slider]
		public int MaxNumberOfPeoplePrincessCanTalkAboutAtOnce { get; set; }

		[DefaultValue(SimplifiedDialogueSetting.Off)]
		public SimplifiedDialogueSetting SimplifiedHappinessDialogue { get; set; }

		[Header("BestiaryOptions")]

		[DefaultValue(BestiaryPreferencesEntry.HappinessIcon)]
		[ReloadRequired]
		public BestiaryPreferencesEntry BestiaryFlavorTextEntry { get; set; }

		public enum IconType
		{
			None,
			Asterick,
			Bullet,
			RightArrow,
			GreaterThan,
			Tilde,
			Emoticons,
			ChecksAndXs,
			HappinessIcon,
			Potions,
			Pickaxes,
			MathSymbols
		}

		public enum ColorCoding
		{
			None,
			HappinessYGBP,
			HappinessGYOR,
			HappinessGrayscale,
			HappinessDialogPanelRework
		}

		public enum BestiaryPreferencesEntry
		{
			None,
			TextYGBP,
			TextGYOR,
			TextGrayscale,
			TextDialogPanelRework,
			Emoticons,
			ChecksAndXs,
			HappinessIcon,
			Potions,
			Pickaxes,
			MathSymbols
		}

		public enum SimplifiedDialogueSetting
		{
			Off,
			VanillaOnly,
			ModdedOnly,
			All
		}
	}
}