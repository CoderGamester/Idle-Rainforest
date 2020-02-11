/* AUTO GENERATED CODE */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameLovers.AddressableIdsScriptGenerator;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

namespace Ids
{
	public enum AddressableId
	{
		Sprites_Trees_AppleTree,
		Sprites_Trees_ChristmasTree,
		Sprites_Trees_NormalTree,
		Configs_CardConfigs,
		Configs_LevelTreeConfigs,
		Configs_UiConfigs,
		Prefabs_Building,
		Prefabs_Ui_Cards_Panel,
		Prefabs_Ui_Event_Panel,
		Prefabs_Ui_Hud,
		Prefabs_Ui_Loading_Screen,
		Prefabs_Vfx_Ui_Vfx
	}

	public enum AddressableLabel
	{
	}

	public static class AddressablePathLookup
	{
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
			new AddressableConfig(0, "Sprites/Trees/AppleTree.png", "Assets/Art/Sprites/Trees/AppleTree.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(1, "Sprites/Trees/ChristmasTree.png", "Assets/Art/Sprites/Trees/ChristmasTree.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(2, "Sprites/Trees/NormalTree.png", "Assets/Art/Sprites/Trees/NormalTree.png", typeof(UnityEngine.Texture2D), new [] {""}),
			new AddressableConfig(3, "Configs/CardConfigs.asset", "Assets/ScriptableObjects/Configs/CardConfigs.asset", typeof(Configs.CardConfigs), new [] {""}),
			new AddressableConfig(4, "Configs/LevelTreeConfigs.asset", "Assets/ScriptableObjects/Configs/LevelTreeConfigs.asset", typeof(Configs.LevelTreeConfigs), new [] {""}),
			new AddressableConfig(5, "Configs/UiConfigs.asset", "Assets/ScriptableObjects/Configs/UiConfigs.asset", typeof(GameLovers.UiService.UiConfigs), new [] {""}),
			new AddressableConfig(6, "Prefabs/Building.prefab", "Assets/Art/Prefabs/Building.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(7, "Prefabs/Ui/Cards Panel.prefab", "Assets/Art/Prefabs/Ui/Cards Panel.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(8, "Prefabs/Ui/Event Panel.prefab", "Assets/Art/Prefabs/Ui/Event Panel.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(9, "Prefabs/Ui/Hud.prefab", "Assets/Art/Prefabs/Ui/Hud.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(10, "Prefabs/Ui/Loading Screen.prefab", "Assets/Art/Prefabs/Ui/Loading Screen.prefab", typeof(UnityEngine.GameObject), new [] {""}),
			new AddressableConfig(11, "Prefabs/Vfx/Ui/Vfx.prefab", "Assets/Art/Prefabs/Vfx/Ui/Vfx.prefab", typeof(UnityEngine.GameObject), new [] {""})
		}.AsReadOnly();
	}
}
