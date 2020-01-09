using System;
using Data;
using Ids;
using Infos;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IResourceDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		ResourceInfo GetInfo(GameId resource);
	}

	/// <inheritdoc />
	public interface IResourceLogic : IResourceDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void AddResource(GameId resource, float amount);
		
		/// <summary>
		/// TODO:
		/// </summary>
		void DeductResource(GameId resource, float amount);
	}
	
	/// <inheritdoc />
	public class ResourceLogic : IResourceLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		
		private ResourceLogic() {}

		public ResourceLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}

		/// <inheritdoc />
		public ResourceInfo GetInfo(GameId resource)
		{
			if (!resource.IsInGroup(GameIdGroup.UserResource))
			{
				throw new InvalidOperationException($"The given resource {resource} is not of {nameof(GameIdGroup.UserResource)}");
			}

			return new ResourceInfo
			{
				GameId = resource, 
				Amount = GetData(resource).Amount
				
			};
		}

		/// <inheritdoc />
		public void AddResource(GameId resource, float amount)
		{
			var index = _gameLogic.DataProviderLogic.PlayerData.Resources.FindIndex(resourceData => resourceData.GameId == resource);
			
			if (index < 0)
			{
				_gameLogic.DataProviderLogic.PlayerData.Resources.Add(new ResourceData {GameId = resource, Amount = amount});
			}
			else
			{
				var data = _gameLogic.DataProviderLogic.PlayerData.Resources[index];

				data.Amount += amount;

				_gameLogic.DataProviderLogic.PlayerData.Resources[index] = data;
			}
		}

		/// <inheritdoc />
		public void DeductResource(GameId resource, float amount)
		{
			var index = _gameLogic.DataProviderLogic.PlayerData.Resources.FindIndex(resourceData => resourceData.GameId == resource);
			
			if (index < 0 || _gameLogic.DataProviderLogic.PlayerData.Resources[index].Amount - amount < 0)
			{
				throw new InvalidOperationException($"The player doesn't have enough {resource} to deduct. " +
				                                    $"It needs {amount.ToString()} and only has " +
				                                    $"{_gameLogic.DataProviderLogic.PlayerData.Resources[index].Amount.ToString()}");
			}
			else
			{
				var data = _gameLogic.DataProviderLogic.PlayerData.Resources[index];

				data.Amount -= amount;

				_gameLogic.DataProviderLogic.PlayerData.Resources[index] = data;
			}
		}

		private ResourceData GetData(GameId resource)
		{
			var data = new ResourceData {GameId = resource, Amount = 0};
			
			var index = _gameLogic.DataProviderLogic.PlayerData.Resources.FindIndex(resourceData => resourceData.GameId == resource);
			if (index >= 0)
			{
				data.Amount = _gameLogic.DataProviderLogic.PlayerData.Resources[index].Amount;
			}

			return data;
		}
	}
}