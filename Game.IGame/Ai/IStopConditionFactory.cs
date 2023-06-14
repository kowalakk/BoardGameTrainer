namespace Game.IGame
{
    public interface IStopConditionFactory
    {
        public IStopCondition Create(int param);
    }
}