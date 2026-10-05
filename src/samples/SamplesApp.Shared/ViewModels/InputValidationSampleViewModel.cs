using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Uno.Themes.Samples.ViewModels;

/// <summary>
/// Form for the input validation sample, validated through <see cref="INotifyDataErrorInfo"/> only.
/// Every property is validated eagerly, so the error visuals show as soon as the page loads.
/// </summary>
[XamlBindable]
public sealed class InputValidationSampleViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
	private static readonly string[] KnownCities =
	[
		"Montreal", "Toronto", "Vancouver", "Ottawa", "Quebec City", "Calgary", "Halifax",
	];

	private static readonly string[] ValidatedProperties =
	[
		nameof(UserName), nameof(Codename), nameof(Password), nameof(Pin), nameof(City), nameof(Country),
	];

	/// <summary>Only failing properties have an entry.</summary>
	private readonly Dictionary<string, string[]> _errors = new(StringComparer.Ordinal);

	public InputValidationSampleViewModel()
	{
		foreach (var propertyName in ValidatedProperties)
		{
			Validate(propertyName);
		}
	}

	/// <summary>Required, 5 to 10 characters, letters/digits/dash/underscore only.</summary>
	public string UserName
	{
		get;
		set => Set(ref field, value);
	} = string.Empty;

	/// <summary>Required, with an even number of characters: one more keystroke flips it.</summary>
	public string Codename
	{
		get;
		set => Set(ref field, value);
	} = string.Empty;

	/// <summary>Required, at least 9 characters, with a digit.</summary>
	public string Password
	{
		get;
		set => Set(ref field, value);
	} = string.Empty;

	/// <summary>Exactly 4 digits.</summary>
	public string Pin
	{
		get;
		set => Set(ref field, value);
	} = string.Empty;

	/// <summary>Required, and one of <see cref="CitySuggestions"/>.</summary>
	public string City
	{
		get;
		set
		{
			if (Set(ref field, value))
			{
				OnPropertyChanged(nameof(CitySuggestions));
			}
		}
	} = string.Empty;

	/// <summary>The known cities matching what has been typed so far.</summary>
	public IReadOnlyList<string> CitySuggestions => string.IsNullOrWhiteSpace(City)
		? KnownCities
		: KnownCities.Where(c => c.Contains(City, StringComparison.OrdinalIgnoreCase)).ToArray();

	/// <summary>Required, and French speaking.</summary>
	public object? Country
	{
		get;
		set => Set(ref field, value);
	}

	public IReadOnlyList<string> Countries { get; } = ["Canada", "France", "Japan", "Portugal"];

	private IEnumerable<string> GetRuleViolations(string propertyName)
	{
		switch (propertyName)
		{
			case nameof(UserName):
				if (string.IsNullOrWhiteSpace(UserName))
				{
					yield return "Please enter a user name.";
					yield break;
				}
				if (UserName.Length is < 5 or > 10)
				{
					yield return $"User name must be 5 to 10 characters long ({UserName.Length} given).";
				}
				if (!UserName.All(c => char.IsLetterOrDigit(c) || c is '-' or '_'))
				{
					yield return "User name may only contain letters, digits, '-' and '_'.";
				}
				break;

			case nameof(Codename):
				if (string.IsNullOrEmpty(Codename))
				{
					yield return "Please enter a codename.";
				}
				else if (Codename.Length % 2 != 0)
				{
					yield return $"Codename must have an even number of characters ({Codename.Length} is odd).";
				}
				break;

			case nameof(Password):
				if (string.IsNullOrEmpty(Password))
				{
					yield return "Please enter a password.";
					yield break;
				}
				if (Password.Length < 9)
				{
					yield return "Password must be at least 9 characters long.";
				}
				if (!Password.Any(char.IsDigit))
				{
					yield return "Password must contain at least one digit.";
				}
				break;

			case nameof(Pin):
				if (Pin.Length != 4 || !Pin.All(char.IsDigit))
				{
					yield return "PIN must be exactly 4 digits.";
				}
				break;

			case nameof(City):
				if (string.IsNullOrWhiteSpace(City))
				{
					yield return "Please enter a city.";
				}
				else if (!KnownCities.Contains(City, StringComparer.OrdinalIgnoreCase))
				{
					yield return $"'{City}' is not one of the suggested cities.";
				}
				break;

			case nameof(Country):
				if (Country is null)
				{
					yield return "Please choose a country.";
				}
				else if (Country is not ("Canada" or "France"))
				{
					yield return "Please choose a French speaking country.";
				}
				break;
		}
	}

	/// <summary>Re-runs one property's rules, and raises <see cref="ErrorsChanged"/> when the outcome changed.</summary>
	private void Validate(string propertyName)
	{
		var violations = GetRuleViolations(propertyName).ToArray();

		if (violations.Length == 0)
		{
			if (!_errors.Remove(propertyName))
			{
				return;
			}
		}
		else if (_errors.TryGetValue(propertyName, out var previous) && previous.SequenceEqual(violations))
		{
			return;
		}
		else
		{
			_errors[propertyName] = violations;
		}

		ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		OnPropertyChanged(nameof(HasErrors));
	}

	private bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
	{
		if (EqualityComparer<T>.Default.Equals(field, value))
		{
			return false;
		}

		field = value;
		OnPropertyChanged(propertyName);
		Validate(propertyName);
		return true;
	}

	#region INotifyDataErrorInfo

	public bool HasErrors => _errors.Count != 0;

	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	/// <summary>Errors of one property, or of all of them when the name is null or empty.</summary>
	public IEnumerable GetErrors(string? propertyName)
		=> string.IsNullOrEmpty(propertyName)
			? _errors.Values.SelectMany(errors => errors).ToArray()
			: _errors.TryGetValue(propertyName, out var errors) ? errors : Array.Empty<string>();

	#endregion

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
