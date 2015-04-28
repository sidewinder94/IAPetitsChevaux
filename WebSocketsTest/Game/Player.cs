using System;
using System.Collections.Generic;
using System.Linq;

namespace PetitsChevaux.Game
{
    public class Player
    {
        private static int _nextId = 0;

        public readonly int Id;

        public Boolean Won
        {
            get
            {
                return Pawns.Any(p => p.Type == CaseType.EndGame && p.Position == 6);
            }
        }

        public readonly List<Pawn> Pawns = new List<Pawn>();

        public Action<Player> NextMove { private get; set; }

        public int StartCase
        {
            get { return Id * 14; }
        }


        public Player()
        {
            this.Id = _nextId++;
            for (int i = 0; i < Board.PawnsPerPlayer; i++)
            {
                Pawns.Add(new Pawn
                {

                    Type = CaseType.Square,
                    Position = 0

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

        public void MovePawn(Pawn pawn, int roll)
        {
            var newPosition = Board.Normalize(pawn.Position + roll);

            Board.Players.ForEach(player =>
            {

                var count = player.Pawns.Count(p => p.Position == pawn.Position && p.Type == pawn.Type);
                //Si un pion d'un autre joueur est sur la case de destination, il est renvoyé au "Box"
                if (count == 1)
                {
                    var removed = player.Pawns.First(p => p.Position == pawn.Position && p.Type == pawn.Type);
                    removed.Position = 0;
                    removed.Type = CaseType.Square;
                }

                //Si 2 pions du même joueur sur la même case, on ne peut atterrir dessus... on annule donc le déplacement
                if (count == 2)
                {
                    newPosition = pawn.Position;
                }

            });

            pawn.Position = newPosition;
        }
    }
}