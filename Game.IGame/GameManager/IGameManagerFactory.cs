namespace Game.IGame
{
    public interface IGameManagerFactory
    {
        public IGameManager Create(IAiFactory aiFactory, IStopCondition stopCondition);
    }
}