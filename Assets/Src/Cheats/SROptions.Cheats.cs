using System;
using System.ComponentModel;
using GameLovers.Services;
using Logic;
using Services;
using UnityEngine;

public partial class SROptions
{
	[Category("Data")]
	public void ResetAllData()
	{
		PlayerPrefs.DeleteAll();
	}

	[Category("Currency")]
	public void Add100MC()
	{
		var currencyLogic = MainInstaller.Resolve<IGameDataProvider>().CurrencyDataProvider as CurrencyLogic;
		
		currencyLogic.AddMainCurrency(100);
	}

	[Category("Currency")]
	public void Add10000MC()
	{
		var currencyLogic = MainInstaller.Resolve<IGameDataProvider>().CurrencyDataProvider as CurrencyLogic;
		
		currencyLogic.AddMainCurrency(10000);
	}

	[Category("Currency")]
	public void Add100SC()
	{
		var currencyLogic = MainInstaller.Resolve<IGameDataProvider>().CurrencyDataProvider as CurrencyLogic;
		
		currencyLogic.AddSoftCurrency(100);
	}

	[Category("Currency")]
	public void Add1000SC()
	{
		var currencyLogic = MainInstaller.Resolve<IGameDataProvider>().CurrencyDataProvider as CurrencyLogic;
		
		currencyLogic.AddSoftCurrency(1000);
	}

	[Category("Currency")]
	public void Add100HC()
	{
		var currencyLogic = MainInstaller.Resolve<IGameDataProvider>().CurrencyDataProvider as CurrencyLogic;
		
		currencyLogic.AddHardCurrency(100);
	}
		
	[Category("Time")]
	public void Add1Day()
	{
		var timeManipulator = MainInstaller.Resolve<IGameServices>().TimeService as ITimeManipulator;
		var timeNow = timeManipulator.DateTimeUtcNow;
			
		timeManipulator.AddTime((float) (timeNow.AddDays(1) - timeNow).TotalSeconds);
	}
		
	[Category("Time")]
	public void Add1Hour()
	{
		var timeManipulator = MainInstaller.Resolve<IGameServices>().TimeService as ITimeManipulator;
		var timeNow = timeManipulator.DateTimeUtcNow;
			
		timeManipulator.AddTime((float) (timeNow.AddHours(1) - timeNow).TotalSeconds);
	}
		
	[Category("Time")]
	public void Add1Minute()
	{
		var timeManipulator = MainInstaller.Resolve<IGameServices>().TimeService as ITimeManipulator;
		var timeNow = timeManipulator.DateTimeUtcNow;
			
		timeManipulator.AddTime((float) (timeNow.AddHours(1) - timeNow).TotalSeconds);
	}
}