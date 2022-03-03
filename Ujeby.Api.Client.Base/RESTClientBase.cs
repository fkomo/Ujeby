using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Ujeby.Api.Client.Base
{
	public abstract class RESTClientBase : ClientBase
	{
		protected static HttpClient HttpClient { get; set; }

		public RESTClientBase(string baseUrl) : base(baseUrl)
		{
		}

		static RESTClientBase()
		{
			HttpClient = new HttpClient();
		}

		protected async Task<TResponse> Post<TRequest, TResponse>(string route, TRequest request)
		{
			var uri = new Uri(BaseUri, route);
			var response = await HttpClient.PostAsJsonAsync(uri, request);

			response.EnsureSuccessStatusCode();

			return await Task.Run(async () => JsonConvert.DeserializeObject<TResponse>(
				await response.Content.ReadAsStringAsync()
			));
		}

		protected async Task<TResponse> Get<TResponse>(string route = null)
		{
			var uri = new Uri(BaseUri, route);
			var response = await HttpClient.GetAsync(uri);

			response.EnsureSuccessStatusCode();

			return await Task.Run(async () => JsonConvert.DeserializeObject<TResponse>(
				await response.Content.ReadAsStringAsync()
			));
		}
	}
}