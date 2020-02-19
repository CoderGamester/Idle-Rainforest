using System;
using System.Collections.Generic;
using GameLovers.ConfigsContainer;
using Ids;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct TreeConfig
	{
		public GameId Id;
		public int ProductionAmountBase;
		public int ProductionAmountIncrease;
		public float ProductionTimeBase;
		public int UpgradeCostBase;
		public int UpgradeCostIncrease;
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="TreeConfigs"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "TreeConfigs", menuName = "ScriptableObjects/Configs/TreeConfigs")]
	public class TreeConfigs : ScriptableObject, IConfigsContainer<TreeConfig>
	{
		[SerializeField]
		private List<TreeConfig> _configs = new List<TreeConfig>();

		// ReSharper disable once ConvertToAutoProperty
		public List<TreeConfig> Configs => _configs;
	}
}