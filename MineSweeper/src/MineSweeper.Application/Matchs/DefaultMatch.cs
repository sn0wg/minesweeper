using MineSweeper.Application.Matchs;
using MineSweeper.Domain.Boards;
using MineSweeper.Domain.Fields;

namespace MineSweeper.Application.Match
{
    internal class DefaultMatch
    {
        private readonly IBoard _board;

        public MatchDifficultyEnum Difficulty { get; }
        public MatchResultEnum Result { get; private set; }
        public MatchStateEnum State { get; private set; }
        public int RemainingBombs { get; private set; }
        public int RemainingFields { get; private set; }
        public IReadOnlyCollection<IReadOnlyCollection<IField>> Fields => _board.Fields;

        internal DefaultMatch(MatchDifficultyEnum difficulty, IBoardFabric boardFabric)
        {
            var size = difficulty switch
            {
                MatchDifficultyEnum.Easy => 5,
                MatchDifficultyEnum.Normal => 10,
                MatchDifficultyEnum.Hard => 20,
                _ => throw new NotImplementedException(),
            };

            var bombs = (int)(size * size * 0.10);

            _board = boardFabric.Create(size, size, bombs);
            Difficulty = difficulty;
            Result = MatchResultEnum.None;
            State = MatchStateEnum.Waiting;
            RemainingBombs = _board.BombQty;
            RemainingFields = (size * size) - RemainingBombs;
        }

        public void Reveal(int x, int y)
        {
            if (State == MatchStateEnum.Waiting)
                State = MatchStateEnum.Running;
            else if (State == MatchStateEnum.Finished)
                return;

            var field = _board.Fields
                .ElementAtOrDefault(y)? // Line
                .ElementAtOrDefault(x); // Column

            if (field == null)
                return;

            var reveleadFields = field.Reveal();

            RemainingFields -= reveleadFields;

            if (field.State == FieldState.Exploded)
            {
                State = MatchStateEnum.Finished;
                Result = MatchResultEnum.Loss;
                return;
            }

            if (RemainingFields > 0)
                return;

            State = MatchStateEnum.Finished;
            Result = MatchResultEnum.Won;
        }

        public void Mark(int x, int y, FieldFlag flag)
        {
            if (State != MatchStateEnum.Running || RemainingBombs == 0)
                return;

            var field = _board.Fields
                .ElementAtOrDefault(y)? // Line
                .ElementAtOrDefault(x); // Column

            if (field == null || field.Flag == flag) 
                return;

            field.Flag = flag;

            switch (flag)
            {
                case FieldFlag.None:
                    RemainingBombs++;
                    break;
                case FieldFlag.Bomb:
                    RemainingBombs--;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
