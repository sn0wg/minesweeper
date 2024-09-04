using MineSweeper.Application.Fields;
using MineSweeper.Application.Match;
using MineSweeper.Domain.Fields;

namespace MineSweeper.Application.Matchs
{
    public interface IMatch
    {
        MatchDifficultyEnum Difficulty { get; }
        MatchResultEnum Result { get; }
        MatchStateEnum State { get; }
        IReadOnlyCollection<IReadOnlyCollection<IApplicationField>> Fields { get; }
        int RemainingBombs { get; }
        int RemainingFields { get; }
        void Reveal(int x, int y);
        void Mark(int x, int y, FieldFlag flag);
    }
}
