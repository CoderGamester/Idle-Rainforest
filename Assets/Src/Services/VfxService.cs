using DG.Tweening;
using GameLovers.AddressableIdsScriptGenerator;
using Ids;
using Logic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Services
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IVfxService
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void PlayUiVfx(Vector3 endPosition);
	}
	
	/// <inheritdoc />
	public class VfxService : IVfxService
	{
		private readonly IGameDataProvider _dataProvider;
		private readonly IGameServices _services;
		
		public VfxService(IGameServices services, IGameDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
			_services = services;
		}
		
		/// <inheritdoc />
		public async void PlayUiVfx(Vector3 endPosition)
		{
			var operation = Addressables.InstantiateAsync(AddressableId.Prefabs_Vfx_Ui_Vfx.GetConfig().Address, 
				Input.mousePosition, Quaternion.identity, _services.UiService.GetLayer(0).transform);

			await operation.Task;

			operation.Result.transform.DOMove(endPosition, 1).OnComplete(() => Addressables.ReleaseInstance(operation));
		}
	}
}