using System.Security.Cryptography;
using System.Text;

namespace Ujeby.Tools
{
	public static class Hashing
	{
		private static readonly MD5 _md5 = MD5.Create();
		private static readonly SHA1 _sha1 = SHA1.Create();
		private static readonly SHA256 _sha256 = SHA256.Create();

		public static string Hash(string source, HashAlgorithm hashAlg)
			=> FormatHash((hashAlg ?? hashAlg).ComputeHash(FormatSource(source)));

		public static string HashMd5(string source, MD5 md5 = null) => Hash(source, md5 ?? _md5);
		public static string HashSha1(string source, SHA1 sha1 = null) => Hash(source, sha1 ?? _sha1);
		public static string HashSha256(string source, SHA256 sha256 = null) => Hash(source, sha256 ?? _sha256);

		public static string SafeHashMd5(string source) => FormatHash(MD5.HashData(FormatSource(source)));
		public static string SafeHashSha1(string source) => FormatHash(SHA1.HashData(FormatSource(source)));
		public static string SafeHashSha256(string source) => FormatHash(SHA256.HashData(FormatSource(source)));

		private static string FormatHash(byte[] hash) => Convert.ToHexString(hash).ToLower();
		private static byte[] FormatSource(string source) => Encoding.ASCII.GetBytes(source);
	}
}
