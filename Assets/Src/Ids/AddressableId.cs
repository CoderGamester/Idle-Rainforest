/* AUTO GENERATED CODE */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameLovers.AssetLoader;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

namespace Ids
{
	public enum AddressableId
	{
		Levels_Forest,
		Sprites_Animals_Blackbird,
		Sprites_Animals_Fruitbat,
		Sprites_Animals_Squirrel,
		Sprites_Trees_AppleTree,
		Sprites_Trees_ChristmasTree,
		Sprites_Trees_NormalTree,
		Configs_AnimalConfigs,
		Configs_CardConfigs,
		Configs_GameConfigs,
		Configs_LevelAchievementConfigs,
		Configs_LevelTreeConfigs,
		Configs_TreeConfigs,
		Configs_UiConfigs,
		Prefabs_Tree,
		Prefabs_Ui_Animal_Cards_Panel,
		Prefabs_Ui_Automate_PopUp,
		Prefabs_Ui_Event_Panel,
		Prefabs_Ui_Hud,
		Prefabs_Ui_Loading_Screen,
		Prefabs_Ui_Tree_Cards_Panel,
		Prefabs_Vfx_Ui_Vfx
	}

	public enum AddressableLabel
	{
	}

	public static class AddressablePathLookup
	{
		public static readonly string Levels = "Levels";
		public static readonly string SpritesAnimals = "Sprites/Animals";
		public static readonly string SpritesTrees = "Sprites/Trees";
		public static readonly string Configs = "Configs";
		public static readonly string Prefabs = "Prefabs";
		public static readonly string PrefabsUi = "Prefabs/Ui";
		public static readonly string PrefabsVfxUi = "Prefabs/Vfx/Ui";
	}

	public static class AddressableConfigLookup
	{
		public static IList<AddressableConfig> Configs => _addressableConfigs;
		public static IList<string> Labels => _addressableLabels;

		public static AddressableConfig GetConfig(this AddressableId addressable)
		{
			return _addressableConfigs[(int) addressable];
		}

		public static IList<AddressableConfig> GetConfigs(this AddressableLabel label)
		{
			return _addressableLabelMap[_addressableLabels[(int) label]];
		}

		public static IList<AddressableConfig> GetConfigs(string label)
		{
			return _addressableLabelMap[label];
		}

		private static readonly IList<string> _addressableLabels = new List<string>
		{
		}.AsReadOnly();

		private static readonly IReadOnlyDictionary<string, IList<AddressableConfig>> _addressableLabelMap = new ReadOnlyDictionary<string, IList<AddressableConfig>>(new Dictionary<string, IList<AddressableConfig>>
		{
		});

		private static readonly IList<AddressableConfig> _addressableConfigs = new List<AddressableConfig>
		{
			new AddressableConfig(0, "Levels/Forest.unity", "Assets/Scenes/Levels/Forest.unity", typeof(UnityEngine.SceneManagement.Scene), new [] {""}),
			new AddressableConfig(1, "Sprites/Animals/Blackbird.png", "Assets/Art/Sprites/Animals/Blackbird.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(2, "Sprites/Animals/Fruitbat.png", "Assets/Art/Sprites/Animals/Fruitbat.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(3, "Sprites/Animals/Squirrel.png", "Assets/Art/Sprites/Animals/Squirrel.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(4, "Sprites/Trees/AppleTree.png", "Assets/Art/Sprites/Trees/AppleTree.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(5, "Sprites/Trees/ChristmasTree.png", "Assets/Art/Sprites/Trees/ChristmasTree.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(6, "Sprites/Trees/NormalTree.png", "Assets/Art/Sprites/Trees/NormalTree.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(7, "Configs/AnimalConfigs.asset", "Assets/ScriptableObjects/Configs/AnimalConfigs.asset", typeof(Configs.AnimalConfigs), new [] {""}),
			new AddressableConfig(8, "Configs/CardConfigs.asset", "Assets/ScriptableObjects/Configs/CardConfigs.asset", typeof(Configs.CardConfigs), new [] {""}),
			new AddressableConfig(9, "Configs/GameConfigs.asset", "Assets/ScriptableObjects/Configs/GameConfigs.asset", typeof(Configs.GameConfigs), new [] {""}),
			new AddressableConfig(10, "Configs/LevelAchievementConfigs.asset", "Assets/ScriptableObjects/Configs/LevelAchievementConfigs.asset", typeof(Configs.LevelAchievementConfigs), new [] {""}),
			new AddressableConfig(11, "Configs/LevelTreeConfigs.asset", "Assets/ScriptableObjects/Configs/LevelTreeConfigs.asset", typeof(Configs.LevelTreeConfigs), new [] {""}),
			new AddressableConfig(12, "Configs/TreeConfigs.asset", "Assets/ScriptableObjects/Configs/TreeConfigs.asset", typeof(Configs.TreeConfigs), new [] {""}),
			new AddressableConfig(13, "Configs/UiConfigs.asset", "Assets/ScriptableObjects/Configs/UiConfigs.asset", typeof(GameLovers.UiService.UiConfigs), new [] {""}),
			new AddressableConfig(14, "Prefabs/Tree.prefab", "Assets/Art/Prefabs/Tree.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(15, "Prefabs/Ui/Animal Cards Panel.prefab", "Assets/Art/Prefabs/Ui/Animal Cards Panel.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(16, "Prefabs/Ui/Automate PopUp.prefab", "Assets/Art/Prefabs/Ui/Automate PopUp.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(17, "Prefabs/Ui/Event Panel.prefab", "Assets/Art/Prefabs/Ui/Event Panel.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(18, "Prefabs/Ui/Hud.prefab", "Assets/Art/Prefabs/Ui/Hud.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(19, "Prefabs/Ui/Loading Screen.prefab", "Assets/Art/Prefabs/Ui/Loading Screen.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(20, "Prefabs/Ui/Tree Cards Panel.prefab", "Assets/Art/Prefabs/Ui/Tree Cards Panel.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(21, "Prefabs/Vfx/Ui/Vfx.prefab", "Assets/Art/Prefabs/Vfx/Ui/Vfx.prefab", typeof(UnityEngine.GameObject), new [] {""})
		}.AsReadOnly();
	}
}
