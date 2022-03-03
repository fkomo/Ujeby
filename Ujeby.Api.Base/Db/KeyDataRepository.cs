using MySqlConnector;
using Ujeby.Api.Base.Repositories;
using Dapper;

namespace Ujeby.Api.Base.Db
{
	public class KeyDataRepository : ICRUD<KeyDataItem>
	{
		protected string ConnectionString { get; set; }

		public KeyDataRepository(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public async Task<KeyDataItem> CreateAsync(KeyDataItem item)
		{
			using (var connection = new MySqlConnection(ConnectionString))
			{
				await connection.OpenAsync();

				await connection.ExecuteAsync(
					"insert into bill.KeyData(`Key`, `Data`, Changed) values(@key, @data, @changed)",
					new
					{
						key = item.Key,
						data = item.Data,
						changed = DateTime.UtcNow,
					});

				return await ReadAsync(item);
			}
		}

		public Task DeleteAsync(KeyDataItem item)
		{
			throw new NotImplementedException();
		}

		public async Task<KeyDataItem> ReadAsync(KeyDataItem item)
		{
			using (var connection = new MySqlConnection(ConnectionString))
			{
				await connection.OpenAsync();

				return await connection.QuerySingleOrDefaultAsync<KeyDataItem>(
					"select `Key`, `Data` from bill.KeyData where `Key`=@key",
					new
					{
						key = item.Key,
					});
			}
		}

		public async Task<IEnumerable<KeyDataChangedItem>> ListAsync()
		{
			using (var connection = new MySqlConnection(ConnectionString))
			{
				await connection.OpenAsync();

				return await connection.QueryAsync<KeyDataChangedItem>(
					"select `Key`, `Data`, `Changed` from bill.KeyData order by `Changed` desc");
			}
		}

		public async Task<KeyDataItem> UpdateAsync(KeyDataItem item)
		{
			using (var connection = new MySqlConnection(ConnectionString))
			{
				await connection.OpenAsync();

				await connection.ExecuteAsync(
					"update bill.KeyData set `Key`=@key, `Data`=@data, Changed=@changed where `Key`=@key",
					new
					{
						key = item.Key,
						data = item.Data,
						changed = DateTime.UtcNow,
					});

				return await ReadAsync(item);
			}
		}
	}
}
