using System;
using System.Collections.Generic;
using GameLovers.ConfigsContainer;
using Ids;
using UnityEngine;

namespace GameConfigs
{
	[Serializable]
	public struct BuildingConfig : IConfig
	{
		public GameId Id;
		public GameId ProductionType;
		public int ProductionAmountBase;
		public float ProductionTimeBase;
		
		public int ConfigId => (int) Id;
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