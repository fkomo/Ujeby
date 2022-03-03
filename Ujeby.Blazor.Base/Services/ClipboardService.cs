using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ujeby.Blazor.Base.Services
{
	/// <summary>
	/// requires a secure origin — either HTTPS or localhost
	/// </summary>
	public sealed class ClipboardService
	{
		[Inject]
		private IJSRuntime JSRuntime { get; set; }

		public ClipboardService(IJSRuntime jsRuntime)
		{
			JSRuntime = jsRuntime;
		}

		public async Task<string> ReadTextAsync()
		{
			return await JSRuntime.InvokeAsync<string>("navigator.clipboard.readText");
		}

		public async Task WriteTextAsync(string text)
		{
			await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
		}
	}
}