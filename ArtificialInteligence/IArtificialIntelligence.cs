namespace ArtificialInteligence
{
    public interface IArtificialIntelligence<Game, State, ModuleData>
        // chyba nie powinno być parametryzowane przez Game, tylko funkcje powinny przyjmować parametr typu IGame???
    {
        List<(Action, double)> MoveAssessment(Game game, State state, ModuleData moduleData);
        Action ChooseMove(Game game, State state);
    }
}