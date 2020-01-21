using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Data;
using GameLovers.ConfigsContainer;
using GameLovers.GoogleSheetImporter;
using Ids;
using Newtonsoft.Json;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct LevelBuildingConfig
	{
		public GameId Building;
		public int ProductionAmountBase;
		public int ProductionAmountIncrease;
		public float ProductionTimeBase;
		public int UpgradeCostBase;
		public int UpgradeCostIncrease;
		public int AutomationCurrencyRequired;
		public int AutomationCardLevelRequired;

		[SerializeField] private List<IntData> _upgradeRewards;
		[SerializeField] private List<IntPairData> _upgradeBrackets;

		public ReadOnlyCollection<IntData> UpgradeRewards
		{
			get => _upgradeRewards.AsReadOnly();
			set => _upgradeRewards = new List<IntData>(value);
		}

		public ReadOnlyCollection<IntPairData> UpgradeBrackets
		{
			get => _upgradeBrackets.AsReadOnly();
			set => _upgradeBrackets = new List<IntPairData>(value);
		}
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="LevelBuildingConfig"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "LevelBuildingConfigs", menuName = "ScriptableObjects/Configs/LevelBuildingConfigs")]
	public class LevelBuildingConfigs : ScriptableObject, IConfigsContainer<LevelBuildingConfig>
	{
		[SerializeField]
		private List<LevelBuildingConfig> _configs = new List<LevelBuildingConfig>();

		// ReSharper disable once ConvertToAutoProperty
		public List<LevelBuildingConfig> Configs => _configs;
	}
}