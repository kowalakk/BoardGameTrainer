namespace Game.IGame
{
    public interface IStopCondition
    {
        public void Start();
        public void Advance();
        public bool Occured();
    }
}