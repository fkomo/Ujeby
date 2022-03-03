
namespace Ujeby.Blazor.Base
{
	public interface IApplicationState
	{
		string HomeUrl { get; }

		string ApplicationTitle { get; }

		bool DarkMode { get; set; }

		event Action OnChange;
	}

	public abstract class ApplicationStateBase
	{
		public event Action OnChange;

		protected void NotifyStateChanged() => OnChange?.Invoke();
	}
}
