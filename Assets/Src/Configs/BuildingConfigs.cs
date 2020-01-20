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
	public struct BuildingConfig
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
	/// Scriptable Object tool to import the <seealso cref="BuildingConfig"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "BuildingConfigs", menuName = "ScriptableObjects/Configs/BuildingConfigs")]
	public class BuildingConfigs : ScriptableObject, IConfigsContainer<BuildingConfig>
	{
		[SerializeField]
		private List<BuildingConfig> _configs = new List<BuildingConfig>();

		// ReSharper disable once ConvertToAutoProperty
		public List<BuildingConfig> Configs => _configs;
	}
}