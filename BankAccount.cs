namespace BankProject;

/// <summary>
/// Аккаунт в банке.
/// </summary>
public sealed class BankAccount
{
	private long cash;
	private DateOnly expireDate;
	private readonly Client owner;
	private bool isActive;

	/// <summary>
	/// Количество валюты на балансе (в копейках).
	/// </summary>
	public long Cash => cash;

	/// <summary>
	/// Дата устаревания аккаунта.
	/// </summary>
	public DateOnly ExpireDate => expireDate;

	/// <summary>
	/// Владелец аккаунта.
	/// </summary>
	public Client Owner => owner;

	/// <summary>
	/// Создаёт аккаунт в банке.
	/// </summary>
	/// <param name="owner">Владелец аккаунта.</param>
	/// <param name="expireDate">Дата окончания действия аккаунта.</param>
	/// <exception cref="OperationCanceledException">Пользователь слишком молод.</exception>
	public BankAccount(Client owner, DateOnly expireDate)
	{
		this.owner = owner;
		this.expireDate = expireDate;
		this.isActive = true;

		if (owner.Age < 16)
		{
			throw new OperationCanceledException();
		}
	}

	/// <summary>
	/// Проверяет действует ли аккаунт или нет.
	/// </summary>
	public bool IsExpired => DateTime.Now >= expireDate.ToDateTime(TimeOnly.MinValue);

	/// <summary>
	/// Статус аккаунта
	/// </summary>
	public AccountStatus Status
	{
		get
		{
			AccountStatus status = default;
			status |= IsExpired || isActive ? AccountStatus.Open : AccountStatus.Closed;
			if (Cash <= 0)
			{
				status |= AccountStatus.Bankrupt;
			}
			return status;
		}
	}

	/// <summary>
	/// Снять деньги со счёта.
	/// </summary>
	/// <param name="amount">Количество, сколько взять со счёта.</param>
	/// <returns>Возвращает <see langword="true"/> если удалось снять, иначе — <see langword="false"/></returns>
	public bool Withdraw(uint amount)
	{
		if (IsExpired)
			return false;

		if (amount > cash)
		{
			return false;
		}
		else
		{
			cash -= amount;
			return true;
		}
	}

	/// <summary>
	/// Пополнить счёт.
	/// </summary>
	/// <param name="amount">Количество на которое пополнить счёт.</param>
	/// <returns>Возвращает <see langword="true"/> если удалось пополнить, иначе — <see langword="false"/></returns>
	public bool Replenish(uint amount)
	{
		if (IsExpired)
			return false;

		cash += amount;
		return true;
	}

	/// <summary>
	/// Закрывает счёт.
	/// </summary>
	public void Close()
	{
		this.isActive = false;
	}
}