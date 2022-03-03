using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ujeby.Blazor.Base.ViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public ViewModelBase()
		{

		}

		public bool IsValid { get; set; }

		#region Implementation of INotifyPropertyChanged

		/// <inheritdoc />
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Called when <see cref="INotifyPropertyChanged.PropertyChanged"/> occurs
		/// to invoke attached handlers.
		/// </summary>
		/// <param name="propertyName"></param>
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion Implementation of INotifyPropertyChanged
	}
}
