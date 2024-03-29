using System;
using GameLovers;
using GameLovers.ConfigsContainer;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct GameConfig
	{
		public IntPairData UpgradeBracketReward;
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="GameConfig"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "GameConfigs", menuName = "ScriptableObjects/Configs/GameConfigs")]
	public class GameConfigs : ScriptableObject, ISingleConfigContainer<GameConfig>
	{
		[SerializeField]
		private GameConfig _config;

		public GameConfig Config
		{
			get => _config;
			set => _config = value;
		}
	}
}