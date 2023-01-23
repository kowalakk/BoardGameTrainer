namespace Game.IGame
{
    public abstract class GameManagerFactory
    {
        protected abstract IGameManager MakeGameManager();

        public IGameManager GetGameManager()
        {
            return this.MakeGameManager();
        }
    }
}