using System;
using System.Collections.Generic;

namespace PetitsChevaux.Game
{
    public class Player
    {
        private static int _nextId = 0;

        public readonly int Id;

        public readonly List<Pawn> Pawns = new List<Pawn>();

        public Action<Player> NextMove { private get; set; }

        public int StartCase
        {
            get { return Id * 14; }
        }


        public Player()
        {
            this.Id = _nextId++;
            for (int i = 0; i < Game.PawnsPerPlayer; i++)
            {
                Pawns.Add(new Pawn
                {
                    Position =
                    {
                        Type = CaseType.Square,
                        Position = 0
                    }
                });
            }
        }

        public void Play()
        {
            if (NextMove == null) throw new ArgumentException("No Method to determine " + this + " next move");
            NextMove(this);
        }


        #region Overrides of Object

        /// <summary>
        /// Retourne une chaîne qui représente l'objet actuel.
        /// </summary>
        /// <returns>
        /// Chaîne qui représente l'objet en cours.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Player {0}", Id + 1);
        }

        #endregion
    }
}