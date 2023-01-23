namespace Ai
{
    public interface IGameManagerFactory
    {
        public IGameManager GetGameManager(IAiFactory aiFactory);
    }
}