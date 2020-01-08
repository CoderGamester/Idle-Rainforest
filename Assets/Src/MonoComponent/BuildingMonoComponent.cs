using GameLovers.Services;
using Logic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MonoComponent
{
	public class BuildingMonoComponent : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private EntityMonoComponent _entityMonoComponent;
		[SerializeField] private TextMeshPro _buildingName;

		private IGameDataProvider _gameDataProvider;

		private void Awake()
		{
			_gameDataProvider = MainInstaller.Resolve<IGameDataProvider>();
		}

		private void Start()
		{
			_buildingName.text = _gameDataProvider.EntityDataProvider.GetGameId(_entityMonoComponent.Entity).ToString();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log($"{_buildingName.text} clicked");
		}
	}
}