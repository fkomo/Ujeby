namespace Ujeby.Api.Base.Repositories
{
	public interface IRepository<TItem>
		where TItem : class
	{
		Task<TItem> GetAsync(string key);
		Task<TItem> UpdateAsync(string key, TItem item);
	}

	public interface IListRepository<TItem>
		where TItem : class
	{
		Task<TItem> AddAsync(string key, TItem item);
		Task<IEnumerable<TItem>> ListAsync(string key);
	}
}
