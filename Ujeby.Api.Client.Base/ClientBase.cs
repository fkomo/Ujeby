
namespace Ujeby.Api.Client.Base
{
	public abstract class ClientBase
	{
		protected Uri BaseUri { get; private set; }

		public ClientBase(string baseUrl)
		{
			BaseUri = new Uri(baseUrl);
		}
	}
}