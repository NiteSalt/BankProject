using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Windows.Data;

namespace BankProject.UI.ViewModels;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
	private readonly HashSet<Client> clients;

	private string registrationName, registrationSurname, registrationPatronym;
	private ulong registrationPassport;
	private DateOnly registrationDate;

	private DateOnly createAccountDate;
	private Client? createAccountClient;

	private Client? infoSelectedClient;
	private BankAccount? infoSelectedAccount;

	private uint controlCashAmount;

	private Client? transactionFromClient;
	private BankAccount? transactionFromAccount;
	private Client? transactionToClient;
	private BankAccount? transactionToAccount;

	private uint transactionAmount;

	public event PropertyChangedEventHandler? PropertyChanged;

	public MainWindowViewModel()
	{
		clients = new();
		registrationName = registrationSurname = registrationPatronym = string.Empty;
		CreateAccountDate = RegistrationDate = DateOnly.FromDateTime(DateTime.Now);
	}

	public CollectionView Clients => new(clients);

	public string RegistrationName
	{
		get => registrationName;
		set
		{
			this.registrationName = value;
			OnPropertyChanged();
		}
	}

	public string RegistrationSurname
	{
		get => registrationSurname;
		set
		{
			this.registrationSurname = value;
			OnPropertyChanged();
		}
	}

	public string RegistrationPatronym
	{
		get => registrationPatronym;
		set
		{
			this.registrationPatronym = value;
			OnPropertyChanged();
		}
	}

	public ulong RegistrationPassport
	{
		get => registrationPassport;
		set
		{
			this.registrationPassport = value;
			OnPropertyChanged();
		}
	}

	public DateOnly RegistrationDate
	{
		get => registrationDate;
		set
		{
			this.registrationDate = value;
			OnPropertyChanged();
		}
	}

	public DateOnly CreateAccountDate
	{
		get => createAccountDate;
		set
		{
			this.createAccountDate = value;
			OnPropertyChanged();
		}
	}

	public Client? CreateAccountClient
	{
		get => createAccountClient;
		set
		{
			this.createAccountClient = value;
			OnPropertyChanged();
		}
	}

	public Client? InfoSelectedClient
	{
		get => infoSelectedClient;
		set
		{
			this.infoSelectedClient = value;
			OnPropertyChanged();
			InfoSelectedAccount = null;
			OnPropertyChanged(nameof(InfoClientAccounts));
		}
	}

	public ObservableCollection<BankAccount> InfoClientAccounts
	{
		get
		{
			if (infoSelectedClient is null)
			{
				return new ObservableCollection<BankAccount>(Array.Empty<BankAccount>());
			}

			return new ObservableCollection<BankAccount>(infoSelectedClient.Accounts);
		}
	}

	public BankAccount? InfoSelectedAccount
	{
		get => infoSelectedAccount;
		set
		{
			this.infoSelectedAccount = value;
			OnPropertyChanged();
		}
	}

	public uint ControlCashAmount
	{
		get => controlCashAmount;
		set
		{
			this.controlCashAmount = value;
			OnPropertyChanged();
		}
	}

	public Client? TransactionFromClient
	{
		get => transactionFromClient;
		set
		{
			this.transactionFromClient = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(TransactionFromAccounts));
		}
	}

	public Client? TransactionToClient
	{
		get => transactionToClient;
		set
		{
			this.transactionToClient = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(TransactionToAccounts));
		}
	}

	public ObservableCollection<BankAccount> TransactionToAccounts
	{
		get
		{
			return new(TransactionToClient?.Accounts ?? Array.Empty<BankAccount>());
		}
	}

	public ObservableCollection<BankAccount> TransactionFromAccounts
	{
		get
		{
			return new(TransactionFromClient?.Accounts ?? Array.Empty<BankAccount>());
		}
	}

	public BankAccount? TransactionFromAccount
	{
		get => transactionFromAccount;
		set
		{
			this.transactionFromAccount = value;
			OnPropertyChanged();
		}
	}


	public BankAccount? TransactionToAccount
	{
		get => transactionToAccount;
		set
		{
			this.transactionToAccount = value;
			OnPropertyChanged();
		}
	}

	public uint TransactionAmount
	{
		get => controlCashAmount;
		set
		{
			this.controlCashAmount = value;
			OnPropertyChanged();
		}
	}

	public void RegisterNewClient(Client client)
	{
		clients.Add(client);
		OnPropertyChanged(nameof(Clients));
	}

	public void RegisterNewAccount(Client client, BankAccount account)
	{
		OnPropertyChanged(nameof(this.InfoClientAccounts));
		OnPropertyChanged(nameof(this.InfoSelectedAccount));
		OnPropertyChanged(nameof(this.TransactionFromAccounts));
		OnPropertyChanged(nameof(this.TransactionToAccounts));
	}

	private void OnPropertyChanged([CallerMemberName] string? caller = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
	}

	public void OnUpdateAccount()
	{
		OnPropertyChanged(nameof(this.InfoSelectedAccount));
		OnPropertyChanged(nameof(this.InfoSelectedClient));
		OnPropertyChanged(nameof(this.InfoClientAccounts));

		OnPropertyChanged(nameof(this.TransactionFromAccount));
		OnPropertyChanged(nameof(this.TransactionToAccount));
		OnPropertyChanged(nameof(this.TransactionFromAccounts));
		OnPropertyChanged(nameof(this.TransactionToAccounts));
	}
}