using System;
using System.Collections.Generic;
using GameLovers.ConfigsContainer;
using Ids;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct CardConfig
	{
		public GameId Id;
		public List<int> UpgradeCost;
		public List<int> UpgradeCardsRequired;
		public List<int> LevelBonus;
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