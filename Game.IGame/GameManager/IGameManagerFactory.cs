namespace Game.IGame
{
    public interface IGameManagerFactory
    {
        public string Name { get; }
        public IGameManager Create(
            IAiFactory aiFactory, 
            IStopCondition stopCondition, 
            Dictionary<Player, bool> humanPlayers,
            Dictionary<Player, bool> showHints, 
            int numberOfHints);
    }
}