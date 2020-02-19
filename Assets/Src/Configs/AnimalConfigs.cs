using System;
using System.Collections.Generic;
using GameLovers.ConfigsContainer;
using Ids;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct AnimalConfig
	{
		public GameId Id;
		public GameId Tree;
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="AnimalConfigs"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "AnimalConfigs", menuName = "ScriptableObjects/Configs/AnimalConfigs")]
	public class AnimalConfigs : ScriptableObject, IConfigsContainer<AnimalConfig>
	{
		[SerializeField]
		private List<AnimalConfig> _configs = new List<AnimalConfig>();

		// ReSharper disable once ConvertToAutoProperty
		public List<AnimalConfig> Configs => _configs;
	}
}