using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Ujeby.Common
{
	public class Serialization
	{
		public static string? Serialize<T>(T obj)
		{
			if (obj == null)
				return null;

			var serializer = new DataContractSerializer(obj.GetType());
			using var writer = new StringWriter();
			using var stm = new XmlTextWriter(writer);
			serializer.WriteObject(stm, obj);
			return writer.ToString();
		}

		public static T? Deserialize<T>(string serialized)
		{
			if (serialized == null)
				return default;

			var serializer = new DataContractSerializer(typeof(T));
			using var reader = new StringReader(serialized);
			using var stm = new XmlTextReader(reader);
			return (T)serializer.ReadObject(stm);
		}

		public static byte[] Compress(string stringToCompress)
		{
			var bytes = Encoding.UTF8.GetBytes(stringToCompress);

			using var msi = new MemoryStream(bytes);
			using var mso = new MemoryStream();
			using (var gs = new GZipStream(mso, CompressionMode.Compress))
				Utils.CopyTo(msi, gs);

			return mso.ToArray();
		}

		public static string Decompress(byte[] bytesToDecompress)
		{
			using var msi = new MemoryStream(bytesToDecompress);
			using var mso = new MemoryStream();
			using (var gs = new GZipStream(msi, CompressionMode.Decompress))
				Utils.CopyTo(gs, mso);

			return Encoding.UTF8.GetString(mso.ToArray());
		}
	}
}
