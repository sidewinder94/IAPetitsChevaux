using PetitsChevaux.Game;

namespace PetitsChevaux.Plans
{
    public interface IPlan
    {
        void NextMove(Player player);
    }
}