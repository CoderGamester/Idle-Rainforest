using System;
using Events;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface ICurrencyDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		float MainCurrencyAmount { get; }
		/// <summary>
		/// TODO:
		/// </summary>
		int SoftCurrencyAmount { get; }
		/// <summary>
		/// TODO:
		/// </summary>
		int HardCurrencyAmount { get; }
	}

	/// <inheritdoc />
	public interface ICurrencyLogic : ICurrencyDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void AddMainCurrency(float amount);
		/// <summary>
		/// TODO:
		/// </summary>
		void DeductMainCurrency(float amount);
		
		/// <summary>
		/// TODO:
		/// </summary>
		void AddSoftCurrency(int amount);
		/// <summary>
		/// TODO:
		/// </summary>
		void DeductSoftCurrency(int amount);
		/// <summary>
		/// TODO:
		/// </summary>
		void AddHardCurrency(int amount);
		/// <summary>
		/// TODO:
		/// </summary>
		void DeductHardCurrency(int amount);
	}
	
	/// <inheritdoc />
	public class CurrencyLogic : ICurrencyLogic
	{
		private readonly IGameInternalLogic _gameLogic;

		/// <inheritdoc />
		public float MainCurrencyAmount => _gameLogic.DataProviderLogic.PlayerData.MainCurrency;
		/// <inheritdoc />
		public int SoftCurrencyAmount => _gameLogic.DataProviderLogic.PlayerData.SoftCurrency;
		/// <inheritdoc />
		public int HardCurrencyAmount => _gameLogic.DataProviderLogic.PlayerData.HardCurrency;
		
		private CurrencyLogic() {}

		public CurrencyLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}

		/// <inheritdoc />
		public void AddMainCurrency(float amount)
		{
			var seeds = _gameLogic.DataProviderLogic.PlayerData.MainCurrency;
			
			_gameLogic.DataProviderLogic.PlayerData.MainCurrency += amount;
			
			_gameLogic.MessageBrokerService.Publish(new MainCurrencyValueChangedEvent
			{
				NewValue = _gameLogic.DataProviderLogic.PlayerData.MainCurrency,
				OldValue = seeds
			});
		}

		/// <inheritdoc />
		public void DeductMainCurrency(float amount)
		{
			if (_gameLogic.DataProviderLogic.PlayerData.MainCurrency - amount < 0)
			{
				throw new InvalidOperationException($"The player needs {amount.ToString()} main currency and only has " +
				                                    $"{_gameLogic.DataProviderLogic.PlayerData.MainCurrency.ToString()}");
			}

			var seeds = _gameLogic.DataProviderLogic.PlayerData.MainCurrency;

			_gameLogic.DataProviderLogic.PlayerData.MainCurrency -= amount;
			
			_gameLogic.MessageBrokerService.Publish(new MainCurrencyValueChangedEvent
			{
				NewValue = _gameLogic.DataProviderLogic.PlayerData.MainCurrency,
				OldValue = seeds
			});
		}

		/// <inheritdoc />
		public void AddSoftCurrency(int amount)
		{
			_gameLogic.DataProviderLogic.PlayerData.MainCurrency += amount;
		}

		/// <inheritdoc />
		public void DeductSoftCurrency(int amount)
		{
			if (_gameLogic.DataProviderLogic.PlayerData.SoftCurrency - amount < 0)
			{
				throw new InvalidOperationException($"The player needs {amount.ToString()} soft currency and only has " +
				                                    $"{_gameLogic.DataProviderLogic.PlayerData.SoftCurrency.ToString()}");
			}

			_gameLogic.DataProviderLogic.PlayerData.SoftCurrency -= amount;
		}

		/// <inheritdoc />
		public void AddHardCurrency(int amount)
		{
			_gameLogic.DataProviderLogic.PlayerData.HardCurrency += amount;
		}

		/// <inheritdoc />
		public void DeductHardCurrency(int amount)
		{
			if (_gameLogic.DataProviderLogic.PlayerData.HardCurrency - amount < 0)
			{
				throw new InvalidOperationException($"The player needs {amount.ToString()} hard currency and only has " +
				                                    $"{_gameLogic.DataProviderLogic.PlayerData.HardCurrency.ToString()}");
			}

			_gameLogic.DataProviderLogic.PlayerData.HardCurrency -= amount;
		}
	}
}