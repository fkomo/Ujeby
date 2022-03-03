namespace Ujeby.Api.Base
{
	public class KeyDataItem
	{
		public string Key { get; set; }
		public string Data { get; set; }
	}

	public class KeyDataChangedItem : KeyDataItem
	{
		public DateTime Changed { get; set; }
	}
}
