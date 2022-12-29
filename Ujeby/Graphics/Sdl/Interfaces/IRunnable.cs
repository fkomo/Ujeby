namespace Ujeby.Graphics.Sdl.Interfaces
{
	public interface IRunnable
	{
		void Run(Func<bool> handleInput);
	}
}