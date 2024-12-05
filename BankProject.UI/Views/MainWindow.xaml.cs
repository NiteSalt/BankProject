using BankProject.UI.ViewModels;
using ModernWpf.Controls.Primitives;
using System;
using System.Media;
using System.Windows;

namespace BankProject.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		if (base.DataContext is not MainWindowViewModel)
		{
			throw new Exception();
		}

		Client defaultClient = new (2100231, "Иван", "Иванов", "Иванович", new DateOnly(1995, 10, 27));
		BankAccount defaultAccount = new(defaultClient, new DateOnly(2030, 9, 11));
		DataContext.RegisterNewClient(defaultClient);
	}

	private new MainWindowViewModel DataContext => (base.DataContext as MainWindowViewModel)!;

	private void OnAccountCreationButtonClick(object sender, RoutedEventArgs e)
	{
		Client? client = DataContext.CreateAccountClient;
		DateOnly expireDate = DataContext.CreateAccountDate;

		if (client is null)
		{
			return;
		}

		BankAccount newAccount = null;
		try
		{
			newAccount = new(client, expireDate);
		}
		catch (OperationCanceledException)
		{
			SystemSounds.Hand.Play();
		}

		if (newAccount != null)
		{
			DataContext.RegisterNewAccount(client, newAccount);
		}
	}

	private void OnRegistrationButtonClick(object sender, RoutedEventArgs e)
	{
		string name = DataContext.RegistrationName;
		string surname = DataContext.RegistrationSurname;
		string patronym = DataContext.RegistrationPatronym;
		ulong passport = DataContext.RegistrationPassport;
		DateOnly birth = DataContext.RegistrationDate;
	
		if (string.IsNullOrWhiteSpace(name))
		{
			FlyoutBase.SetAttachedFlyout(registrationNameBox, (FlyoutBase)Resources["FieldNotSetFlyout"]);
			FlyoutBase.ShowAttachedFlyout(registrationNameBox);
			return;
		}
		if (string.IsNullOrWhiteSpace(surname))
		{
			FlyoutBase.SetAttachedFlyout(registrationSurnameBox, (FlyoutBase)Resources["FieldNotSetFlyout"]);
			FlyoutBase.ShowAttachedFlyout(registrationSurnameBox);
			return;
		}
		if (string.IsNullOrWhiteSpace(patronym))
		{
			FlyoutBase.SetAttachedFlyout(registrationPatronymBox, (FlyoutBase)Resources["FieldNotSetFlyout"]);
			FlyoutBase.ShowAttachedFlyout(registrationPatronymBox);
			return;
		}
		if (passport == 0)
		{
			FlyoutBase.SetAttachedFlyout(registrationPassportBox, (FlyoutBase)Resources["PassportInvalidFlyout"]);
			FlyoutBase.ShowAttachedFlyout(registrationPassportBox);
			return;
		}
		if (birth.CompareTo( DateOnly.FromDateTime(DateTime.Now) ) > 0)
		{
			FlyoutBase.SetAttachedFlyout(registrationDatePicker, (FlyoutBase)Resources["DateIsInFutureFlyout"]);
			FlyoutBase.ShowAttachedFlyout(registrationDatePicker);
			return;
		}

		Client newClient = new(passport, name, surname, patronym, birth);
		DataContext.RegisterNewClient(newClient);
	}

	private void OnReplenishButtonClick(object sender, RoutedEventArgs e)
	{
		BankAccount? account = DataContext.InfoSelectedAccount;
		uint amount = DataContext.ControlCashAmount;

		if (account?.Replenish(amount) == false)
		{
			SystemSounds.Hand.Play();
		}
		else
		{
			DataContext.OnUpdateAccount();
		}
	}

	private void OnWithdrawButtonClick(object sender, RoutedEventArgs e)
	{
		BankAccount? account = DataContext.InfoSelectedAccount;
		uint amount = DataContext.ControlCashAmount;

		if (account?.Withdraw(amount) == false)
		{
			SystemSounds.Hand.Play();
		}
		else
		{
			DataContext.OnUpdateAccount();
		}
	}

	private void OnTransferButtonClick(object sender, RoutedEventArgs e)
	{
		BankAccount? from = DataContext.TransactionFromAccount;
		BankAccount? to = DataContext.TransactionToAccount;
		uint amount = DataContext.TransactionAmount;
	
		if (from == to || from == null || to == null)
		{
			SystemSounds.Hand.Play();
			return;
		}

		if ((from?.IsExpired ?? false) || (to?.IsExpired ?? false))
		{
			SystemSounds.Hand.Play();
			return;
		}

		if (from!.Withdraw(amount))
		{
			if (!to!.Replenish(amount))
			{
				from.Replenish(amount); // Возврат денег, если не удалось перевести
				return;
			}

			DataContext.OnUpdateAccount();
		}
	}
}