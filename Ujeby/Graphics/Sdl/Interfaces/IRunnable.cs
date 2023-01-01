namespace Ujeby.Graphics.Sdl.Interfaces
{
	public interface IRunnable
	{
		string Name { get; }

		void Run(Func<bool> handleInput);
	}
}