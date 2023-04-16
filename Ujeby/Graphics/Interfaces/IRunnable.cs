using Ujeby.Game.Sdl;

namespace Ujeby.Graphics.Interfaces
{
    public interface IRunnable
    {
        string Name { get; }

        void Run(Action<InputButton, InputButtonState> handleInput);
    }
}