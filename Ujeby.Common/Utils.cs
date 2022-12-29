using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Ujeby.Common
{
	public class Utils
	{
		public static void CopyTo(Stream source, Stream destination)
		{
			var bytes = new byte[4096];

			int cnt;
			while ((cnt = source.Read(bytes, 0, bytes.Length)) != 0)
				destination.Write(bytes, 0, cnt);
		}

		public static string GetFormattedXml(string xmlInput, string indent = "\t")
		{
			if (indent == null)
				throw new System.Exception($"invalid value: { nameof(indent) }");

			var xml = new XmlDocument();

			using var ms = new MemoryStream();
			var writerSettings = new XmlWriterSettings()
			{
				Indent = indent != null,
#pragma warning disable CS8601 // Possible null reference assignment.
				IndentChars = indent,
#pragma warning restore CS8601 // Possible null reference assignment.
				Encoding = System.Text.Encoding.UTF8,
				ConformanceLevel = ConformanceLevel.Document,
				OmitXmlDeclaration = true,
				//NewLineOnAttributes = true,
				NamespaceHandling = NamespaceHandling.OmitDuplicates,
			};

			using (var xmlWriter = XmlWriter.Create(ms, writerSettings))
			{
				xml.LoadXml(xmlInput);
				xml.WriteTo(xmlWriter);
			}

			ms.Position = 0;

			using var sr = new StreamReader(ms);
			return sr.ReadToEnd();
		}

		public static string? GetCurrentMethodName([CallerMemberName] string? caller = null)
		{
			return caller;
		}
	}
}
