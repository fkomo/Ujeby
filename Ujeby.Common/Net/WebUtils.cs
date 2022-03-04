using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Linq;
using System.Diagnostics;

namespace Ujeby.Common.Net
{
	public class WebUtils
	{
		public static string WebRequest(string url, string method = "GET", string? postData = null)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var responseData = WebRequestInternal(url, method, postData);

			stopwatch.Stop();

			return responseData;
		}

		public static string SilentWebRequest(string url, string method = "GET", string? postData = null)
		{
			return WebRequestInternal(url, method, postData);
		}

		public static string Post(string url, string? postData = null, Dictionary<string, string>? headers = null)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var responseData = WebRequestInternal(url, "POST", postData, headers);

			stopwatch.Stop();

			return responseData;
		}

		private static string WebRequestInternal(string url, string method = "GET", string? postData = null, 
			Dictionary<string, string>? headers = null)
		{
			var responseData = "";

			var cookieJar = new System.Net.CookieContainer();
			var hwrequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
			hwrequest.CookieContainer = cookieJar;
			hwrequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
			hwrequest.AllowAutoRedirect = true;
			hwrequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36";
			hwrequest.Timeout = 60000;
			hwrequest.Method = method;

			if (headers != null)
			{
				foreach (var header in headers)
					hwrequest.Headers.Add(header.Key, header.Value);
			}

			if (hwrequest.Method == "POST" && postData != null)
			{
				hwrequest.ContentType = "application/x-www-form-urlencoded";
				// Use UTF8Encoding instead of ASCIIEncoding for XML requests:
				var encoding = new System.Text.ASCIIEncoding();
				var postByteArray = encoding.GetBytes(postData);
				hwrequest.ContentLength = postByteArray.Length;
				var postStream = hwrequest.GetRequestStream();
				postStream.Write(postByteArray, 0, postByteArray.Length);
				postStream.Close();
			}

			var hwresponse = (System.Net.HttpWebResponse)hwrequest.GetResponse();
			if (hwresponse.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var responseStream = hwresponse.GetResponseStream();
				var myStreamReader = new System.IO.StreamReader(responseStream);
				responseData = myStreamReader.ReadToEnd();
			}
			hwresponse.Close();

			return responseData;
		}

		//public static Image DownloadImage(string url, out ImageFormat imageFormat)
		//{
		//	var stopwatch = new Stopwatch();
		//	stopwatch.Start();

		//	try
		//	{
		//		var webClient = new WebClient();
		//		webClient.Headers.Add("User-Agent: Other");

		//		var data = webClient.DownloadData(url);

		//		var mem = new MemoryStream(data);
		//		var image = Image.FromStream(mem);

		//		var extension = url.Substring(url.LastIndexOf('.') + 1);
		//		imageFormat = Tools.Graphics.GetImageFormat(extension);

		//		stopwatch.Stop();
		//		Log.WriteLine($"{ CurrentClassName }.{ Utils.GetCurrentMethodName() }({ url }) in { stopwatch.ElapsedMilliseconds }ms");

		//		return image;
		//	}
		//	catch (Exception ex)
		//	{
		//		stopwatch.Stop();
		//		Log.WriteLine($"{ CurrentClassName }.{ Utils.GetCurrentMethodName() }({ url }):null { ex.ToString() } in { stopwatch.ElapsedMilliseconds }ms");
		//	}

		//	imageFormat = ImageFormat.Jpeg;
		//	return null;
		//}

		public static string[] ScrapeImages(string url)
		{
			var images = new List<string>();

			var content = WebRequest(url);
			if (content == null)
				return images.ToArray();

			var currentUrlStart = content.IndexOf("=\"");
			while (currentUrlStart > 0)
			{
				currentUrlStart += "=\"".Length;
				var currentUrlEnd = content.IndexOf("\"", currentUrlStart);
				var currentUrl = content[currentUrlStart..currentUrlEnd];

				if (currentUrl.EndsWith(".jpg") || currentUrl.EndsWith(".png"))
					images.Add(currentUrl);

				currentUrlStart = content.IndexOf("=\"", currentUrlEnd);
			}

			return images.Distinct().ToArray();
		}
	}
}
