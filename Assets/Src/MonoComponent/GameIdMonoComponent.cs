using Commands;
using GameLovers.Services;
using Ids;
using Logic;
using Services;
using UnityEngine;

namespace MonoComponent
{
	/// <summary>
	/// This component identifies the attached object by it's <seealso cref="GameId"/>.
	/// Use this component only on singleton GameIds in order to avoid incorrect mapping.
	/// </summary>
	public class GameIdMonoComponent : MonoBehaviour
	{
		[SerializeField] private EntityMonoComponent _entityMonoComponent;
		[SerializeField] private GameId _gameId;

		private void Awake()
		{
			MainInstaller.Resolve<IGameServices>().CommandService.ExecuteCommand(new AddWorldObjectReference
			{
				GameId = _gameId,
				WorldObjectReference = gameObject
			});

			_entityMonoComponent.UniqueId = MainInstaller.Resolve<IGameDataProvider>().GameIdDataProvider.GetData(_gameId).Id;
		}
	}
}