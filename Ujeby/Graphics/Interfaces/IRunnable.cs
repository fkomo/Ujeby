using Ujeby.Game.Sdl;

namespace Ujeby.Graphics.Interfaces
{
    public interface IRunnable
    {
        string Name { get; }

        void Run(Func<InputButton, InputButtonState, bool> handleInput);
    }
}