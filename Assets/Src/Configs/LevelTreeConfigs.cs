using System;
using System.Collections.Generic;
using Data;
using GameLovers;
using GameLovers.ConfigsContainer;
using Ids;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct LevelTreeConfig
	{
		public GameId Tree;
		public int BuildCost;
		public int ProductionAmountBase;
		public int ProductionAmountIncrease;
		public float ProductionTimeBase;
		public int UpgradeCostBase;
		public int UpgradeCostIncrease;
		public int AutomationCurrencyRequired;
		public IntData AutomationCardRequired;
		public List<IntData> UpgradeRewards;
		public List<IntPairData> UpgradeBrackets;
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="LevelTreeConfig"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "LevelTreeConfigs", menuName = "ScriptableObjects/Configs/LevelTreeConfigs")]
	public class LevelTreeConfigs : ScriptableObject, IConfigsContainer<LevelTreeConfig>
	{
		[SerializeField]
		private List<LevelTreeConfig> _configs = new List<LevelTreeConfig>();

		// ReSharper disable once ConvertToAutoProperty
		public List<LevelTreeConfig> Configs => _configs;
	}
}