using BankProject;
using System.Runtime.CompilerServices;

public static class Program
{
	private static void Main()
	{
		Client client = new(2021811223, "Антон", "Гусько", "Васильевич", new DateOnly(1992, 7, 24));

		BankAccount account = new(client, new DateOnly(2027, 12, 12));

		Console.WriteLine($"Account status: {account.Status}");
		Test(account.IsExpired);
		Test(account.Withdraw(20));
		Test(account.Replenish(120));
		Test(account.Withdraw(20));
		Console.WriteLine($"Account status: {account.Status}");
		Test(account.Withdraw(100));
		Console.WriteLine($"Account status: {account.Status}");
		account.Close();
		Console.WriteLine($"Account status: {account.Status}");
	}

	private static void Test(bool result, [CallerArgumentExpression(nameof(result))] string? expression = null)
	{
		Console.WriteLine($"{expression} = {result}");
	}
}