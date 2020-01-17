using System;
using Data;
using Events;
using GameLovers.Services;
using Ids;

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
		private readonly CurrencyData _currencyData;

		/// <inheritdoc />
		public float MainCurrencyAmount => _currencyData.MainCurrency;
		/// <inheritdoc />
		public int SoftCurrencyAmount => _currencyData.SoftCurrency;
		/// <inheritdoc />
		public int HardCurrencyAmount => _currencyData.HardCurrency;
		
		private CurrencyLogic() {}

		public CurrencyLogic(IGameInternalLogic gameLogic, CurrencyData currencyData)
		{
			_gameLogic = gameLogic;
			_currencyData = currencyData;
		}

		/// <inheritdoc />
		public void AddMainCurrency(int amount)
		{
			var oldAmount = _currencyData.MainCurrency;
			
			_currencyData.MainCurrency += amount;

			PublishCurrencyEvent(GameId.MainCurrency, oldAmount, _currencyData.MainCurrency);
		}

		/// <inheritdoc />
		public void DeductMainCurrency(int amount)
		{
			if (_currencyData.MainCurrency - amount < 0)
			{
				throw new InvalidOperationException($"The player needs {amount.ToString()} main currency and only has " +
				                                    $"{_currencyData.MainCurrency.ToString()}");
			}

			var oldAmount = _currencyData.MainCurrency;

			_currencyData.MainCurrency -= amount;

			PublishCurrencyEvent(GameId.MainCurrency, oldAmount, _currencyData.MainCurrency);
		}

		/// <inheritdoc />
		public void AddSoftCurrency(int amount)
		{
			var oldAmount = _currencyData.SoftCurrency;
			
			_currencyData.SoftCurrency += amount;

			PublishCurrencyEvent(GameId.SoftCurrency, oldAmount, _currencyData.SoftCurrency);
		}

		/// <inheritdoc />
		public void DeductSoftCurrency(int amount)
		{
			if (_currencyData.SoftCurrency - amount < 0)
			{
				throw new InvalidOperationException($"The player needs {amount.ToString()} soft currency and only has " +
				                                    $"{_currencyData.SoftCurrency.ToString()}");
			}

			var oldAmount = _currencyData.SoftCurrency;
			
			_currencyData.SoftCurrency -= amount;

			PublishCurrencyEvent(GameId.SoftCurrency, oldAmount, _currencyData.SoftCurrency);
		}

		/// <inheritdoc />
		public void AddHardCurrency(int amount)
		{
			var oldAmount = _currencyData.HardCurrency;
			
			_currencyData.HardCurrency += amount;

			PublishCurrencyEvent(GameId.HardCurrency, oldAmount, _currencyData.HardCurrency);
		}

		/// <inheritdoc />
		public void DeductHardCurrency(int amount)
		{
			if (_currencyData.HardCurrency - amount < 0)
			{
				throw new InvalidOperationException($"The player needs {amount.ToString()} hard currency and only has " +
				                                    $"{_currencyData.HardCurrency.ToString()}");
			}

			var oldAmount = _currencyData.HardCurrency;
			
			_currencyData.HardCurrency -= amount;

			PublishCurrencyEvent(GameId.HardCurrency, oldAmount, _currencyData.HardCurrency);
		}

		private void PublishCurrencyEvent(GameId currencyType, int oldAmount, int newAmount)
		{
			switch (currencyType)
			{
				case GameId.MainCurrency:
					_gameLogic.MessageBrokerService.Publish(new MainCurrencyValueChangedEvent
					{
						OldValue = oldAmount,
						NewValue = newAmount
					});
					break;
				case GameId.SoftCurrency:
					_gameLogic.MessageBrokerService.Publish(new SoftCurrencyValueChangedEvent
					{
						OldValue = oldAmount,
						NewValue = newAmount
					});
					break;
				case GameId.HardCurrency:
					_gameLogic.MessageBrokerService.Publish(new HardCurrencyValueChangedEvent
					{
						OldValue = oldAmount,
						NewValue = newAmount
					});
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, "Wrong currency type");
			}
		}
	}
}