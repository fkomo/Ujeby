namespace Ujeby.Graphics.Interfaces
{
    public interface ILoop
    {
		abstract void Init();
		abstract void Update();
		abstract void Render();
		abstract void Destroy();
	}
}