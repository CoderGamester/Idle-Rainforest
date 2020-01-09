using System;

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
		int MainCurrencyAmount { get; }
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
		void AddMainCurrency(int amount);
		/// <summary>
		/// TODO:
		/// </summary>
		void DeductMainCurrency(int amount);
		
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
		public int MainCurrencyAmount => _gameLogic.DataProviderLogic.PlayerData.MainCurrency;
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
		public void AddMainCurrency(int amount)
		{
			_gameLogic.DataProviderLogic.PlayerData.MainCurrency += amount;
		}

		/// <inheritdoc />
		public void DeductMainCurrency(int amount)
		{
			if (_gameLogic.DataProviderLogic.PlayerData.MainCurrency - amount < 0)
			{
				throw new InvalidOperationException($"The player doesn't have enough main currency to deduct. " +
				                                    $"It needs {amount.ToString()} and only has " +
				                                    $"{_gameLogic.DataProviderLogic.PlayerData.MainCurrency.ToString()}");
			}

			_gameLogic.DataProviderLogic.PlayerData.MainCurrency -= amount;
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
				throw new InvalidOperationException($"The player doesn't have enough soft currency to deduct. " +
				                                    $"It needs {amount.ToString()} and only has " +
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
				throw new InvalidOperationException($"The player doesn't have enough hard currency to deduct. " +
				                                    $"It needs {amount.ToString()} and only has " +
				                                    $"{_gameLogic.DataProviderLogic.PlayerData.HardCurrency.ToString()}");
			}

			_gameLogic.DataProviderLogic.PlayerData.HardCurrency -= amount;
		}
	}
}