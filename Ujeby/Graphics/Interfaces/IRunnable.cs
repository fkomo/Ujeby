namespace Ujeby.Graphics.Interfaces
{
    public interface IRunnable
    {
        string Name { get; }

        void Run(Func<bool> handleInput);
    }
}