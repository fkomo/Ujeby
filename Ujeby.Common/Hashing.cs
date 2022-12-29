using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ujeby.Common
{
	public class Hashing
	{
		public static string? GetHash(string input)
		{
			if (input == null)
				return null;

			using var sha256 = SHA256.Create();
			var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
			return string.Concat(hash.Select(b => b.ToString("x2")));
		}
	}
}
