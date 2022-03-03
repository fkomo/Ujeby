namespace Ujeby.Api.Base.Repositories
{
	public interface ICRUD<TItem>
	{
		Task<TItem> CreateAsync(TItem item);

		Task<TItem> ReadAsync(TItem item);
		
		Task<TItem> UpdateAsync(TItem item);
		
		Task DeleteAsync(TItem item);
	}
}