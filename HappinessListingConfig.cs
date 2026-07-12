using System.ComponentModel;
using Newtonsoft.Json;
using Terraria.ModLoader.Config;

namespace HappinessListing
{
	public class HappinessListingConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("HappinessDialogueOptions")]

		[DefaultValue(true)]
		public bool BreakEntriesWithNewLine { get; set; }

		[DefaultValue(IconType.HappinessIcon)]
		public IconType EntryIconType { get; set; }

		[Range(1, 50)]
		[DefaultValue(20)]
#if TML144
		// 1.4.4: The GUI looks weird with more than 16 lines and only 20 lines can render.
		[Range(1, 20)]
		[DefaultValue(16)]
#endif
		[Slider]
		public int MaxTextLines { get; set; }

		[DefaultValue(ColorCoding.HappinessYGBP)]
		// [JsonIgnore]
		public ColorCoding ColorCodeHappinessText { get; set; }

		[Range(1, 20)]
		[DefaultValue(5)]
		[Slider]
		public int MaxNumberOfPeoplePrincessCanTalkAboutAtOnce { get; set; }

		[DefaultValue(SimplifiedDialogueSetting.Off)]
		public SimplifiedDialogueSetting SimplifiedHappinessDialogue { get; set; }

		[Header("RegularDialogueOptions")]

		[DefaultValue(ColorCoding.None)]
		public ColorCoding ColorCodeRegularDialogue { get; set; }

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
			MathSymbols,
			Statues,
			HeartsAndSkulls,
			Paintings
		}

		public enum ColorCoding
		{
			None,
			HappinessYGBP,
			HappinessGYOR,
			HappinessGrayscale,
			HappinessDialogPanelRework,
			HappinessVibrant
		}

		public enum BestiaryPreferencesEntry
		{
			None,
			TextYGBP,
			TextGYOR,
			TextGrayscale,
			TextDialogPanelRework,
			TextVibrant,
			Emoticons,
			ChecksAndXs,
			HappinessIcon,
			Potions,
			Pickaxes,
			MathSymbols,
			Statues,
			HeartsAndSkulls,
			Paintings
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