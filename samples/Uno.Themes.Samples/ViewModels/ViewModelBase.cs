using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Uno.Themes.Samples.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	private readonly Dictionary<string, object> _propertyValueStore = new Dictionary<string, object>();

	protected T GetProperty<T>([CallerMemberName] string propertyName = null)
	{
		return _propertyValueStore.TryGetValue(propertyName, out var value) ? (T)value : default;
	}

	protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
	{
		if (!(_propertyValueStore.TryGetValue(propertyName, out var oldValue) && oldValue?.Equals(value) == true))
		{
			_propertyValueStore[propertyName] = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	[XamlBindable]
	public class Command : ICommand
	{
		private readonly Action<object> _action;
		private readonly Func<object, Task> _actionTask;
		private readonly Func<object, bool> _canExecute;
		private bool _manualCanExecute = true;

		public bool ManualCanExecute
		{
			get => _manualCanExecute;
			set
			{
				_manualCanExecute = value;
				CanExecuteChanged?.Invoke(this, null);
			}
		}

		private object _isExecuting = null;
		private static object _isExecutingNull = new object(); // this represent a null parameter when something is executing

		public Command(Action<object> action, Func<object, bool> canExecute = null)
		{
			_action = action ?? throw new ArgumentNullException(nameof(action));
			_canExecute = canExecute;
		}

		public Command(Func<object, Task> actionTask, Func<object, bool> canExecute = null)
		{
			_actionTask = actionTask ?? throw new ArgumentNullException(nameof(actionTask));
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			var canExecuteParameter = parameter ?? _isExecutingNull;

			return (_isExecuting != canExecuteParameter)
				&& (_canExecute?.Invoke(parameter) ?? true)
				&& _manualCanExecute;
		}

		public async void Execute(object parameter)
		{
			var isExecutingParameter = parameter ?? _isExecutingNull;

			if (_isExecuting == isExecutingParameter)
			{
				// This parameter is executing
				return;
			}

			try
			{
				_isExecuting = isExecutingParameter;
				CanExecuteChanged?.Invoke(this, null);
				if (_action != null)
				{
					_action(parameter);
				}
				else
				{
					await _actionTask(parameter);
				}
			}
			finally
			{
				_isExecuting = null;
				CanExecuteChanged?.Invoke(this, null);
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}
