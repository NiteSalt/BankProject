using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace BankProject.UI.Converters;

public sealed class NullableDateTimeToDateOnlyConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (targetType == typeof(DateTime?) && value is DateOnly date)
		{
			return new DateTime?(date.ToDateTime(TimeOnly.MinValue));
		}
		else
		{
			throw new InvalidCastException();
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (targetType == typeof(DateOnly) && value is DateTime?)
		{
			DateTime? time = value as DateTime?;

			return DateOnly.FromDateTime(time!.Value);
		}
		else
		{
			throw new InvalidCastException();
		}
	}
}