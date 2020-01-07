using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MonoComponent
{
	public class BuildingMonoComponent : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private TextMeshPro _buildingName;

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log($"{_buildingName.text} clicked");
		}
	}
}