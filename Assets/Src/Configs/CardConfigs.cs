using System;
using System.Collections.Generic;
using GameLovers.ConfigsContainer;
using Ids;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct CardConfig : IConfig
	{
		public GameId Id;
		public GameId Building;
		public List<int> UpgradeCost;
		public List<int> UpgradeCardsRequired;
		public List<int> LevelBonus;
		
		public int ConfigId => (int) Id;
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="CardConfig"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "CardConfigs", menuName = "ScriptableObjects/Configs/CardConfigs")]
	public class CardConfigs : ScriptableObject, IConfigsContainer<CardConfig>
	{
		[SerializeField]
		private List<CardConfig> _configs = new List<CardConfig>();

		// ReSharper disable once ConvertToAutoProperty
		public List<CardConfig> Configs => _configs;
	}
}