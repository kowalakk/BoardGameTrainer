using Game.IGame;

namespace Game.Checkers
{
    public partial class Checkers : IGame<ICheckersAction, CheckersState, ICheckersInputState>
    {
        public IEnumerable<ICheckersAction> PossibleActions(CheckersState state)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleActions = (new(), 0);
            if (state.CurrentPlayer == Player.One)
            {
                for (int field = 0; field < CheckersState.fieldCount; field++)
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.WhitePawn)
                        UpdateActions(ref possibleActions, PossibleWhitePawnActions(state, field, possibleActions.maxCapturesCount));
                    else if (piece == Piece.WhiteCrowned)
                        UpdateActions(ref possibleActions, PossibleCrownedActions(state, field, possibleActions.maxCapturesCount));
                }
            }
            else //state.CurrentPlayer == Player.Black
            {
                for (int field = 0; field < CheckersState.fieldCount; field++)
                {
                    Piece piece = state.GetPieceAt(field);
                    if (piece == Piece.BlackPawn)
                        UpdateActions(ref possibleActions, PossibleBlackPawnActions(state, field, possibleActions.maxCapturesCount));
                    else if (piece == Piece.BlackCrowned)
                        UpdateActions(ref possibleActions, PossibleCrownedActions(state, field, possibleActions.maxCapturesCount));
                }
            }
            return possibleActions.list;
        }

        private (List<ICheckersAction>, int) PossibleWhitePawnActions(
            CheckersState state, int field, int minCapturesCount)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, field, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;

            //no captures - simple move
            if (minCapturesCount > 0)
                return (new List<ICheckersAction>(), 0);
            List<ICheckersAction> possibleMoves = new();

            int? neighbour = CheckersState.neighbours[field][0];
            if (neighbour is not null && state.GetPieceAt((int)neighbour) == Piece.None)
                possibleMoves.Add(new MoveAction(field, (int)neighbour));

            neighbour = CheckersState.neighbours[field][1];
            if (neighbour is not null && state.GetPieceAt((int)neighbour) == Piece.None)
                possibleMoves.Add(new MoveAction(field, (int)neighbour));

            return (possibleMoves, 0);
        }

        private (List<ICheckersAction>, int) PossibleBlackPawnActions(
            CheckersState state, int field, int minCapturesCount)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = PossiblePawnCaptures(state, field, minCapturesCount);
            if (possibleCaptures.list.Any())
                return possibleCaptures;

            //no captures - simple move
            if (minCapturesCount > 0)
                return (new List<ICheckersAction>(), 0);
            List<ICheckersAction> possibleMoves = new();

            int? neighbour = CheckersState.neighbours[field][2];
            if (neighbour is not null && state.GetPieceAt((int)neighbour) == Piece.None)
                possibleMoves.Add(new MoveAction(field, (int)neighbour));

            neighbour = CheckersState.neighbours[field][3];
            if (neighbour is not null && state.GetPieceAt((int)neighbour) == Piece.None)
                possibleMoves.Add(new MoveAction(field, (int)neighbour));

            return (possibleMoves, 0);
        }

        private (List<ICheckersAction>, int) PossiblePawnCaptures(
            CheckersState state, int field, int minCapturesCount)
        {
            (List<ICheckersAction>, int) possibleCaptures = (new(), minCapturesCount);

            Piece capturer = state.GetPieceAt(field);
            int?[] neighbours = CheckersState.neighbours[field];
            for (int direction = 0; direction < 4; direction++)
            {
                if (neighbours[direction] is null)
                    continue;
                int neighbour = (int)neighbours[direction]!;
                Piece target = state.GetPieceAt(neighbour);
                if (HaveOppositeColors(capturer, target))
                {
                    int? finish = CheckersState.neighbours[neighbour][direction];
                    if (finish != null && state.GetPieceAt((int)finish) == Piece.None)
                    {
                        CaptureAction action = new(field, neighbour, (int)finish);
                        CheckersState tmpState = PerformTemporaryCapture(action, state);
                        (List<ICheckersAction>, int) furtherCaptures = PossiblePawnCaptures(tmpState, (int)finish, minCapturesCount - 1);
                        CombineCaptures(action, ref furtherCaptures);
                        UpdateActions(ref possibleCaptures, furtherCaptures);
                    }
                }
            }
            return possibleCaptures;
        }

        private static (List<ICheckersAction>, int) PossibleCrownedActions(
            CheckersState state, int field, int minCapturesCount)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = (new(), minCapturesCount);
            List<ICheckersAction> possibleMoves = new();

            Piece movingPiece = state.GetPieceAt(field);
            int?[] neighbours = CheckersState.neighbours[field];
            for (int direction = 0; direction < 4; direction++)
            {
                int? targetedField = neighbours[direction];
                while (targetedField.HasValue)
                {
                    Piece target = state.GetPieceAt(targetedField.Value);
                    if (target == Piece.None)
                    {
                        possibleMoves.Add(new MoveAction(field, targetedField.Value));
                    }
                    else // there is some piece on the way
                    {
                        if (HaveOppositeColors(movingPiece, target)) // there is some piece to capture
                        {
                            int? fieldBehindTarget = CheckersState.neighbours[targetedField.Value][direction];
                            while (fieldBehindTarget.HasValue && state.GetPieceAt(fieldBehindTarget.Value) == Piece.None)
                            {
                                CaptureAction action = new(field, targetedField.Value, fieldBehindTarget.Value);
                                CheckersState tmpState = PerformTemporaryCapture(action, state);
                                (List<ICheckersAction>, int) furtherCaptures
                                    = PossibleCrownedCaptures(tmpState, fieldBehindTarget.Value, minCapturesCount - 1);
                                CombineCaptures(action, ref furtherCaptures);
                                UpdateActions(ref possibleCaptures, furtherCaptures);

                                fieldBehindTarget = CheckersState.neighbours[fieldBehindTarget.Value][direction];
                            }
                        }
                        break;
                    }
                    targetedField = CheckersState.neighbours[targetedField.Value][direction];
                }
            }
            if (possibleCaptures.list.Any())
                return possibleCaptures;

            //no captures found so far - simple moves through diagonals
            return (possibleMoves, 0);
        }

        private static (List<ICheckersAction>, int) PossibleCrownedCaptures(
            CheckersState state, int field, int minCapturesCount)
        {
            (List<ICheckersAction> list, int maxCapturesCount) possibleCaptures = (new(), minCapturesCount);

            Piece movingPiece = state.GetPieceAt(field);
            int?[] neighbours = CheckersState.neighbours[field];
            for (int direction = 0; direction < 4; direction++)
            {
                int? targetedField = neighbours[direction];
                while (targetedField.HasValue)
                {
                    Piece target = state.GetPieceAt(targetedField.Value);
                    if (target != Piece.None) // there is some piece on the way
                    {
                        if (HaveOppositeColors(movingPiece, target)) // there is some piece to capture
                        {
                            int? fieldBehindTarget = CheckersState.neighbours[targetedField.Value][direction];
                            while (fieldBehindTarget.HasValue && state.GetPieceAt(fieldBehindTarget.Value) == Piece.None)
                            {
                                CaptureAction action = new(field, targetedField.Value, fieldBehindTarget.Value);
                                CheckersState tmpState = PerformTemporaryCapture(action, state);
                                (List<ICheckersAction>, int) furtherCaptures
                                    = PossibleCrownedCaptures(tmpState, fieldBehindTarget.Value, minCapturesCount - 1);
                                CombineCaptures(action, ref furtherCaptures);
                                UpdateActions(ref possibleCaptures, furtherCaptures);

                                fieldBehindTarget = CheckersState.neighbours[fieldBehindTarget.Value][direction];
                            }
                        }
                        break;
                    }
                    targetedField = CheckersState.neighbours[targetedField.Value][direction];
                }
            }
            return possibleCaptures;
        }

        private static bool CombineCaptures(CaptureAction action,
            ref (List<ICheckersAction> list, int maxCapturesCount) furtherCaptures)
        {
            if (furtherCaptures.list.Any())
            {
                    foreach (CaptureAction furtherCapture in furtherCaptures.list.Cast<CaptureAction>())
                    {
                        furtherCapture.CombineCapture(action.Start, action.Captures.First!.Value.Captured);
                    }
                    furtherCaptures.maxCapturesCount++;
                    return true;
// only MoveActions found
            }
            furtherCaptures = (new List<ICheckersAction>() { action }, 1);
            return false;
        }

        private static bool UpdateActions(
            ref (List<ICheckersAction> list, int maxCapturesCount) possibleActions,
            (List<ICheckersAction> list, int maxCapturesCount) newActions)
        {
            if (newActions.maxCapturesCount == possibleActions.maxCapturesCount) // add actions of same length
            {
                possibleActions.list.AddRange(newActions.list);
                return true;
            }
            if (newActions.maxCapturesCount > possibleActions.maxCapturesCount) // trim to longest actions only
            {
                possibleActions = newActions;
                return true;
            }
            return false; // no changes
        }

        private static bool HaveOppositeColors(Piece capturer, Piece target)
        {
            if (capturer == Piece.None || target == Piece.None) return false;
            if (capturer == Piece.CapturedPiece || target == Piece.CapturedPiece) return false;
            bool isCapturerWhite = capturer == Piece.WhitePawn || capturer == Piece.WhiteCrowned;
            bool isTargetBlack = target == Piece.BlackPawn || target == Piece.BlackCrowned;
            return (isCapturerWhite && isTargetBlack) || (!isCapturerWhite && !isTargetBlack);
        }
    }
}