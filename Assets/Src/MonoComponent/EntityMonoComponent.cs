using Ids;
using Services;
using UnityEngine;

namespace MonoComponent
{
	public class EntityMonoComponent : MonoBehaviour
	{
		[SerializeField] private GameId _gameId;

		private IGameServices _gameServices;
		
		public UniqueId UniqueId { get; set; }
	}
}