using System.Collections.Generic;
using PetitsChevaux.Game;

namespace PetitsChevaux.Contract
{
    public abstract class ContractBase
    {
        public List<Player> Players;

        public ContractBase()
        {
            Players = new List<Player>();
        }
    }
}