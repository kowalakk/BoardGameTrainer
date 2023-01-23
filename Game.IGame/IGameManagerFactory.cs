namespace Game.IGame
{
    public interface IGameManagerFactory
    {
        public IGameManager CreateGameManager(IAiFactory aiFactory);
    }
}