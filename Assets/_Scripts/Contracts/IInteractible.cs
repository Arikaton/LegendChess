using LegendChess.Enums;

namespace LegendChess.Contracts
{
    public interface IInteractible
    {
        void OnInteract(SquadType squadType);
    }
}