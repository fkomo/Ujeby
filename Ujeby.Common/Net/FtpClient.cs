using System;
using System.IO;
using System.Net;

namespace Ujeby.Common.Net
{
	/// <summary>
	/// https://www.codeproject.com/Tips/443588/Simple-Csharp-FTP-Class
	/// </summary>
	public class FtpClient : IDisposable
	{
		private readonly string? host = null;
		private readonly string? user = null;
		private readonly string? pass = null;
		private FtpWebRequest? ftpRequest = null;
		private FtpWebResponse? ftpResponse = null;
		private Stream? ftpStream = null;
		private readonly int bufferSize = 2048;

		public FtpClient(string hostIP, string userName, string password)
		{
			host = hostIP;
			user = userName;
			pass = password;
		}

		public void Download(string remoteFile, string localFile)
		{
			/* Create an FTP Request */
			ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
			/* Log in to the FTP Server with the User Name and Password Provided */
			ftpRequest.Credentials = new NetworkCredential(user, pass);
			/* When in doubt, use these options */
			ftpRequest.UseBinary = true;
			ftpRequest.UsePassive = true;
			ftpRequest.KeepAlive = true;
			/* Specify the Type of FTP Request */
			ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
			/* Establish Return Communication with the FTP Server */
			ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
			/* Get the FTP Server's Response Stream */
			ftpStream = ftpResponse.GetResponseStream();
			/* Open a File Stream to Write the Downloaded File */
			FileStream localFileStream = new(localFile, FileMode.Create);
			/* Buffer for the Downloaded Data */
			byte[] byteBuffer = new byte[bufferSize];
			int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
			/* Download the File by Writing the Buffered Data Until the Transfer is Complete */
			try
			{
				while (bytesRead > 0)
				{
					localFileStream.Write(byteBuffer, 0, bytesRead);
					bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
				}
			}
			catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			/* Resource Cleanup */
			localFileStream.Close();
			ftpStream.Close();
			ftpResponse.Close();
			ftpRequest = null;
		}

		public void Upload(string remoteFile, string localFile)
		{
			var localFileStream = null as FileStream;
            try
			{
				/* Create an FTP Request */
				ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
				/* Log in to the FTP Server with the User Name and Password Provided */
				ftpRequest.Credentials = new NetworkCredential(user, pass);
				/* When in doubt, use these options */
				ftpRequest.UseBinary = true;
				ftpRequest.UsePassive = true;
				ftpRequest.KeepAlive = true;
				/* Specify the Type of FTP Request */
				ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
				/* Establish Return Communication with the FTP Server */
				ftpStream = ftpRequest.GetRequestStream();
				/* Open a File Stream to Read the File for Upload */
				localFileStream = new FileStream(localFile, FileMode.Open);
				/* Buffer for the Downloaded Data */
				var byteBuffer = new byte[bufferSize];
				var bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
				/* Upload the File by Sending the Buffered Data Until the Transfer is Complete */

				while (bytesSent != 0)
				{
					ftpStream.Write(byteBuffer, 0, bytesSent);
					bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
				}
			}
			finally
			{
				/* Resource Cleanup */
				localFileStream?.Close();
				ftpStream?.Close();
				ftpRequest = null;
			}
		}

		public void CreateDirectory(string newDirectory)
		{
			/* Create an FTP Request */
			ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
			/* Log in to the FTP Server with the User Name and Password Provided */
			ftpRequest.Credentials = new NetworkCredential(user, pass);
			/* When in doubt, use these options */
			ftpRequest.UseBinary = true;
			ftpRequest.UsePassive = true;
			ftpRequest.KeepAlive = true;
			/* Specify the Type of FTP Request */
			ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
			/* Establish Return Communication with the FTP Server */
			ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
			/* Resource Cleanup */
			ftpResponse.Close();
			ftpRequest = null;
		}

		public void Dispose()
		{
			ftpStream?.Close();
			ftpRequest = null;
			ftpResponse = null;
		}
	}
}
