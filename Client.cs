namespace BankProject;

/// <summary>
/// Данные клиента.
/// </summary>
public sealed class Client
{
	private readonly ulong passportId;
	private string name, surname, patronym;
	private readonly DateOnly birth;

	/// <summary>
	/// Имя.
	/// </summary>
	public string Name => name;

	/// <summary>
	/// Фамилия.
	/// </summary>
	public string Surname => surname;

	/// <summary>
	/// Отчество.
	/// </summary>
	public string Patronym => patronym;

	/// <summary>
	/// Дата рождения.
	/// </summary>
	public DateOnly Birth => birth;

	/// <summary>
	/// Создать пользователя.
	/// </summary>
	/// <param name="passportId">Номер паспорта.</param>
	/// <param name="name">Имя человека</param>
	/// <param name="surname">Фамилия человека.</param>
	/// <param name="patronym">Отчество человека.</param>
	/// <param name="birth">Дата рождения человека.</param>
	public Client(ulong passportId, string name, string surname, string patronym, DateOnly birth)
	{
		this.passportId = passportId;
		this.name = name;
		this.surname = surname;
		this.patronym = patronym;
		this.birth = birth;
	}

	/// <summary>
	/// Возраст человека.
	/// </summary>
	public int Age => DateTime.Today.Year - birth.Year;
}