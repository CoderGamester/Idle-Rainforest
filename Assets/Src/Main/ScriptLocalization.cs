using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static class Achievements
		{
			public static string AutomateTree 		{ get{ return LocalizationManager.GetTranslation ("Achievements/AutomateTree"); } }
			public static string CollectCards 		{ get{ return LocalizationManager.GetTranslation ("Achievements/CollectCards"); } }
			public static string CollectMainCurrency 		{ get{ return LocalizationManager.GetTranslation ("Achievements/CollectMainCurrency"); } }
			public static string CollectSoftCurrency 		{ get{ return LocalizationManager.GetTranslation ("Achievements/CollectSoftCurrency"); } }
			public static string RankUpTree 		{ get{ return LocalizationManager.GetTranslation ("Achievements/RankUpTree"); } }
			public static string SpendMainCurrency 		{ get{ return LocalizationManager.GetTranslation ("Achievements/SpendMainCurrency"); } }
			public static string SpendSoftCurrency 		{ get{ return LocalizationManager.GetTranslation ("Achievements/SpendSoftCurrency"); } }
			public static string UpgradeAnimal 		{ get{ return LocalizationManager.GetTranslation ("Achievements/UpgradeAnimal"); } }
			public static string UpgradeLevelTree 		{ get{ return LocalizationManager.GetTranslation ("Achievements/UpgradeLevelTree"); } }
			public static string UpgradeTree 		{ get{ return LocalizationManager.GetTranslation ("Achievements/UpgradeTree"); } }
		}

		public static class GameIds
		{
			public static string AppleTree 		{ get{ return LocalizationManager.GetTranslation ("GameIds/AppleTree"); } }
			public static string Blackbird 		{ get{ return LocalizationManager.GetTranslation ("GameIds/Blackbird"); } }
			public static string ChristmasTree 		{ get{ return LocalizationManager.GetTranslation ("GameIds/ChristmasTree"); } }
			public static string Fruitbat 		{ get{ return LocalizationManager.GetTranslation ("GameIds/Fruitbat"); } }
			public static string HardCurrency 		{ get{ return LocalizationManager.GetTranslation ("GameIds/HardCurrency"); } }
			public static string MainCurrency 		{ get{ return LocalizationManager.GetTranslation ("GameIds/MainCurrency"); } }
			public static string NormalTree 		{ get{ return LocalizationManager.GetTranslation ("GameIds/NormalTree"); } }
			public static string SoftCurrency 		{ get{ return LocalizationManager.GetTranslation ("GameIds/SoftCurrency"); } }
			public static string Squirrel 		{ get{ return LocalizationManager.GetTranslation ("GameIds/Squirrel"); } }
		}

		public static class General
		{
			public static string Automate 		{ get{ return LocalizationManager.GetTranslation ("General/Automate"); } }
			public static string AutomateRequireParam 		{ get{ return LocalizationManager.GetTranslation ("General/AutomateRequireParam"); } }
			public static string Automated 		{ get{ return LocalizationManager.GetTranslation ("General/Automated"); } }
			public static string Cards 		{ get{ return LocalizationManager.GetTranslation ("General/Cards"); } }
			public static string Collect 		{ get{ return LocalizationManager.GetTranslation ("General/Collect"); } }
			public static string Free 		{ get{ return LocalizationManager.GetTranslation ("General/Free"); } }
			public static string LevelParam 		{ get{ return LocalizationManager.GetTranslation ("General/LevelParam"); } }
			public static string Loading 		{ get{ return LocalizationManager.GetTranslation ("General/Loading"); } }
			public static string Max 		{ get{ return LocalizationManager.GetTranslation ("General/Max"); } }
		}
	}

    public static class ScriptTerms
	{

		public static class Achievements
		{
		    public const string AutomateTree = "Achievements/AutomateTree";
		    public const string CollectCards = "Achievements/CollectCards";
		    public const string CollectMainCurrency = "Achievements/CollectMainCurrency";
		    public const string CollectSoftCurrency = "Achievements/CollectSoftCurrency";
		    public const string RankUpTree = "Achievements/RankUpTree";
		    public const string SpendMainCurrency = "Achievements/SpendMainCurrency";
		    public const string SpendSoftCurrency = "Achievements/SpendSoftCurrency";
		    public const string UpgradeAnimal = "Achievements/UpgradeAnimal";
		    public const string UpgradeLevelTree = "Achievements/UpgradeLevelTree";
		    public const string UpgradeTree = "Achievements/UpgradeTree";
		}

		public static class GameIds
		{
		    public const string AppleTree = "GameIds/AppleTree";
		    public const string Blackbird = "GameIds/Blackbird";
		    public const string ChristmasTree = "GameIds/ChristmasTree";
		    public const string Fruitbat = "GameIds/Fruitbat";
		    public const string HardCurrency = "GameIds/HardCurrency";
		    public const string MainCurrency = "GameIds/MainCurrency";
		    public const string NormalTree = "GameIds/NormalTree";
		    public const string SoftCurrency = "GameIds/SoftCurrency";
		    public const string Squirrel = "GameIds/Squirrel";
		}

		public static class General
		{
		    public const string Automate = "General/Automate";
		    public const string AutomateRequireParam = "General/AutomateRequireParam";
		    public const string Automated = "General/Automated";
		    public const string Cards = "General/Cards";
		    public const string Collect = "General/Collect";
		    public const string Free = "General/Free";
		    public const string LevelParam = "General/LevelParam";
		    public const string Loading = "General/Loading";
		    public const string Max = "General/Max";
		}
	}
}